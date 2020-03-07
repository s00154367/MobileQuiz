using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MobileQuiz.Pages
{
    public class CreateProfileModel : PageModel
    {
        public string GameCode { get; set; }
        public string DBload { get; set; }
        public string connectionstring = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = LocalDB; Integrated Security = True; Pooling=False";
        public string PubIP { get; set; }
        public string PriIP { get; set; }
        public void OnGet()
        {

        }
        public void OnPost()
        {
            CheckCode();
        }

        public void CheckCode()
        {
            GameCode = Request.Form["code"].ToString();
            int sqlresult;
            string sql = "Select * from Games where GameCode =" + GameCode;
            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {

                    conn.Open();
                    //sqlresult = (int)command.ExecuteScalar();
                    SqlDataReader read = command.ExecuteReader();
                    if (read.HasRows)
                    {
                        DBload = "Code OK!";
                    }
                    else
                    {
                        DBload = "BAD CODE!!";
                    }
                    conn.Close();
                }
            }

            


        }

        public void SQLInsertUser()
        {
            //SQL INSERT 
            string sql = "insert into Users ([Name],[Quote],[Picture],[GameCode],[Ready]) values(@name,@quote,@picture,@gamecode,@ready)";
            using (SqlConnection cnn = new SqlConnection(connectionstring))
            {
                try
                {
                    
                    cnn.Open();


                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        // Create and set the parameters values 
                        cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = Request.Form["Name"].ToString(); ;
                        cmd.Parameters.Add("@quote", SqlDbType.NVarChar).Value = Request.Form["Quote"].ToString();
                        cmd.Parameters.Add("@picture", SqlDbType.NVarChar).Value = "Stewie.png";
                        cmd.Parameters.Add("@gamecode", SqlDbType.NVarChar).Value = GameCode;
                        cmd.Parameters.Add("@ready", SqlDbType.NVarChar).Value = "Not Ready";

                        // Let's ask the db to execute the query
                        int rowsAdded = cmd.ExecuteNonQuery();
                        if (rowsAdded > 0)
                            DBload = "Loaded!";
                        else
                            DBload = "NOT LOADED!!";
                    }

                    cnn.Close();
                }
                catch (Exception ex)
                {
                    DBload = "NOTLOADED: " + ex.Message;
                }
            }
        }
    }
}