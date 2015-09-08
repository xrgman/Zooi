using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Simulator
    {
        private int pulse, rpm, speed, time, distance, requested_power, energy, actual_power;

        private bool commandMode;

        public Simulator()
        {
    
        }

        public String ReceiveCommand(String command)
        {
            String splittedCommand = command.Substring(0, 2);
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
                    break;
                case "ID":
                    return "Simulator";
                    break;
                case "VE":
                    return "120";
                    break;
                case "CM":
                    commandMode = true;
                    return "ACK";
                case "ST":
                    return getStatus();
                    break;
                case "PW":
                    String newPower = command.Substring(3, command.Length);
                    int convertedPower;
                    if(!Int32.TryParse(newPower, out convertedPower))
                    {
                        return "ERROR";
                    }
                    else
                    {
                        requested_power = convertedPower;
                        return getStatus();
                    }
                    break;
                case "PD":
                    String newDistance = command.Substring(3, command.Length);
                    int convertedDistance;
                    if (!Int32.TryParse(newDistance, out convertedDistance))
                    {
                        return "ERROR";
                    }
                    else
                    {
                        distance = convertedDistance/10;
                        return getStatus();
                    }
                    break;
                case "PT":
                    String newTime = command.Substring(3, command.Length);
                    int convertedTime;
                    if (!Int32.TryParse(newTime, out convertedTime))
                    {
                        return "ERROR";
                    }
                    else
                    {
                        int seconds = Int32.Parse(command.Substring(command.Length - 2, command.Length));
                        if (seconds > 60)
                            seconds = 59;
                        String finalTime = newTime.Substring(0, command.Length - 2) + seconds;
                        time = Int32.Parse(finalTime);
                        return getStatus();
                    }
                    break;
            }


            return "";   
        }

        private String getStatus()
        {
            String timeString = time.ToString();
            if(timeString.Length > 1)
                timeString = timeString.Substring(0, timeString.Length - 2) + ":" + timeString.Substring(timeString.Length - 2, timeString.Length);
            return pulse + "\t" + rpm + "\t" + speed * 10 + "\t" + distance + "\t" + requested_power + "\t" + energy + "\t" + timeString + "\t" + actual_power;
        }
    }
}
