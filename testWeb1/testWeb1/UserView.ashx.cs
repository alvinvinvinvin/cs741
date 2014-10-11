using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace testWeb1
{
    /// <summary>
    /// Summary description for UserView
    /// </summary>
    public class UserView : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            context.Response.Write("Hello World");
            DataTable dtUsers = SqlHelper.ExecuteDataTable(
                           "select * from T_User");
            var data = new { Users = dtUsers.Rows };
            string UserViewHtml = CommonHelper.RenderHtml("Front/UserView.html", data);
            context.Response.Write(UserViewHtml);
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