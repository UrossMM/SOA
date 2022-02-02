using CsvHelper;
using CsvHelper.Configuration;
using SensorDeviceMicroservice.API.Model;
using System.Globalization;
using System.Timers;

namespace SensorDeviceMicroservice.API.Services
{
    public class SensorService
    {
        public const float DEFAULT_THRESHOLD = 1500;
        public float Threshold { get; set; }
        public double Timeout { get; set; }
        public bool IsThresholdSet { get; set; }
        // public double Value { get; set; }
        //public string SensorType { get; set; }
        public Data DataToProceed { get; set; }

        public string _filePath;

        private readonly System.Timers.Timer _timer;

        private StreamReader _streamReader;

        private CsvReader _csv;

        private static bool _shouldTimerWork = false;

        public SensorService(string sensorType)
        {
            Threshold = DEFAULT_THRESHOLD;
            Timeout = 2000;
            _timer = new System.Timers.Timer(Timeout);
            _timer.Elapsed += OnTimerEventAsync;
            DataToProceed = new Data();
            DataToProceed.SensorType = sensorType;
            _filePath = "./Resources/air_pol_delhi.csv";
            configureCsv();
            IsThresholdSet = false;
        }

        private void configureCsv()
        {
            _streamReader = new StreamReader(_filePath);
            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture);
            _csv = new CsvReader(_streamReader, config);
            _csv.Read();
            _csv.ReadHeader();
        }

        public void SensorStop()
        {
            _shouldTimerWork = false;
            _timer.Enabled = false;
        }

        public void SensorStart()
        {
            _shouldTimerWork = true;
            _timer.Enabled = true;
        }

        private async void OnTimerEventAsync(object sender, ElapsedEventArgs args)
        {
            if (_shouldTimerWork)
            {
                ReadValue();
                Console.WriteLine(DataToProceed.SensorType);
                Console.WriteLine(DataToProceed.Value);
                HttpClient httpClient = new HttpClient();
                var responseMessage = await httpClient.PostAsJsonAsync("http://datamicroservice.api/api/Data/AddData", DataToProceed);
                Console.WriteLine(responseMessage);
            }

        }

        public void SetTimeout(double interval)
        {
            _timer.Stop();
            this.Timeout = interval;
            _timer.Interval = interval;
            _timer.Start();
        }


        private void ReadValue()
        {
            try
            {
                string sensor_value;
                string id;
                string city;
                string siteName;
                string site;
                string qName;
                string toDate;
                string fromDate;

                if (_csv.Read())
                {
                    sensor_value = _csv.GetField<string>(DataToProceed.SensorType);
                    id = _csv.GetField<string>("id");
                    city = _csv.GetField<string>("city");
                    siteName = _csv.GetField<string>("site_name");
                    site = _csv.GetField<string>("site");
                    qName = _csv.GetField<string>("query_name");
                    toDate = _csv.GetField<string>("to_date");
                    fromDate = _csv.GetField<string>("from_date");
                }

                else
                {
                    _streamReader.DiscardBufferedData();
                    using (_csv) { }
                    configureCsv();
                    _csv.Read();
                    sensor_value = _csv.GetField<string>(DataToProceed.SensorType);
                    id = _csv.GetField<string>("id");
                    city = _csv.GetField<string>("city");
                    siteName = _csv.GetField<string>("site_name");
                    site = _csv.GetField<string>("site");
                    qName = _csv.GetField<string>("query_name");
                    toDate = _csv.GetField<string>("to_date");
                    fromDate = _csv.GetField<string>("from_date");
                }
                DataToProceed.Id = Int32.Parse(id);
                DataToProceed.City = city;
                DataToProceed.SiteName = siteName;
                DataToProceed.Site = site;
                DataToProceed.QueryName = qName;
                DataToProceed.ToDate = Convert.ToDateTime(toDate);
                DataToProceed.FromDate = Convert.ToDateTime(fromDate);
                DataToProceed.Value = decimal.Parse(sensor_value, CultureInfo.InvariantCulture);
            }
            catch (IOException e)
            {
                Console.WriteLine("Something went wrong,this file can not be read: ");
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
