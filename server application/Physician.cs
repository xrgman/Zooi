using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server_application
{
    class Physician : User
    {
        private List<UserClient> clients = new List<UserClient>();
        public Physician(String username, string password): base(username, password)
        {
        }

        public void addClient(UserClient client)
        {
            clients.Add(client);
        }
    }
}
