using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace WindowsFormsApplication1
{
    class Simulator
    {
        private int pulse, rpm, speed, time, distance, requested_power, actual_power;
        private double energy;
        private bool commandMode;

        public Simulator()
        {
            Thread updateThread = new Thread(new ThreadStart(update));
            updateThread.Start();
        }

        public String ReceiveCommand(String command)
        {
            String splittedCommand = command.Substring(0,2);
            switch(splittedCommand)
            {
                case "RS":
                    speed = 0;
                    time = 0;
                    pulse = 0;
                    rpm = 0;
                    distance = 0;
                    requested_power = 0;
                    energy = 0;
                    actual_power = 0;
                    commandMode = false;
                    return "ACK";
                case "ID":
                    return "SF1B1706";
                case "VE":
                    return "120";
                case "CM":
                    commandMode = true;
                    return "ACK";
                case "ST":
                    return getStatus();
                case "PW":
                    if (commandMode)
                    {
                        String[] newPower = command.Split(' ');
                        int convertedPower;
                        if (!Int32.TryParse(newPower[1], out convertedPower))
                        {
                            return "ERROR";
                        }
                        else
                        {
                            if (requested_power > 400)
                                requested_power = 400;
                            else if (requested_power < 0)
                                requested_power = 0;
                            else 
                                requested_power = convertedPower;
                            return getStatus();
                        }
                    }
                    return "";
                case "PD":
                    if (commandMode)
                    {
                        String[] newDistance = command.Split(' ');
                        int convertedDistance;
                        if (!Int32.TryParse(newDistance[1], out convertedDistance))
                        {
                            return "ERROR";
                        }
                        else
                        {
                            distance = convertedDistance / 10;
                            return getStatus();
                        }
                    }
                    return "";
                case "PT":
                    if (commandMode)
                    {
                        String[] newTime = command.Split(' ');
                        int convertedTime;
                        if (!Int32.TryParse(newTime[1], out convertedTime))
                        {
                            return "ERROR";
                        }
                        else
                        {
                            int seconds;
                            if (newTime[1].Length < 3)
                            {
                                seconds = Int32.Parse(newTime[1]);
                                if (seconds > 60)
                                    seconds = 59;
                                time = seconds;
                            }
                            else
                            {
                                seconds = Int32.Parse(newTime[1].Substring(newTime[1].Length-2));
                                if (seconds > 60)
                                    seconds = 59;
                                String finalTime = newTime[1].Substring(0,newTime[1].Length-2) + seconds; 
                                time = Int32.Parse(finalTime);
                            }
                            return getStatus();
                        }
                    }
                    return "";
                default:
                    return "";
            }
        }

        private String getStatus()
        {
            String timeString = time.ToString();
            if (time < 1)
                timeString = "00:00";
            else if (time < 10)
                timeString = "00:0" + time;
            else if (time < 100)
                timeString = "00:" + time;
            else if (time < 1000)
                timeString = "0" + timeString.Substring(0, 1) + ":" + timeString.Substring(1,2);
            else
                timeString = timeString.Substring(0, timeString.Length-2) + ":" + timeString.Substring(timeString.Length-2);

           return pulse + "\t" + rpm + "\t" + speed + "\t" + distance + "\t" + requested_power + "\t" + energy + "\t" + timeString + "\t" + actual_power;
        }

        private void update()
        {
            while (true)
            {
                time += 1;
                UpdateValues();
                Thread.Sleep(1000);
            }
        }

        private void UpdateValues()
        {
                //generate rpm
                Random r = new Random();
                int newrpm = r.Next(rpm - 10, rpm + 20);
                if (newrpm > 0)
                    rpm = newrpm;
                if (rpm > 125)
                    rpm -= r.Next(rpm - 120);


                //generate pulse 
                pulse = 60 + (int)(0.5 * rpm);

                //generate speed (km/uur)
                speed = (int)(rpm / 2.5);

                //generate distance (meter)
                int actualDistance = (int)(speed / 3.6) * time;  //when time == 0 distance is also 0
                if (actualDistance > distance)
                    distance = actualDistance;

                //generate engergy (kjoule)
                //formula: energy = weight(75 kg) * hours * (watt / 6) * rpm /100
                energy += 75 * ((double)1.0 / 3600) * ((requested_power / 6) + 1) * ((rpm / 100) + 1);

                //generate actual power
                actual_power = requested_power;
                Thread.Sleep(1000);
            }
        }
    }
}
