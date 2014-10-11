using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NVelocity.App;
using NVelocity.Runtime;
using NVelocity;
using System.Data;
using System.Data.SqlClient;

namespace testWeb1
{
    public class CommonHelper
    {
        public static string RenderHtml(string name, object data)
        {
             VelocityEngine vltEngine = new VelocityEngine();
            vltEngine.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            vltEngine.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, 
                System.Web.Hosting.HostingEnvironment.MapPath("~/templates"));
            vltEngine.Init();

            VelocityContext vltContext = new VelocityContext();
            vltContext.Put("Model", data);

            Template vltTemplate = vltEngine.GetTemplate(name);
            System.IO.StringWriter vltWriter = new System.IO.StringWriter();
            vltTemplate.Merge(vltContext, vltWriter);
            return vltWriter.GetStringBuilder().ToString();
        }

        public static bool HasFile(HttpPostedFile file)
        {
            if (file == null)
            {
                return false;
            }
            else
            {
                return file.ContentLength > 0;
            }
        }

        public static DataRowCollection GetProductCategories()
        {
            return SqlHelper.ExecuteDataTable("select * from T_ProductCategories").Rows;
        }

        public static object GetSettings()
        {
            string siteName = CommonHelper.ReadSetting("SiteName");
            string siteURL = CommonHelper.ReadSetting("SiteURL");
            string address = CommonHelper.ReadSetting("Address");
            string postCode = CommonHelper.ReadSetting("PostCode");
            string contactPerson = CommonHelper.ReadSetting("ContactPerson");
            string telPhone = CommonHelper.ReadSetting("TelPhone");
            string fax = CommonHelper.ReadSetting("Fax");
            string mobile = CommonHelper.ReadSetting("Mobile");
            string email = CommonHelper.ReadSetting("Email");

            var data = new { SiteName = siteName, SiteURL = siteURL, Address = address, PostCode = postCode, ContactPerson = contactPerson, TelPhone = telPhone, Fax = fax, Mobile = mobile, Email = email };
            return data;
        }

        public static string ReadSetting(string name)
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select value from T_Settings where Name=@Name",new SqlParameter("@Name",name));
            if (dt.Rows.Count <= 0)
            {
                throw new Exception("Cannot find Name="+name+"'s configuration");
            }
            else if (dt.Rows.Count > 1)
            {
                throw new Exception("Found many Name=" + name + "s' configuration");
            }
            return (string)dt.Rows[0]["Value"];
        }

        public static void WriteSetting(string name, string value)
        {
            SqlHelper.ExecuteNonQuery("Update T_Settings set Value=@Value where Name=@Name", new SqlParameter("@Value", value), new SqlParameter("@Name", name));
        }
    }
}