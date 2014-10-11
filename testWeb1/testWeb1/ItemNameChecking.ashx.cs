using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using testWeb1.Admin.DAL;

namespace testWeb1
{
    /// <summary>
    /// Summary description for ItemNameChecking
    /// </summary>
    public class ItemNameChecking : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string itemName1 = context.Request["ItemName1"];
            string itemName2 = context.Request["ItemName2"];
            if(!string.IsNullOrEmpty(itemName1))
            {
                bool result1 = new ItemDAL().CheckingExisting(itemName1);
                
            }
            else if (!string.IsNullOrEmpty(itemName2))
            {
                bool result2 = new ItemDAL().CheckingExisting(itemName2);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}