using System;
using System.Collections.Generic;
using Network;
using System.Threading;
using Network.Packets;

namespace WindowsFormsApplication1
{
    public class Networkconnect : ClientInterface
    {
        private NetworkCommunication networkCommunication;
        public string status { get; set; }
        public string power { get; set; }
        public string time { get; set; }
        public string distance { get; set; }
        private bool loginOk, isPhysician;
        private List<User> users;
        private User user;
        private FormClient parent;

        public Networkconnect(string ipAdress, int port)
        {
            networkCommunication = new NetworkCommunication(ipAdress, port, this);
            if (networkCommunication.ConnectToServer())
                status = "Connected";
            else
                status = "Can't connect to: " + ipAdress + ":" + port;
        }

        public void SetParent(FormClient parent)
        {
            this.parent = parent;
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

        public void addNewClient(User user, string physicianName)
        {
            networkCommunication.sendPacket(new PacketAddClient(user, physicianName));
        }

        public void sendBikeValues(string power, string time, string distance, string username)
        {
            networkCommunication.sendPacket(new PacketBikeValues(power, time, distance, username));
        }

        public void sendMeasurement(Measurement measurement)
        {
            //Send object;
            //Do something here or delete this method, otherwise it's wasting space!
        }
        
        public void sendChatMessage(string message, string sender, string receiver)
        {
            networkCommunication.sendPacket(new PacketChatMessage(message, sender, receiver));
        }

        public void sendBroadcast(string message, string sender)
        {
            networkCommunication.sendPacket(new PacketBroadcast(message, sender));
        }

       /* public void getChatMessage(PacketChatMessage chatPacket)
        {
            parent.getChatMessage(chatPacket.sender, chatPacket.message);
        }

        public void getBroadcast(PacketBroadcast broadcastPacket)
        {
            parent.getChatMessage(broadcastPacket.sender, broadcastPacket.message);
        }*/

        public void loginResponse(bool loginOk, bool isPhysician)
        {
            this.loginOk = loginOk;
            this.isPhysician = isPhysician;
        }

        //Get all users that are currently online.
        public List<User> GetAllConnectedUsers(string physicianName)
        {
            networkCommunication.sendPacket(new PacketGiveUser("*",false,physicianName));
            Thread.Sleep(1000);
            return users;
        }

        //Get all users in the database.
        public List<User> GetAllUsers()
        {
            networkCommunication.sendPacket(new PacketGiveUser("*", true,""));
            Thread.Sleep(1000);
            return users;
        }

        public User getUser(string username)
        {
            return null;
        }

        public void GiveUserResponse(User user)
        {
            this.user = user;
        } 

        public void GiveUserResponse(List<User> users)
        {
            this.users = users;
        }

        public void LoginResponse(bool loginOk, bool isPhysician)
        {
            this.loginOk = loginOk;
            this.isPhysician = isPhysician;
        }

        public void GiveUserResponse(List<UserClient> users)
        {
            List<User> tempusers = users.ConvertAll(user => (User)user);
            
            this.users = tempusers;
        }

        public void BikeValuesResponse(string power, string time, string distance)
        {
            this.power = power;
            this.time = time;
            this.distance = distance;
            parent.setBikeValues(power, time, distance);
        }

        public void ChatMessageResponse(string sender, string receiver, string message)
        {
           // throw new NotImplementedException();
        }

        public void BroadcastResponse(string sender, string message)
        {
           // throw new NotImplementedException();
        }
        
    }
}
