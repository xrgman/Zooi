using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1;
using Network;
using System.Threading;

namespace WindowsFormsApplication1
{
    public class Networkconnect : ClientInterface
    {
        private NetworkCommunication networkCommunication;
        public string status { get; set; }
        public string power, time, distance;
        private bool loginOk, isPhysician;

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
        public Tuple<bool,bool> login(string username, string password)
        {
            networkCommunication.sendPacket(new PacketLogin(username,password));
            Thread.Sleep(1000);
            return new Tuple<bool, bool>(loginOk,isPhysician);

        public void sendBikeValues(string power, string time, string distance)
        {
            
        }

        public void sendMeasurement(Measurement measurement)
        {
            //Send object;
        }

        public void loginResponse(bool loginOk, bool isPhysician)
        {
            this.loginOk = loginOk;
            this.isPhysician = isPhysician;
        }
    }
}
