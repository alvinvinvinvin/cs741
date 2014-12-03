using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace testWeb1
{
    /// <summary>
    /// Summary description for MainPage
    /// </summary>
    public class MainPage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            
            context.Response.ContentType = "text/html";
            string search = context.Request["searchBt"];
            string keyword = context.Request["searchBox"];
            string username = context.Request["username"];
            string action = context.Request["Action"];
            if (action == "AddNew")
            {
                string addNewhtml = CommonHelper.RenderHtml("Front/ItemEdit.html", null);//Generate a null itemEdit page for adding new item.
                context.Response.Write(addNewhtml);//Generate corresponding page.
            }
            else
            {
                if (string.IsNullOrEmpty(search))
                {
                    DataTable dtItems = SqlHelper.ExecuteDataTable(
                               "select * from T_Item");
                    var data = new { Username = username, Items = dtItems.Rows };
                    string MainPageHtml = CommonHelper.RenderHtml("Front/MainPage.html", data);
                    context.Response.Write(MainPageHtml);
                }
                else
                {
                    DataTable dtItems = SqlHelper.ExecuteDataTable(
                               "select * from T_Item where ItemName like @ItemName",
                               new SqlParameter("@ItemName", "%" + keyword + "%"));
                    if (dtItems.Rows.Count > 0)
                    {
                        var data = new { Username = username, Items = dtItems.Rows };
                        string MainPageHtml = CommonHelper.RenderHtml("Front/MainPage.html", data);
                        context.Response.Write(MainPageHtml);
                    }
                    else
                    {
                        var data = new { Username = username, Items = "" };
                        string MainPageHtml = CommonHelper.RenderHtml("Front/MainPage.html", data);
                        context.Response.Write(MainPageHtml);
                    }
                } 
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