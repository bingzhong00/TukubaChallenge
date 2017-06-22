using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRSystemConfig
{
    public class CarSetting
    {
        public string CarName;
    }
    /*
    static public class CarType {
        CarSetting Cersio( carName = "" );
    }
    */


    static public class Cersio
    {
        static string CarName = "Cersio";
    }

    static public class Benz // : CarSetting
    {
        static string CarName = "Benz";
    }

}
