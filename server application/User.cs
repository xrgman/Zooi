using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server_application
{
    [Serializable]
    class User
    {
        public string username { get; private set; }
        private string password;

        public User (string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public bool checkPassword(string passwordTry)
        {
            return password.Equals(passwordTry);
        }
    }
}
