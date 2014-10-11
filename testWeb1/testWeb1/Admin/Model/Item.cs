using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testWeb1.Admin.Model
{
    public class Item
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDes { get; set; }
        public string ItemImage { get; set; }
        public string ItemCateg { get; set; }
    }
}