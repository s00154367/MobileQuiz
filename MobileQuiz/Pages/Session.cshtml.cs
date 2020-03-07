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
    public class SessionModel : PageModel
    {
        public string Message { get; set; }
        public string GameCode { get; set; }
        public string DBload { get; set; }
        public string connectionstring = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = LocalDB; Integrated Security = True; Pooling=False";
        public string PubIP { get; set; }
        public string PriIP { get; set; }
        public List<Users> UserList = new List<Users>();

        public void OnGet()
        {
            GetIP();
            CheckSession();
            GetUsers();
        }

        public void GetUsers()
        {
            string name = "";
            string quote = "";
            string picture = "";
            string ready = "";
            string sql = "Select [Name], [Quote] , [Picture] , [GameCode] , [Ready] from Users where [GameCode] =" + GameCode;
            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {

                    conn.Open();
                    //sqlresult = command.ExecuteNonQuery();
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            name = reader["Name"].ToString();
                            quote = reader["Quote"].ToString();
                            picture = reader["Picture"].ToString();                
                            ready = reader["Ready"].ToString();

                            Users newuser = new Users(name, quote, picture,GameCode,ready);
                            UserList.Add(newuser);
                        }
                    }

                    
                    conn.Close();


                }
            }
        }

        public void CheckSession()
        {
            int sqlresult;
            string gCode = "";
            string sql = "Select [Public IP Address], [GameCode] from Games where [Public IP Address] =" + "'" +PubIP+"'";
            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {

                    conn.Open();
                    //sqlresult = command.ExecuteNonQuery();
                    string ip = "";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ip = reader["Public IP Address"].ToString();
                            gCode = reader["GameCode"].ToString();
                        }
                    }

                    if (ip == PubIP)
                    {
                        GameCode = gCode;
                    }
                    else
                    {
                        conn.Close();
                        CreateCode();
                    }
                    conn.Close();
                }
            }
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
                SQLInsertSession(GameCode);
            }
            else
            {
                CreateCode();
            }


        }

        public void SQLInsertSession(string gCode)
        {
            
            //SQL INSERT 
            string sql = "insert into Games ([GameCode],[Private IP Address],[Public IP Address]) values(@gcode,@priIP,@pubIP)";
            using (SqlConnection cnn = new SqlConnection(connectionstring))
            {
                try
                {
                    
                    cnn.Open();


                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        // Create and set the parameters values 
                        cmd.Parameters.Add("@gcode", SqlDbType.NVarChar).Value = GameCode;
                        cmd.Parameters.Add("@priIP", SqlDbType.NVarChar).Value = PriIP;
                        cmd.Parameters.Add("@pubIP", SqlDbType.NVarChar).Value = PubIP;

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

        public void GetIP()
        {
            //GET BOTH IP ADDRESS' FROM HOST PC
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    PriIP = ip.ToString();
                }
            }
            PubIP = new WebClient().DownloadString("http://icanhazip.com").TrimEnd('\n');

        }
    }
}
