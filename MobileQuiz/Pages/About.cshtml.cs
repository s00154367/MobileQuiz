using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MobileQuiz.Pages
{
    public class AboutModel : PageModel
    {
        public string Message { get; set; }
        public string GameCode { get; set; }

        public void OnGet()
        {            
            Random r = new Random();
            var x = r.Next(0, 1000000);
            GameCode = x.ToString("000000");
            
        }
    }
}
