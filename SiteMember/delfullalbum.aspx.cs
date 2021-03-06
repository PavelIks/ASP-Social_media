﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.IO.MemoryMappedFiles;

public partial class SiteMember_delfullalbum : System.Web.UI.Page
{
    dataservices ds = new dataservices();
    SqlConnection cn = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\data.mdf;Integrated Security=True;User Instance=True");
    SqlCommand cmd = new SqlCommand();
    SqlDataReader reader;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataList1.DataBind();
            cn.Open();
            cmd.Connection = cn;
            cmd.CommandText = "select first_name, last_name  from login_detail where username=@username";
            cmd.Parameters.AddWithValue("@username", Session["username"].ToString());
            Session["firstName"] = "";
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                Session["firstName"] = reader["first_name"].ToString();
            }
            Label1.Text = Session["firstName"].ToString();
            Label2.Text = reader["last_name"].ToString();
            reader.Close();
            cmd.Parameters.Clear();
            cmd.CommandText = "select photo from profile_picture where username=@username";
            cmd.Parameters.AddWithValue("@username", Session["username"].ToString());

            try
            {
                string filepath = cmd.ExecuteScalar().ToString();
                ImageButton1.ImageUrl = filepath;
            }
            catch (Exception ex)
            {

                ImageButton1.ImageUrl = "~/profilepic/avatar.jpg";

            }
            cn.Close();
            cmd.Parameters.Clear();
        }

    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void button_command(object sender, CommandEventArgs e)
    {
        Session["status"] = "No change";
        Session["aid"] = e.CommandName;
        Response.Redirect("Delphoto.aspx?albumid=" + e.CommandName + "");
    }

    protected void button1_command(object sender, CommandEventArgs e)
    {

        DataTable dt1 = ds.GetData("select photo from photo where albumid='" + Convert.ToInt32(e.CommandName) + "' ");
         
        foreach (DataRow dr in dt1.Rows  )
        {
        string photopath = dr["photo"].ToString();
        File.Delete(Server.MapPath(photopath));  
        }
      
        DataTable dt = ds.GetData("select albumphoto from albumphoto where albumid='" + Convert.ToInt32(e.CommandName) + "' ");
        string albumpath = dt.Rows[0][0].ToString();
        File.Delete(Server.MapPath(albumpath));
        ds.UpdateData("delete from albumphoto where albumid='" + e.CommandName + "'");
        DataList1.DataBind();
    }

    protected void sqldatasource_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {

    }
}
   