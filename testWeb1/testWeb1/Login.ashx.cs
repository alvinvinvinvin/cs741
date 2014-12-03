using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace testWeb1
{
    /// <summary>
    /// Summary description for Login
    /// </summary>
    public class Login : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            bool isLogin = !string.IsNullOrEmpty(context.Request["Login"]);
            if (isLogin)
            {
                string username = context.Request["UserName"];
                string password = context.Request["Password"];
                //Encrypt password for querying from database.
                string encryptedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(password,"MD5");
                int count = (int)SqlHelper.ExecuteScalar(
                    "select count(*) from T_User where UserName=@UserName and Password=@Password", 
                    new SqlParameter("@UserName", username)
                    , new SqlParameter("@Password", encryptedPwd));
                if (count == 1)
                {
                    //Login successful;
                    DataTable dtItems = SqlHelper.ExecuteDataTable(
                        "select * from T_Item");
                    var user = new {Username = username, Items = dtItems.Rows };
                    string mainPage = CommonHelper.RenderHtml("Front/MainPage.html", user);
                    context.Response.Write(mainPage);

                }
                else if (count > 1)
                {
                    context.Response.Write("Duplicated user");
                }
                else
                {
                    string prevPage = context.Request.UrlReferrer.ToString();
                    context.Response.Write("<meta http-equiv= \"refresh\" content= \"2;url="+prevPage+"\">Wrong username or password");
                }
            }
            else
            {
                var html = CommonHelper.RenderHtml("Front/Login.html", null);
                context.Response.Write(html);
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