using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notest.Class
{
    class Completed
    {
        public string UserLogin { get; set; }
        public string TestTheme { get; set; }
        public string TestName { get; set; }
        public int Result { get; set; }
        public string Date { get; set; }

        public Completed()
        {
            UserLogin = "логин";
            TestTheme = "тема";
            TestName = "название";
            Result = 0;
            Date = DateTime.Now.ToString();
        }
    }
}
