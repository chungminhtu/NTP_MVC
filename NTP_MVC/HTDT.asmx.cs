using NTP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;

namespace NTP_MVC
{
    /// <summary>
    /// Summary description for HTDT
    /// </summary>
    [WebService(Namespace = "http://vitimes.org.vn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class HTDT : System.Web.Services.WebService
    {
        NTP_DBEntities db = new NTP_DBEntities();

        private string Base64Decode(string Text)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                byte[] Bytes = new byte[Text.Length];

                Bytes = Convert.FromBase64String(Text);

                return Encoding.Default.GetString(Bytes);
            }

            return string.Empty;
        }

        private string SHA1Encrypt(string OriginalData)
        {
            SHA1CryptoServiceProvider Provider = new SHA1CryptoServiceProvider();

            byte[] HashedBytes = Provider.ComputeHash(Encoding.UTF8.GetBytes(OriginalData));

            Provider.Clear();

            return Convert.ToBase64String(HashedBytes);
        }

        [WebMethod]
        public string Receive(int MOId, string Telco, string ServiceNum, string Phone, string Syntax, string message)
        {
            int MessageCode = 0; /*
                                      * Ma tin nhan: 0    - ban tin text.
                                      *              1000 - ban tin wappush.
                                      */

            string Response = string.Empty;

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Server.MapPath("~") + "/Content/HTDT_TemplateFiles/logpath.txt"))
            {
                file.WriteLine(Phone);
            }
            Response = MessageCode + "|" + "Cam on ban da nhan tin uong thuoc.";

            return Response;
        }

        [WebMethod]
        public string Receive(int MOId, string Telco, string ServiceNum, string Phone, string Syntax, string EncryptedMessage, string Signature)
        {
            int MessageCode = 0; /*
                                  * Ma tin nhan: 0    - ban tin text.
                                  *              1000 - ban tin wappush.
                                  */

            string Response = string.Empty;
            string PrivateKey = "1067825375";
            List<DM_PhacDoDT> therapies = db.DM_PhacDoDT.ToList();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Server.MapPath("~") + "/Content/HTDT_TemplateFiles/logpath.txt"))
            {
                file.WriteLine(Phone + " " + therapies.Count);
            }
            string MessageRequest = Base64Decode(EncryptedMessage).ToLower();
            string mSignature = SHA1Encrypt(MOId + ServiceNum + Phone + MessageRequest + PrivateKey);

            if (!Signature.Equals(mSignature))
            {
                MessageCode = 1;
                Response = MessageCode + "|" + "Chu ky khong hop le.";
            }
            else
            {
                try
                {
                    switch (Syntax)
                    {
                        case "UT":
                            Response = MessageCode + "|" + "Cam on ban da nhan tin uong thuoc.";
                            break;
                        case "DHBT":
                            Response = MessageCode + "|" + "Cam on ban da gui tin nhan dau hieu bat thuong.";
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageCode = 1;
                    Response = MessageCode + "|" + "He thong dang co loi, vui long thu lai sau.";
                }
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Server.MapPath("~") + "/Content/HTDT_TemplateFiles/logpath.txt", true))
            {
                file.WriteLine(Response);
            }
            return Response;
        }
    }
}
