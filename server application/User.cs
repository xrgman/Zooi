using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server_application
{
    class User
    {
        private List<Session> Sessions = new List<Session>();
        public string userName
        { get; private set; }
        private string password;

        public User(string username, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        public void addSession(DateTime startedDate)
        {
            Session s = new Session(startedDate);
        }

        public void addMeasurement(Measurement measurement)
        {
            Session s = Sessions.Last();
            addMeasurement(s, measurement);
        }


        public void addMeasurement(Session session, Measurement measurement)
        {
            session.AddMeasurement(measurement);
        }

        public List<Session> getSessions()
        {
            return Sessions;
        }

        public Session getLastSession()
        {
            return Sessions.Last();
        }
       
    }
}
