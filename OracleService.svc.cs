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
        [WebGet(UriTemplate = "Decrypt?cipher={cipher}")]
        [OperationContract]
        public string Decrypt(string cipher)
        {
            try
            {
                var key = System.Text.Encoding.ASCII.GetBytes("VerySecretPasswd");

                var cipherData = System.Text.Encoding.ASCII.GetBytes(cipher).Skip(16).ToArray();
                var IV = System.Text.Encoding.ASCII.GetBytes(cipher).Take(16).ToArray();

                //throw new WebFaultException(HttpStatusCode.BadRequest);

                var decryptedMessage=CryptoHelper.DecryptAES(cipherData, key, IV);

                if (decryptedMessage != "My very very secret message")
                    throw new WebFaultException(HttpStatusCode.Forbidden);

            }
            catch (Exception)
            {
                throw new WebFaultException(HttpStatusCode.NotFound);
            }

            return "OK";
           
        }
    }
}
