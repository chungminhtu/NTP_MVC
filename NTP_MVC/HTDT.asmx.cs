using NTP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [WebMethod]
        public string receive(int MOID, string telco, string serviceNum, string phone, string syntax, string message)
        {
            List<DM_PhacDoDT> therapies = db.DM_PhacDoDT.ToList();
            System.Diagnostics.Debug.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> " + MOID + " " + telco + " " + therapies.Count);
            return "";
        }
    }
}
