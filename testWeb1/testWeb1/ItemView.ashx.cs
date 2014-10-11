using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace testWeb1
{
    /// <summary>
    /// Summary description for ItemView
    /// </summary>
    public class ItemView : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
                context.Response.ContentType = "text/html";
                string username = context.Request["username"];
                string ItemId = context.Request["Id"];
                DataTable dtItem = SqlHelper.ExecuteDataTable(
                    "select * from T_Item where ItemId=@ItemId",
                    new SqlParameter("@ItemId", ItemId));
                var data = new { Username = username, item = dtItem.Rows[0] };
                string ItemViewHtml = CommonHelper.RenderHtml("Front/ItemView.html", data);
                context.Response.Write(ItemViewHtml);
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