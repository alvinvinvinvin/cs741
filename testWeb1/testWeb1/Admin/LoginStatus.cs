using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testWeb1.Admin
{
    public class LoginStatus
    {
        public static void CheckLogin()
        {
            HttpContext context = HttpContext.Current;
            if (context.Session["LoginUserName"] != "admin")
            {
                context.Response.Redirect("Login.ashx");
            }
        }
    }
}