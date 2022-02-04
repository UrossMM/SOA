namespace SensorDeviceMicroservice.API.Services
{
    public class SensorServiceList
    {
        private static SensorServiceList sensors_list = null;
        private static readonly object objLock = new object();
        private readonly List<SensorService> _sensors;

        private SensorServiceList()
        {
            _sensors = new List<SensorService>()
            {
                new SensorService("pm25"),
                new SensorService("pm10"),
                new SensorService("SO2"),
                new SensorService("CO"),
                new SensorService("Ozone")

            };
        }

        public List<SensorService> GetSensors()
        {
            if (sensors_list == null)
                sensors_list = new SensorServiceList();
            return _sensors;
        }

        public static SensorServiceList GetSensorsListInstance()
        {
            if (sensors_list == null)
            {
                lock (objLock)
                {
                    if (sensors_list == null)
                    {
                        sensors_list = new SensorServiceList();
                    }
                }
            }

            return sensors_list;
        }
    }
}
