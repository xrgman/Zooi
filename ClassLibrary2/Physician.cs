using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Network
{
    [Serializable]
    public class Physician : User, ISerializable
    {
        public List<UserClient> clients = new List<UserClient>();
            
        public Physician(String username, string password): base(username, password)
        {
            clients = new List<UserClient>();
        }

        //deserialize method (constructor)
        public Physician(SerializationInfo info, StreamingContext ctxt)
        {
            username = (string)info.GetValue("username", typeof(string));
            password = (string)info.GetValue("password", typeof(string));
            clients = (List<UserClient>)info.GetValue("clients", typeof(List<UserClient>));

        }

        public void addClient(UserClient client)
        {
            clients.Add(client);
        }

        //serialize method
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("username", username);
            info.AddValue("password", password);
            info.AddValue("clients", clients);
        }
    }
}
