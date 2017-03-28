

#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：支付宝支付模快 —— 支付宝官方签名加密类
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

namespace OSS.PayCenter.ZFB.SysTools
{
    public class ZPaySignature
    {
        public static string RSASignCharSet(string data, string privateKeyPem, string charset, string signType)
        {
            RSACryptoServiceProvider rsaCsp = LoadCertificateFile(privateKeyPem, signType);

            byte[] dataBytes = null;
            dataBytes = string.IsNullOrEmpty(charset)
                ? Encoding.UTF8.GetBytes(data)
                : Encoding.GetEncoding(charset).GetBytes(data);

            byte[] signatureBytes = rsaCsp.SignData(dataBytes, "RSA2".Equals(signType) ? "SHA256" : "SHA1");

            return Convert.ToBase64String(signatureBytes);
        }

        public static bool RSACheckContent(string signContent, string sign, string publicKeyPem, string charset, string signType)
        {
            try
            {
                if ("RSA2".Equals(signType))
                {
                    string sPublicKeyPEM = File.ReadAllText(publicKeyPem);

                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                    rsa.PersistKeyInCsp = false;
                   
                    RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsa, sPublicKeyPEM);
                   
                    bool bVerifyResultOriginal = rsa.VerifyData(Encoding.GetEncoding(charset).GetBytes(signContent), "SHA256", Convert.FromBase64String(sign));
                    return bVerifyResultOriginal;

                }
                else
                {
                    string sPublicKeyPEM = File.ReadAllText(publicKeyPem);
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                    rsa.PersistKeyInCsp = false;
                    RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsa, sPublicKeyPEM);

                    //SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                    bool bVerifyResultOriginal = rsa.VerifyData(Encoding.GetEncoding(charset).GetBytes(signContent),"SHA1", Convert.FromBase64String(sign));
                    return bVerifyResultOriginal;
                }
            }
            catch
            {
                return false;
            }

        }
      

        //public static string encryptAndSign(string bizContent, string alipayPublicKey,
        //                                string cusPrivateKey, string charset, bool isEncrypt,
        //                                bool isSign, string signType)
        //{
        //    StringBuilder sb = new StringBuilder();
       
        //    sb.Append("<?xml version=\"1.0\" encoding=\"" + charset + "\"?>");
        //    if (isEncrypt)
        //    {// 加密
        //        sb.Append("<alipay>");
        //        String encrypted = RSAEncrypt(bizContent, alipayPublicKey, charset,false);
        //        sb.Append("<response>" + encrypted + "</response>");
        //        sb.Append("<encryption_type>" + signType + "</encryption_type>");
        //        if (isSign)
        //        {
        //            String sign = RSASignCharSet(encrypted, cusPrivateKey, charset, signType);
        //            sb.Append("<sign>" + sign + "</sign>");
        //            sb.Append("<sign_type>" + signType + "</sign_type>");
        //        }
        //        sb.Append("</alipay>");
        //    }
        //    else if (isSign)
        //    {// 不加密，但需要签名
        //        sb.Append("<alipay>");
        //        sb.Append("<response>" + bizContent + "</response>");
        //        String sign = RSASignCharSet(bizContent, cusPrivateKey, charset, signType);
        //        sb.Append("<sign>" + sign + "</sign>");
        //        sb.Append("<sign_type>" + signType + "</sign_type>");
        //        sb.Append("</alipay>");
        //    }
        //    else
        //    {// 不加密，不加签
        //        sb.Append(bizContent);
        //    }
        //    return sb.ToString();
        //}
        
        //public static string RSAEncrypt(string content, string publicKeyPem, string charset, bool keyFromFile)
        //{
        //    try
        //    {
        //        string sPublicKeyPEM;
        //        if (keyFromFile)
        //        {
        //            sPublicKeyPEM = File.ReadAllText(publicKeyPem);
        //        }
        //        else
        //        {
        //            sPublicKeyPEM = "-----BEGIN PUBLIC KEY-----\r\n";
        //            sPublicKeyPEM += publicKeyPem;
        //            sPublicKeyPEM += "-----END PUBLIC KEY-----\r\n\r\n";
        //        }
        //        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        //        rsa.PersistKeyInCsp = false;
        //        RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsa, sPublicKeyPEM);
            
        //        byte[] data = Encoding.GetEncoding(charset).GetBytes(content);
        //        int maxBlockSize = rsa.KeySize / 8 - 11; //加密块最大长度限制
        //        if (data.Length <= maxBlockSize)
        //        {
        //            byte[] cipherbytes = rsa.Encrypt(data, false);
        //            return Convert.ToBase64String(cipherbytes);
        //        }
        //        MemoryStream plaiStream = new MemoryStream(data);
        //        MemoryStream crypStream = new MemoryStream();
        //        Byte[] buffer = new Byte[maxBlockSize];
        //        int blockSize = plaiStream.Read(buffer, 0, maxBlockSize);
        //        while (blockSize > 0)
        //        {
        //            Byte[] toEncrypt = new Byte[blockSize];
        //            Array.Copy(buffer, 0, toEncrypt, 0, blockSize);
        //            Byte[] cryptograph = rsa.Encrypt(toEncrypt, false);
        //            crypStream.Write(cryptograph, 0, cryptograph.Length);
        //            blockSize = plaiStream.Read(buffer, 0, maxBlockSize);
        //        }

        //        return Convert.ToBase64String(crypStream.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("EncryptContent = " + content + ",charset = " + charset, ex);
        //    }
        //}
        
        private static byte[] GetPem(string type, byte[] data)
        {
            string pem = Encoding.UTF8.GetString(data);

            string header = String.Format("-----BEGIN {0}-----\\n", type);
            string footer = String.Format("-----END {0}-----", type);
            int start = pem.IndexOf(header) + header.Length;
            int end = pem.IndexOf(footer, start);
            string base64 = pem.Substring(start, (end - start));

            return Convert.FromBase64String(base64);
        }

        private static RSACryptoServiceProvider LoadCertificateFile(string filename, string signType)
        {
            using (System.IO.FileStream fs = System.IO.File.OpenRead(filename))
            {
                byte[] data = new byte[fs.Length];
                byte[] res = null;
                fs.Read(data, 0, data.Length);
                if (data[0] != 0x30)
                {
                    res = GetPem("RSA PRIVATE KEY", data);
                }
                try
                {
                    RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(res, signType);
                    return rsa;
                }
                catch (Exception ex)
                {
                }
                return null;
            }
        }

        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey, string signType)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // --------- Set up stream to decode the asn.1 encoded RSA private key ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------ all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);


                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                CspParameters CspParameters = new CspParameters();
                CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;

                int bitLen = 1024;
                if ("RSA2".Equals(signType))
                {
                    bitLen = 2048;
                }

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(bitLen, CspParameters);
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                binr.Dispose();
            }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }
    }
}
