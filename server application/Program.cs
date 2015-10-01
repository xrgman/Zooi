using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace server_application
{
    class Program
    {
        static void Main(string[] args)
        {
            Serverapplication server = new Serverapplication();
            

            server.userClients.Add(new UserClient("ab", "bc"));
            server.userClients.Add(new UserClient("bc", "bc"));
            server.userClients.Add(new UserClient("cd", "bc"));
            server.userClients.Add(new UserClient("de", "bc"));
            server.SaveAllData();
            server.LoadAllData();
            

            Console.Read();
        }
    }
}
