using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testWeb1
{
    /// <summary>
    /// Summary description for UserEdit
    /// </summary>
    public class UserEdit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("useredit");
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