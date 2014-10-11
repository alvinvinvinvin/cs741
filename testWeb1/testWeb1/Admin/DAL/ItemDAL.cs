using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using testWeb1.Admin.Model;

namespace testWeb1.Admin.DAL
{
    public class ItemDAL
    {
        public Item ToModel(DataRow dr)
        {
            Item item = new Item();
            item.ItemId = (long)dr["ItemId"];
            item.ItemName = (string)dr["ItemName"];
            item.ItemDes = (string)SqlHelper.FromDBValue(dr["ItemDes"]);
            item.ItemImage = (string)SqlHelper.FromDBValue(dr["ItemImage"]);
            item.ItemCateg = (string)SqlHelper.FromDBValue(dr["ItemCateg"]);
            return item;
        }

        public void AddNew(Item item) 
        {
            
                SqlHelper.ExecuteNonQuery(
                           "insert into T_Item(ItemName, ItemDes, ItemImage, ItemCateg) values(@ItemName, @ItemDes, @ItemImage, @ItemCateg)",
                           new SqlParameter("@ItemName", item.ItemName),
                           new SqlParameter("@ItemDes", item.ItemDes),
                           new SqlParameter("@ItemImage", item.ItemImage),
                           new SqlParameter("@ItemCateg", item.ItemCateg));

        }

        public bool CheckingExisting(string itemName)
        {
            DataTable dt = SqlHelper.ExecuteDataTable(
                "select * from T_Item where ItemName=@ItemName",
                new SqlParameter("@ItemName", itemName));
            if (dt.Rows.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Item getById(long itemId)
        {
            DataTable dt = SqlHelper.ExecuteDataTable(
                "select * from T_Item where ItemId=@ItemId",
                new SqlParameter("@ItemId",itemId));

            return ToModel(dt.Rows[0]);
        }

        public void DeleteById(long itemId)
        {
            SqlHelper.ExecuteNonQuery(
                "delete from T_Item where ItemId=@ItemId",
                new SqlParameter("@ItemId",itemId));
        }

        public void Update(Item item)
        {
            SqlHelper.ExecuteNonQuery(
                "update T_Item set ItemName=@ItemName, ItemDes=@ItemDes, ItemImage=@ItemImage, ItemCateg=@ItemCateg where ItemId=@ItemId",
                new SqlParameter("@ItemName",item.ItemName),
                new SqlParameter("@ItemDes",SqlHelper.ToDBValue(item.ItemDes)),
                new SqlParameter("@ItemImage",SqlHelper.ToDBValue(item.ItemImage)),
                new SqlParameter("@ItemCateg",SqlHelper.ToDBValue(item.ItemCateg)),
                new SqlParameter("@ItemId",item.ItemId));
        }

        public void UpdateWithoutImage(Item item)
        {
            SqlHelper.ExecuteNonQuery(
                "update T_Item set ItemName=@ItemName, ItemDes=@ItemDes, ItemCateg=@ItemCateg where ItemId=@ItemId",
                new SqlParameter("@ItemName", item.ItemName),
                new SqlParameter("@ItemDes", SqlHelper.ToDBValue(item.ItemDes)),
                new SqlParameter("@ItemCateg", SqlHelper.ToDBValue(item.ItemCateg)),
                new SqlParameter("@ItemId", item.ItemId));
        }

    }
}