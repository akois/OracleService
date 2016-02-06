using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace OracleService
{
    [ServiceContract]
    public class OracleService
    {
        [WebGet(UriTemplate = "Decrypt?cipher={cipher}")]
        [OperationContract]
        public string Decrypt(string cipher)
        {
            //throw new WebFaultException(HttpStatusCode.BadRequest);

            return "cipher";
           
        }
    }
}
