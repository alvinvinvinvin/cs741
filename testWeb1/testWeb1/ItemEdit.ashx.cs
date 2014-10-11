using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using testWeb1.Admin.DAL;
using testWeb1.Admin.Model;

namespace testWeb1
{
    /// <summary>
    /// Summary description for ItemEdit
    /// </summary>
    public class ItemEdit : IHttpHandler
    {
        string Category = "";
        public void ProcessRequest(HttpContext context)
        {
            ItemDAL itemDAL = new ItemDAL();
            context.Response.ContentType = "text/html";
            string itemId = context.Request["ItemId"];
            long itemid = Convert.ToInt64(itemId);
            string action = context.Request["Action"];
            if (action == "AddNew")
            {
                string addNewhtml = CommonHelper.RenderHtml("Front/ItemEdit.html", null);
                context.Response.Write(addNewhtml);
            }
            else if (action == "Edit")
            {
                
                Item item = itemDAL.getById(itemid);
                var data = 
                    new 
                    { 
                        ItemId = itemId, 
                        ItemName = item.ItemName, 
                        ItemImage = item.ItemImage,
                        ItemDes = item.ItemDes,
                        Action = "Edit"
                    };
                string editHtml = CommonHelper.RenderHtml("Front/ItemEdit.html",data);
                context.Response.Write(editHtml);
            }
            else if (action == "Delete")
            {
                var data = new 
                {
                    Action = "Delete",
                    ItemId = itemid
                };
                string deleteHtml = CommonHelper.RenderHtml("Front/ItemEdit.html",data);
                context.Response.Write(deleteHtml);

            }
            string save = context.Request["save2"];
            string edit = context.Request["save1"];
            string delete = context.Request["Delete"];
            string IsPostBack = context.Request["IsPostBack"];
            if (!string.IsNullOrEmpty(save))
            {
                string itemName = context.Request["ItemName2"];


                if (!string.IsNullOrEmpty(itemName))
                {
                    if (!itemDAL.CheckingExisting(itemName))
                    {
                        string itemDes = context.Request["Msg2"];
                        HttpPostedFile itemImage = context.Request.Files["ItemImage2"];
                        string imageName =
                            itemName + DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(itemImage.FileName);
                        string imagePath = "\\uploadfile\\image\\" + imageName;
                        itemImage.SaveAs(context.Server.MapPath(imagePath));
                        string itemCategory = Category;
                        Item item =
                            new Item
                            {
                                ItemName = itemName,
                                ItemDes = itemDes,
                                ItemImage = imagePath,
                                ItemCateg = itemCategory
                            };
                        itemDAL.AddNew(item);
                        context.Response.Write(
                        "<meta http-equiv= \"refresh\" content= \"2;url=MainPage.ashx?username=admin\"><h2>Successfully Added!<h2>");
                    }
                    else
                    {
                        string prevPage = context.Request.UrlReferrer.ToString();
                        context.Response.Write(
                            "<meta http-equiv= \"refresh\" content= \"2;url="
                            + prevPage + "\"><h2>Terminology name exits, please try others.</h2>");
                        //context.Response.Write("<script>window.alert('Terminology name exits, try others.')</script>");
                    }
                }
                else
                {
                    string prevPage = context.Request.UrlReferrer.ToString();
                    context.Response.Write(
                        "<meta http-equiv= \"refresh\" content= \"2;url="
                        + prevPage + "\"><h2>Name cannot be null</h2>");
                }
            }

            if (!string.IsNullOrEmpty(edit))
            {
                string itemName = context.Request["ItemName1"];
                string itemDes = context.Request["Msg1"];
                HttpPostedFile itemImage = context.Request.Files["ItemImage1"];
                string itemCategory = Category;
                if (CommonHelper.HasFile(itemImage))
                {
                    string imageName =
                       itemName + DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(itemImage.FileName);
                    string imagePath = "\\uploadfile\\image\\" + imageName;
                    itemImage.SaveAs(context.Server.MapPath(imagePath));

                    
                    Item item =
                        new Item
                        {
                            ItemId = itemid,
                            ItemName = itemName,
                            ItemDes = itemDes,
                            ItemImage = imagePath,
                            ItemCateg = itemCategory
                        };
                    itemDAL.Update(item);
                }
                else
                {
                    Item item =
                        new Item
                        {
                            ItemId = itemid,
                            ItemName = itemName,
                            ItemDes = itemDes,
                            ItemCateg = itemCategory
                        };
                    itemDAL.UpdateWithoutImage(item);
                }
                context.Response.Write(
                "<meta http-equiv= \"refresh\" content= \"2;url=MainPage.ashx?username=admin\"><h3>Successfully Edited</h3>");
            }

            if (!string.IsNullOrEmpty(delete))
            {
                itemDAL.DeleteById(itemid);
                context.Response.Write(
                    "<meta http-equiv= \"refresh\" content= \"2;url=MainPage.ashx?username=admin\"><h3>Successfully Deleted</h3>");
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