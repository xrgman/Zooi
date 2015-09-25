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
        public bool admin;
        public string UserPassword;

        public UserClient(string fullNam, bool admin, string pass)
        {
            fullName = fullNam;
            admini = admin;
            userPass = pass;
        }
        public string fullName
        {
            get { return UserName; }
            set { UserName = value; }
        }
        public bool admini
        {
            get { return admin; }
            set { admin = value;  }
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
