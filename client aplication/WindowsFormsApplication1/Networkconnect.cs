using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1;
using Network;

namespace WindowsFormsApplication1
{
    public class Networkconnect
    {
        private NetworkCommunication networkCommunication;
        public string status { get; set; }
        public string power, time, distance;

        public Networkconnect(string ipAdress, int port)
        {
            networkCommunication = new NetworkCommunication(ipAdress, port, this);
            if (networkCommunication.ConnectToServer())
                status = "Connected";
            else
                status = "Can't connect to: " + ipAdress + ":" + port;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <returns>Wether or not the login was succesful</returns>
        public bool login(string username, string password)
        {
            networkCommunication.sendPacket(new PacketLogin(username,password));
            return true;
        }

        public void sendBikeValues(string power, string time, string distance)
        {
            
        }

        public void sendMeasurement(Measurement measurement)
        {
            //Send object;
        }
    }
}
