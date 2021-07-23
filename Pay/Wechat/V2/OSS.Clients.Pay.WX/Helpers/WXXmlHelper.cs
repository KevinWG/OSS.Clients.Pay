#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— xml和dictionary 辅助转化类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-23
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OSS.Clients.Pay.WX.Helpers
{
    internal static class WXXmlHelper
    {
        /// <summary>
        ///  字典转到xml
        /// </summary>
        /// <param name="dics"></param>
        /// <returns></returns>
       public static string ProduceXml(this SortedDictionary<string,object> dics )
        {
            var xml = new StringBuilder();
            xml.Append("<xml>");
            foreach (var item in dics)
            {
                var value = item.Value?.ToString();
                if (string.IsNullOrEmpty(value))
                    continue;
                
                if (item.Value is int
                    || item.Value is Int64
                    || item.Value is double
                    || item.Value is float)
                {
                    xml.Append("<").Append(item.Key).Append(">")
                        .Append(value)
                        .Append("</").Append(item.Key).Append(">");
                }
                else
                {
                    xml.Append("<").Append(item.Key).Append(">")
                        .Append("<![CDATA[")
                        .Append(value)
                        .Append("]]>")
                        .Append("</").Append(item.Key).Append(">");
                }
            }
            xml.Append("</xml>");
            return xml.ToString();
        }


        /// <summary>
        /// 把xml文本转化成字典对象
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <param name="xmlDoc">通过字符串转化后的xml对象</param>
        /// <returns></returns>
        public static SortedDictionary<string, object> ChangXmlToDir(string xmlStr,ref XmlDocument xmlDoc)
        {
            if (string.IsNullOrEmpty(xmlStr))
            {
                return null;
            }
            var dirs = new SortedDictionary<string, object>();

            xmlDoc = new XmlDocument {XmlResolver = null};

            xmlDoc.LoadXml(xmlStr);
            var xmlNode = xmlDoc.FirstChild;
            var nodes = xmlNode.ChildNodes;

            foreach (XmlNode xn in nodes)
            {
                var xe = (XmlElement)xn;
                dirs[xe.Name] = xe.InnerText;
            }

            return dirs;
        }

        
        private static readonly Random _rnd = new Random();
        private static readonly char[] _arrChar = {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'a', 'b', 'd', 'c', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'p', 'r', 'q', 's', 't', 'u', 'v',
            'w', 'z', 'y', 'x',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Q', 'P', 'R', 'T', 'S', 'V', 'U',
            'W', 'X', 'Y', 'Z'
        };
        /// <summary>
        /// 生成随机串
        /// </summary>
        /// <returns></returns>
        public static string GenerateNonceStr()
        {
            var num = new StringBuilder();

            for (var i = 0; i < 8; i++)
            {
                num.Append(_arrChar[_rnd.Next(0, 59)].ToString());
            }
            return num.ToString();
        }
    }
}