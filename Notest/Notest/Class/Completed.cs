using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notest.Class
{
    class Completed
    {
        public int Id { get; set; }
        public string UserLogin { get; set; }
        public string TestTheme { get; set; }
        public string TestName { get; set; }
        public int Result { get; set; }

        public Completed()
        {
            Id = 0;
            UserLogin = "логин";
            TestTheme = "тема";
            TestName = "название";
            Result = 0;
        }
    }
}
