#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：支付宝支付模快 —— 签名加密助手
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-3-28
*       
*****************************************************************************/

#endregion

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OSS.PaySdk.Ali.SysTools
{
    /// <summary>
    ///   rsa加密
    /// </summary>
    public class ZPayRsaAssist
    {

        internal string AppId { get; set; }

        private readonly RSACryptoServiceProvider m_PublicRsa;
        private readonly RSACryptoServiceProvider m_PrivateRsa ;
        //private readonly string m_SignType;
        private readonly string m_Charset;

        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="privateKeyPem"></param>
        /// <param name="publicKeyPem"></param>
        /// <param name="charset"></param>
        public ZPayRsaAssist(string privateKeyPem,string publicKeyPem,string charset)
        {
            m_PublicRsa = CreateRsaProviderFromPublicKey(publicKeyPem);
            m_PrivateRsa = CreateRsaProviderFromPrivateKey(privateKeyPem);
            //m_SignType = signType;
            m_Charset = charset;
        }

        /// <summary>
        ///  验签操作
        /// </summary>
        /// <param name="signContent"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public  bool CheckSign(string signContent, string sign)
        {
            var bytes = Encoding.GetEncoding(m_Charset).GetBytes(signContent);
            var bVerifyResultOriginal = m_PublicRsa.VerifyData(bytes
                , Convert.FromBase64String(sign), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return bVerifyResultOriginal;
        }

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GenerateSign(string data)
        {
            if (null == m_PrivateRsa)
                throw new Exception("您使用的私钥格式错误，请检查RSA私钥配置" + ",charset = " + m_Charset);

            var dataBytes = Encoding.GetEncoding(m_Charset).GetBytes(data);
            var signatureBytes = m_PrivateRsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            return Convert.ToBase64String(signatureBytes);
        }


        #region 通过公钥创建Rsa对象

        private static RSACryptoServiceProvider CreateRsaProviderFromPublicKey(string publicKeyString)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            var x509key = Convert.FromBase64String(publicKeyString);

            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            using (var mem = new MemoryStream(x509key))
            {
                using (var binr = new BinaryReader(mem))  //wrap Memory Stream with BinaryReader for easy reading
                {
                    byte bt = 0;
                    ushort twobytes = 0;

                    twobytes = binr.ReadUInt16();
                    switch (twobytes)
                    {
                        case 0x8130:
                            binr.ReadByte();    //advance 1 byte
                            break;
                        case 0x8230:
                            binr.ReadInt16();   //advance 2 bytes
                            break;
                        default:
                            return null;
                    }

                    var seq = binr.ReadBytes(15);
                    if (!CompareBytearrays(seq, SeqOID))    //make sure Sequence for OID is correct
                        return null;

                    twobytes = binr.ReadUInt16();
                    switch (twobytes)
                    {
                        case 0x8103:
                            binr.ReadByte();    //advance 1 byte
                            break;
                        case 0x8203:
                            binr.ReadInt16();   //advance 2 bytes
                            break;
                        default:
                            return null;
                    }

                    bt = binr.ReadByte();
                    if (bt != 0x00)     //expect null byte next
                        return null;

                    twobytes = binr.ReadUInt16();
                    switch (twobytes)
                    {
                        case 0x8130:
                            binr.ReadByte();    //advance 1 byte
                            break;
                        case 0x8230:
                            binr.ReadInt16();   //advance 2 bytes
                            break;
                        default:
                            return null;
                    }

                    twobytes = binr.ReadUInt16();

                    byte lowbyte=0x00;
                    byte highbyte = 0x00;

                    switch (twobytes)
                    {
                        case 0x8102:
                            lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus
                            break;
                        case 0x8202:
                            highbyte = binr.ReadByte(); //advance 2 bytes
                            lowbyte = binr.ReadByte();
                            break;
                        default:
                            return null;
                    }
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                    var modsize = BitConverter.ToInt32(modint, 0);

                    var firstbyte = binr.PeekChar();
                    if (firstbyte == 0x00)
                    {   //if first byte (highest order) of modulus is zero, don't include it
                        binr.ReadByte();    //skip this null byte
                        modsize -= 1;   //reduce modulus buffer size by 1
                    }

                    var modulus = binr.ReadBytes(modsize);   //read the modulus bytes
                    if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data
                        return null;
                    var expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
                    var exponent = binr.ReadBytes(expbytes);

                    // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                    var rsaKeyInfo = new RSAParameters
                    {
                        Modulus = modulus,
                        Exponent = exponent
                    };
                    var rsa = new RSACryptoServiceProvider();
                    rsa.ImportParameters(rsaKeyInfo);

                    return rsa;
                }

            }

        }
        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            var i = 0;
            foreach (var c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        #endregion

        #region 通过私钥创建Rsa对象

        private static RSACryptoServiceProvider CreateRsaProviderFromPrivateKey(string strKey)
        {
            var data = Convert.FromBase64String(strKey);
            return DecodeRSAPrivateKey(data);
        }

        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            // --------- Set up stream to decode the asn.1 encoded RSA private key ------
            var mem = new MemoryStream(privkey);
            var binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading
            try
            {
                var twobytes = binr.ReadUInt16();
                switch (twobytes)
                {
                    case 0x8130:
                        binr.ReadByte();    //advance 1 byte
                        break;
                    case 0x8230:
                        binr.ReadInt16();    //advance 2 bytes
                        break;
                    default:
                        return null;
                }

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                var bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------ all private key components are Integer sequences ----
                var elems = GetIntegerSize(binr);
                var modulus = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                var E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                var D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                var P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                var Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                var DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                var DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                var IQ = binr.ReadBytes(elems);
                
                var CspParameters = new CspParameters {Flags = CspProviderFlags.UseMachineKeyStore};
                const int bitLen = 2048;

                var rsaParams = new RSAParameters
                {
                    Modulus = modulus,
                    Exponent = E,
                    D = D,
                    P = P,
                    Q = Q,
                    DP = DP,
                    DQ = DQ,
                    InverseQ = IQ
                };

                var rsa = new RSACryptoServiceProvider(bitLen, CspParameters);
                rsa.ImportParameters(rsaParams);
                return rsa;
            }
            finally
            {
                binr.Dispose();
                mem.Dispose();
            }
        }
        private static int GetIntegerSize(BinaryReader binr)
        {
            var bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            var count = 0;
            switch (bt)
            {
                case 0x81:
                    count = binr.ReadByte();	// data size in next byte
                    break;
                case 0x82:
                    var highbyte = binr.ReadByte();
                    var lowbyte = binr.ReadByte();
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    count = BitConverter.ToInt32(modint, 0);
                    break;
                default:
                    count = bt;     // we already have the data size
                    break;
            }

            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }

        #endregion
    }
}
