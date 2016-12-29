using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSphere.Plugins.RandomGeneratorExample
{
    public class DataPointViewModel
    {
        public DataPointColor color;
        public float declination;
        public float rightAscension;
    }

    public class DataPointColor
    {
        public float r = 1;
        public float g = 1;
        public float b = 1;
        public float a = 1;
    }
}
