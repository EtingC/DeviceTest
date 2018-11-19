using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceTest.Base;

namespace DeviceTest.Projects
{
    public class LierdaProject
    {


        public static DeviceItem CreateNewLierdaDevice()
        {
            DeviceItem device = new DeviceItem();
            device.modelId = "LIERDA";
            device.online = true;
            device.deleted = false;
            device.registered = true;
            return device;
        }

        //[1] 00124B00129CAA63 Ready
        //[1] 00124B00129CAA63 R=000R Test Failed
        //[5] 00124B00129CA7E0 R=904R OK Please Press Button
        //[5] 00124B00129CA7E0 Test OK

        public static Boolean GetResultWithLine(string line, ref Boolean finished, ref string deviceId, ref string message)
        {
            Boolean result = false;
            if (line.Contains("Start"))
            {
                return result;
            }
            string[] list = line.Split(new char[] { ' ' });
            finished = line.Contains("Test OK");
            if (list.Length > 2)
            {
                deviceId = list[1];
                result = true;
                message = "";
                for (int i = 2; i < list.Length; i++)
                {
                    message += list[i] + " ";
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

    }
}
