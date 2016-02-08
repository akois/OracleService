using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Encrypt
{

    public static class CryptoHelper
    {
        /// <summary>
        /// Encryptes a string using 3DES
        /// </summary>
        /// <param name="data">The data to encrypt</param>
        /// <param name="key">The key to use</param>
        /// <param name="IV">Initialization Vector</param>
        /// <returns>An encrypted, base64 encoded string</returns>
        public static byte[] EncryptAES(string data, byte[] key, byte[] IV)
        {
            string retVal = string.Empty;

            var tsp = new System.Security.Cryptography.AesCryptoServiceProvider();

            // default padding and mode
            tsp.Mode = CipherMode.CBC;
            tsp.Padding = PaddingMode.PKCS7;

            // get a crypt transform interface
            ICryptoTransform ct = tsp.CreateEncryptor(key, IV);

            // setup a memory stream
            MemoryStream ms = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
            {
                // write the string through the crypto stream
                byte[] db = System.Text.Encoding.ASCII.GetBytes(data);
                cs.Write(db, 0, db.Length);

                // flush
                cs.FlushFinalBlock();

                // convert the data in the memory stream to base64 text
                ms.Seek(0, SeekOrigin.Begin);

                var edata = ms.ToArray();
                return edata;
            }
        }

        public static string DecryptAES(byte[] data, byte[] key, byte[] IV)
        {
            string retVal = string.Empty;

            var tsp = new System.Security.Cryptography.AesCryptoServiceProvider();

            // default padding and mode
            tsp.Mode = CipherMode.CBC;
            tsp.Padding = PaddingMode.PKCS7;

            // get a crypt transform interface
            ICryptoTransform ct = tsp.CreateDecryptor(key, IV);

            // setup a memory stream
            MemoryStream ms = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
            {
                // write the string through the crypto stream
                cs.Write(data, 0, data.Length);

                // flush
                cs.FlushFinalBlock();

                // convert the data in the memory stream to base64 text
                ms.Seek(0, SeekOrigin.Begin);

                var enc = ms.ToArray();
                retVal = System.Text.Encoding.ASCII.GetString(enc, 0, enc.Length);
            }

            return retVal;
        }
    }

    /*class Program
    {


        static void Main(string[] args)
        {

            var edata = CryptoHelper.EncryptAES("kakapukakakapukakrjaka", new byte[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                                                new byte[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });

            //edata[12] = 14;
            CryptoHelper.DecryptAES(edata, new byte[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                                                new byte[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });

            CryptoHelper.DecryptAES(edata.Skip(16).ToArray(), new byte[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                                                edata.Take(16).ToArray());

        }
    }*/
}
