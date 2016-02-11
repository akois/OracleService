using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using Encrypt;

namespace OracleService
{
    [ServiceContract]
    public class OracleService
    {
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }


        [WebGet(UriTemplate = "Decrypt?mode={mode}&cipher={strCipher}")]
        [OperationContract]
        public string Decrypt(string mode, string strCipher)
        {
            //

//5665727952616e646f6d495631323334ab30cdf1e3fbc97c41c1e44cb22e33ed
//ab30cdf1e3fbc97c41c1e44cb22e33ed40bff4855d03ea9bd4ad8624720be657

            /*
            cipher "5665727952616e646f6d495631323334ab30cdf1e3fbc97c41c1e44cb22e33ed40bff4855d03ea9bd4ad8624720be657"
            obtain with
            var key = System.Text.Encoding.ASCII.GetBytes("VerySecretPasswd");
            var IV = System.Text.Encoding.ASCII.GetBytes("VeryRandomIV1234");

            var edata = CryptoHelper.EncryptAES("My very very secret message", key, IV);

            string cypherHexStr = ByteArrayToString(edata);
            */


            const string strSecretMessage = "My very very secret message";
            try
            {
                var cipher = StringToByteArray(strCipher);

                var key = System.Text.Encoding.ASCII.GetBytes("VerySecretPasswd");

                var cipherData = cipher.Skip(16).ToArray();
                var IV = cipher.Take(16).ToArray();

                var decryptedMessage=CryptoHelper.DecryptAES(cipherData, key, IV);

                var nPos=strSecretMessage.IndexOf(decryptedMessage);

                if (nPos >=0 && nPos % 16 == 0 && (nPos + decryptedMessage.Length == strSecretMessage.Length))
                {
                    return "OK " + decryptedMessage + " " + strCipher;
                }
               
                //return "OK1 " + decryptedMessage;
            }
            catch (Exception e)
            {
                if (mode == "ErrorsInBody")
                    return "Forbidden";
                else
                    throw new WebFaultException(HttpStatusCode.Forbidden);
            }
            if (mode == "ErrorsInBody")
                return "Invalid Padding";
            else 
                throw new WebFaultException(HttpStatusCode.NotFound);
        
        }
    }
}
