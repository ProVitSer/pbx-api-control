using System.Collections.Generic;


namespace PbxApiControl.Models
{
    public class ExtensionDeviceInfo
    {
        public string Extension { get; set; }

        public List<DevInfo> Devices { get; set; }
    }

    public class DevInfo
    {
        public string UserAgent { get; set; }
        public string Contact { get; set; }
    }
}

