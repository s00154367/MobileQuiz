using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileQuiz
{
    public class Users
    {
        public string Name { get; set; }
        public string Quote { get; set; }
        public string Picture { get; set; }
        public string GameCode { get; set; }
        public string Ready { get; set; }

        public Users(string name, string quote, string picture, string gamecode, string ready)
        {
            Name = name;
            Quote = quote;
            Picture = picture;
            GameCode = GameCode;
            Ready = ready;
        }
    }
}
