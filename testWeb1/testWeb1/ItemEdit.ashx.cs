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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            ItemDAL itemDAL = new ItemDAL();
            context.Response.ContentType = "text/html";
            string itemId = context.Request["ItemId"];
            long itemid = Convert.ToInt64(itemId);//Get item ID from front page.
            string save = context.Request["btnSave"];
            string edit = context.Request["btnEdit"];
            string delete = context.Request["btnDelete"];
            string IsPostBack = context.Request["IsPostBack"];
            //add new
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
                        "<meta http-equiv= \"refresh\" content= \"2;url=MainPage.ashx?username=admin\"><h2>Successfully Added!</h2>");
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
            //If user clicked "save" button under editing situation.
            if (!string.IsNullOrEmpty(edit))
            {
                string itemName = context.Request["ItemName1"];
                string itemDes = context.Request["Msg1"];
                HttpPostedFile itemImage = context.Request.Files["ItemImage1"];
                if (!string.IsNullOrEmpty(itemName))
                {
                    if (itemDAL.CheckingExisting(itemName))
                    {
                        string prevPage = context.Request.UrlReferrer.ToString();
                        context.Response.Write(
                            "<meta http-equiv= \"refresh\" content= \"2;url="
                            + prevPage + "\"><h2>Terminology name exits, please try others or click cancel.</h2>");
                    }
                    else 
                    {  
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
                            string path = context.Request["ItemImage4"];//Get image relative path.
                            string oldImagePath = context.Server.MapPath(path);//Get image absolute path.
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);//Delete image file from server.
                            }
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
                                 "<meta http-equiv= \"refresh\" content= \"2;url=MainPage.ashx?username=admin\"><h2>Successfully Edited!</h2>");
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

            if (!string.IsNullOrEmpty(delete))
            {
                itemDAL.DeleteById(itemid);//Delete item from database.
                string path = context.Request["ItemImage3"];//Get image relative path.
                string imagePath = context.Server.MapPath(path);//Get image absolute path.
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);//Delete image file from server.
                }
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