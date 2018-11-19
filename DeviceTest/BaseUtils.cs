using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceTest
{

    public enum Result
    { 
        OK = 0,
        FAIL = 1
    }
    public class StringResource
    {
        private const string CONFIG_FILENAME = "config.ini";
        private const string MAIN_DATA_FOLDER = "data\\";
        private const string MAIN_DEVICES_FOLDER = "devices\\";
        private const string MAIN_PROJECTS_FOLDER = "projects\\";
        private const string PROFILE_CONFIG_FILENAME = "profile.cfg";
        private const string TEST_CONFIG_FILENAME = "test.cfg";
        private const string DEFAULT_PROJECT_PRINT_FILE = "label.prn";
        private const string DEFAULT_PROJECT_SN_LIST_FILE = "sn.txt";

        private const string ABOUT_FILENAME = "About.exe";
        private const string AUTO_UPGRADER_FILENAME = "AutoUpgrader.exe";
        private const string EXCEPTION_DIRECTORY ="exception\\" ;




        public const string DEFAULT_BASE_URL = "http://www.honyarcloud.com:8090/";

        public const string REFRESH_DEVICE_NODES_URL = "zigbee/products/list/";
        public const string DEVICE_RECORED_ADD_URL   =  "zigbee/products/record/add/";

        public const string REFRESH_GATEWAY_NODES_URL = "wifi/products/list/";
        public const string GATEWAY_TYPE_BIND_URL = "wifi/products/bind/";

        public const string QUERY_REGISTER_DEVICE_URL =  "device/register/query/";
        public const string QUERY_SERIAL_NUMBER_DEVICE_URL = "device/sn/query/";
        public const string QUERY_SERIAL_NUMBER_LIST_URL = "device/sn/list/";

        public const string QUERY_PRODUCT_NODES_URL = "http://product.smart.hongyancloud.com/product/product.js";
        


        public const string GATEWAY_DEVICE_ID = "0000000000000000"; 

        public static string AppPath
        {
            get
            {
                return System.AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public static string ProfileFileName
        {
            get {
                return "\\" + PROFILE_CONFIG_FILENAME;
            }
        }

        public static string TestConfigFileName
        {
            get
            {
                return "\\" + TEST_CONFIG_FILENAME;
            }        
        }

        public static string ExceptionDirectory
        {
            get
            {
                return AppPath + EXCEPTION_DIRECTORY;
            }
        }
            
        public static string AboutFileName
        {
            get {
                return AppPath + ABOUT_FILENAME;
            }
        }

        public static string AutoUpgraderFileName
        {
            get
            {
                return AppPath + AUTO_UPGRADER_FILENAME;
            }
        }
        public static string ProjectsPath
        {
            get
            {
                return AppPath + MAIN_PROJECTS_FOLDER;
            }
        }

        public static string ProjectConfigFileName
        {
            get {
                return "\\"+CONFIG_FILENAME;
            }
        }

        public static string ProjectDefaultSNListFile
        {
            get {
                return DEFAULT_PROJECT_SN_LIST_FILE;
            }
        }
        public static string ProjectDefaultPrintLabelFile
        {
            get
            {
                return  DEFAULT_PROJECT_PRINT_FILE;
            }
        }
        public static string DevicesPath
        {
            get {
                return AppPath + MAIN_DEVICES_FOLDER;
            }
        }
        public static string ConfigFileName
        {
            get
            {
                return AppPath + CONFIG_FILENAME;
            }
        }

        public static string TestHistoryFolder
        {
            get {
                return AppPath + MAIN_DATA_FOLDER + DateTime.Now.ToString("yyyy-MM-dd")+"\\";
            }
        }

        public static string TestHistoryFileName
        {
            get {
                return DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            }
        }

        public static string MainDataPath
        {
            get
            {
                return AppPath + MAIN_DATA_FOLDER;
            }
        }


    }
}
