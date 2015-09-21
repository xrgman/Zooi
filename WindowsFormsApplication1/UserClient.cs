using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class UserClient
    {
        public string UserName;
        public string UserAccessLevel;
        public int UserPassword;

        public UserClient(string fullNam, string aLevel, int pass)
        {
            fullName = fullNam;
            accLevel = aLevel;
            userPass = pass;
        }
        public string fullName
        {
            get { return UserName; }
            set { UserName = value; }
        }
        public string accLevel
        {
            get { return UserAccessLevel; }
            set { UserAccessLevel = value;  }
        }
        public int userPass
        {
            get { return UserPassword; }
            set { UserPassword = value; }
        }
        public override bool Equals(object obj)
        {
            UserClient temp = obj as UserClient;
            if(temp != null)
            {
                return (userPass == temp.userPass);
            }
            return false;
        }

    }
}
