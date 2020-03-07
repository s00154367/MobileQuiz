using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MobileQuiz.Pages
{
    public class AboutModel : PageModel
    {
        public string Message { get; set; }
        public string GameCode { get; set; }
        public string DBload { get; set; }
        public string connectionstring = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = LocalDB; Integrated Security = True; Pooling=False";


        public void OnGet()
        {
            //SQLInsertGCode("012345");
            CreateCode();
        }

        public void CreateCode()
        {
            Random r = new Random();
            var x = r.Next(0, 1000000);
            GameCode = x.ToString("000000");
            CheckCode(GameCode);
        }

        public void CheckCode(string gCode)
        {
            int sqlresult;
            string sql = "Select GameCode from Games where GameCode =" + gCode;
            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {

                    conn.Open();
                    sqlresult = command.ExecuteNonQuery();
                    conn.Close();
                }
            }

            if (sqlresult < 0)
            {
                SQLInsertGCode(GameCode);
            }
            else
            {
                CreateCode();
            }


        }

        public void SQLInsertGCode(string gCode)
        {
            //GET BOTH IP ADDRESS' FROM HOST PC
            String IPaddress = "";
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPaddress = ip.ToString();
                }
            }
            string pubIP = new WebClient().DownloadString("http://icanhazip.com");

            //SQL INSERT 
            string sql = "insert into Games ([GameCode],[Private IP Address],[Public IP Address]) values(@gcode,@priIP,@pubIP)";
            using (SqlConnection cnn = new SqlConnection(connectionstring))
            {
                try
                {
                    cnn.Close();
                    cnn.Open();


                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        // Create and set the parameters values 
                        cmd.Parameters.Add("@gcode", SqlDbType.NVarChar).Value = GameCode;
                        cmd.Parameters.Add("@priIP", SqlDbType.NVarChar).Value = IPaddress;
                        cmd.Parameters.Add("@pubIP", SqlDbType.NVarChar).Value = pubIP;

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
