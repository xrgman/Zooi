using System;
using System.Runtime.Serialization;

namespace Network
{
    [Serializable]
    public class User 
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool isOnline { get; set; }

        public User (string username, string password)
        {
            this.username = username.ToLower();
            this.password = PasswordHash.HashPassword(password);
        }

        //deserialization function
        public User(SerializationInfo info, StreamingContext ctxt)
        {
            username = (string)info.GetValue("username", typeof(string));
            password = (String)info.GetValue("password", typeof(string));
        }

        public User()
        {

        }

        public bool checkPassword(string passwordTry)
        {
            return password.Equals(passwordTry);
        }
        
        //serialize method
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("username", username);
            info.AddValue("password", password);
            //info.AddValue("clients", clients);
        }

        public override string ToString()
        {
            return username + " - " + isOnline;
        }

    }
}
