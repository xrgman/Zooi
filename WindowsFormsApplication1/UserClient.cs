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
        public bool Admin;
        public string UserPassword;

        public UserClient(string fullNam, bool admin, string pass)
        {
            fullName = fullNam;
            this.Admin = admin;
            userPass = pass;
        }
        public string fullName
        {
            get { return UserName; }
            set { UserName = value; }
        }
        public bool accLevel
        {
            get { return Admin; }
            set { Admin = value;  }
        }
        public string userPass
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
