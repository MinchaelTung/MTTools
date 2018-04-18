using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemoClient
{
    public class UserInfoCCCC
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Url { get; set; }

        public string Book { get; set; }


        public override string ToString()
        {
            return string.Format("ID:{0},Name:{1},Age:{2},Book:{4},Url:{3}", ID, Name, Age, Url, Book);
        }

    }
}
