using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MobileQuiz.Pages
{
    public class ContactModel : PageModel
    {
        public string connectionstring = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = LocalDB; Integrated Security = True; Pooling=False";
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Your contact page.";            
        }
        public void CheckSession()
        {
            int sqlresult;
            //string gcode = gameCode.Text;
            string sql = "Select GameCode from Games where GameCode =" + "";
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
                
            }
            else
            {
               
            }
        }
    }
}
