using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CheckYourPremiumMVC.Models
{
    public class BindCityData
    {
        SqlConnection constr = new SqlConnection(ConfigurationManager.ConnectionStrings["MyString"].ConnectionString);
        public DataSet Get_City()
        {
            SqlCommand com = new SqlCommand(" select City_Name ,State_ID from tblCity order by City_Name", constr);
            com.CommandType = CommandType.Text;
           // com.Parameters.AddWithValue("@Action", "Select_State");
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(com);
            da.Fill(ds);
            return ds;
        }

        public DataSet Get_HealthInsurence()
        {
            SqlCommand com = new SqlCommand(" select * from tbl_HealthPremiumChartData", constr);
            com.CommandType = CommandType.Text;
            // com.Parameters.AddWithValue("@Action", "Select_State");
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(com);
            da.Fill(ds);
            return ds;
        }

    }
}