using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1;

namespace server_application
{
    class UserClient : User
    {
        private List<Session> sessions = new List<Session>();
        

        public UserClient(string username, bool isAdmin, string userPassword): base(username,userPassword)
        {
        }

        public void addSession(DateTime startedDate)
        {
            Session s = new Session(startedDate);
        }

        public void addMeasurement(Measurement measurement)
        {
            Session s = sessions.Last();
            addMeasurement(s, measurement);
        }


        public void addMeasurement(Session session, Measurement measurement)
        {
            session.AddMeasurement(measurement);
        }

        public List<Session> getSessions()
        {
            return sessions;
        }

        public Session getLastSession()
        {
            return sessions.Last();
        }

 /*       public override bool Equals(object obj)
        {
            UserClient temp = obj as UserClient;
            if(temp != null)
            {
                return (userPassword == temp.userPassword);
            }
            return false;
        }*/

        

    }
}
