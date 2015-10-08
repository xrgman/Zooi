using System;
using System.Collections.Generic;
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
        private List<User> users;
        private User user;

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
        /// <returns>Whether or not the login was succesful</returns>
        public Tuple<bool,bool> login(string username, string password)
        {
            networkCommunication.sendPacket(new PacketLogin(username,password));
            Thread.Sleep(1000);
            return new Tuple<bool, bool>(loginOk,isPhysician);
        }

        public void sendBikeValues(string power, string time, string distance)
        {
            //Do something here or delete this method, otherwise it's wasting space!
        }

        public void sendMeasurement(Measurement measurement)
        {
            //Send object;
            //Do something here or delete this method, otherwise it's wasting space!
        }

        public void LoginResponse(bool loginOk, bool isPhysician)
        {
            this.loginOk = loginOk;
            this.isPhysician = isPhysician;
        }

        //Get all users that are currently online.
        public List<User> GetAllConnectedUsers()
        {
            networkCommunication.sendPacket(new PacketGiveUser("*",false));
            Thread.Sleep(1000);
            return users;
        }

        //Get all users in the database.
        public List<User> GetAllUsers()
        {
            networkCommunication.sendPacket(new PacketGiveUser("*", true));
            Thread.Sleep(1000);
            return users;
        }
        public void GiveUserResponse(User user)
        {
            this.user = user;
        }

        public void GiveUserResponse(List<User> users)
        {
            this.users = users;
        }
    }
}
