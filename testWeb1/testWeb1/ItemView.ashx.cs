using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using testWeb1.Admin.DAL;
using testWeb1.Admin.Model;

namespace testWeb1
{
    /// <summary>
    /// Summary description for ItemView
    /// </summary>
    public class ItemView : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ItemDAL itemDAL = new ItemDAL();
                context.Response.ContentType = "text/html";
                string username = context.Request["username"];
                string ItemId = context.Request["ItemId"];
                string action = context.Request["Action"];//Get action from front page.
                if (string.IsNullOrEmpty(action))
                {
                    DataTable dtItem = SqlHelper.ExecuteDataTable(
                                "select * from T_Item where ItemId=@ItemId",
                                new SqlParameter("@ItemId", ItemId));
                    var data = new { Username = username, item = dtItem.Rows[0] };
                    string ItemViewHtml = CommonHelper.RenderHtml("Front/ItemView.html", data);
                    context.Response.Write(ItemViewHtml); 
                }
                long itemid = Convert.ToInt64(ItemId);//Convert it to long for querying from database.
                
                
                if (action == "Edit")
                {

                    Item item = itemDAL.getById(itemid);
                    var itemData =
                        new
                        {
                            ItemId = ItemId,
                            ItemName = item.ItemName,
                            ItemImage = item.ItemImage,
                            ItemDes = item.ItemDes,
                            Action = "Edit"
                        };
                    string editHtml = CommonHelper.RenderHtml("Front/ItemEdit.html", itemData);
                    context.Response.Write(editHtml);
                }
                else if (action == "Delete")
                {
                    Item item = itemDAL.getById(itemid);
                    var itemData = new
                    {
                        Action = "Delete",
                        ItemId = itemid,
                        ItemImage = item.ItemImage
                    };
                    string deleteHtml = CommonHelper.RenderHtml("Front/ItemEdit.html", itemData);
                    context.Response.Write(deleteHtml);

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