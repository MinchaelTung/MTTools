using System.Collections.Generic;

namespace WebApiDemoServer.Models
{
    public class UserInfo
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Url { get; set; }

        public string Book { get; set; }

        public static List<UserInfo> list = new List<UserInfo>();
        public static void GetDate()
        {
            if (list.Count == 0)
            {
                list.AddRange(new List<UserInfo>(){
                    new UserInfo(){ ID=1, Name="User 1" , Age=28, Book="Book 1", Url="http://Book/1"},
                    new UserInfo(){ ID=2, Name="User 2" , Age=28, Book="Book 2", Url="http://Book/2"},
                    new UserInfo(){ ID=3, Name="User 3" , Age=28, Book="Book 3", Url="http://Book/3"},
                    new UserInfo(){ ID=4, Name="User 4" , Age=28, Book="Book 4", Url="http://Book/4"},
                    new UserInfo(){ ID=5, Name="User 5" , Age=28, Book="Book 5", Url="http://Book/5"},
                    new UserInfo(){ ID=6, Name="User 6" , Age=28, Book="Book 6", Url="http://Book/6"}
                });
            }


        }
    }
}