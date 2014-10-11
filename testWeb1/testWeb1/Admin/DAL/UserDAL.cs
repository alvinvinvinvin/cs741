using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using testWeb1.Admin.Model;

namespace testWeb1.Admin.DAL
{
    public class UserDAL
    {
        public User ToModel(DataRow dr)
        {
            User user = new User();
            user.Id = (long)dr["Id"];
            user.UserName = (string)dr["UserName"];
            user.Password = (string)dr["Password"];
            return user;
        }

    }
}