using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace WindowsFormsApplication1
{ 
    class Bike
    {
        private bool usingComm;
        private BikeCommunication bikeCommunication;
        private Simulator simulator;
        //Values of the bike:
        private string model, versionNumber, status, measurement;


        public Bike(string commportNumber)
        {
            bikeCommunication = new BikeCommunication(this);
            if (bikeCommunication.ConnectToCom(commportNumber))
            {
                usingComm = true;
                status = "Connected to " + commportNumber;
            }
            else //When there is nothing connected to the commport;
            {
                simulator = new Simulator();
                status = "Simulator";
            }
        }   

        public void SetStatus(string status)
        {
            this.status = status;
        }

        public string GetStatus()
        {
            if(!bikeCommunication.stillConnected)
                status = "Error: connection lost";
            return status;
        }

        public void Reset()
        {
            if (usingComm)
                bikeCommunication.SendMessage("RS");
            else
                simulator.SendMessage("RS");
        }

        public void SetModel(string model)
        {
            this.model = model;
        }

        public string GetModel()
        {
            if (usingComm)
                bikeCommunication.SendMessage("ID");
            else
                model = simulator.SendMessage("ID");
            Thread.Sleep(40);
            return model;
        }

        public void SetVersionNumber(string versionNumber)
        {
            this.versionNumber = versionNumber;
        }

        public string GetVersionNumber()
        {
            if (usingComm)
                bikeCommunication.SendMessage("VE");
            else
                versionNumber = simulator.SendMessage("VE");
            Thread.Sleep(40);
            return versionNumber;
        }
        
        public void setMeasurement(string measurement)
        {
            this.measurement = measurement;
        }

        public Measurement getMeasurement()
        {
            if (usingComm)
                bikeCommunication.SendMessage("ST");
            else
                measurement = simulator.SendMessage("ST");
            Thread.Sleep(40);
            //Splitting the values:
            if(measurement != null)
            {
                String[] values = measurement.Split('\t');
                Measurement m = new Measurement(Int32.Parse(values[0]), Int32.Parse(values[1]), Int32.Parse(values[2]), Int32.Parse(values[3]), Int32.Parse(values[4]), Int32.Parse(values[5]), values[6], Int32.Parse(values[7]));
                return m;
            }
            return null;
        }

        public void SetPower(int watt)
        {
            if(usingComm)
            {
                bikeCommunication.SendMessage("CM");
                bikeCommunication.SendMessage("PW " + watt);
            }
            else
            {
                simulator.SendMessage("CM");
                simulator.SendMessage("PW " + watt);
            }
        }

        public void SetDistance(int distance)
        {
            if (usingComm)
            {
                bikeCommunication.SendMessage("CM");
                bikeCommunication.SendMessage("PD " + distance);
            }
            else
            {
                simulator.SendMessage("CM");
                simulator.SendMessage("PD " + distance);
            }
        }

        public void setTime(int time)
        {
            if (usingComm)
            {
                bikeCommunication.SendMessage("CM");
                bikeCommunication.SendMessage("PT " + time);
            }
            else
            {
                simulator.SendMessage("CM");
                simulator.SendMessage("PT " + time);
            }
        }
    }
}
