using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace F1Countdown.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public int iCurrentRound = 2;
        public string sCurrentRaceName = "Default";
        public string sCurrentCircuit = "Default";
        public string sCurrentDate = "Default";
        public string sCurrentTime = "Default";
        public string sFP1Date = "Default";
        public string sFP1Time = "Default";
        public string sQualiDate = "Default";
        public string sQualiTime = "Default";



        [BindProperty]
        public string sCountdown { get; set; }

        public string sDay = "";
        public string sMonth = "";
        public string sYear = "";
        public string[] date;

        private int iDay = 0;
        private int iMonth = 0;
        private int iYear = 0;
        public int iHour = 0;
        public int iMin = 0;

        public string sHour = "";
        public string sMinute = "";
        private string[] time;

        private static System.Timers.Timer UpdateTimer;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            APICall();

            time = sCurrentTime.Split(":");
            sHour = time[0];
            sMinute = time[1];

            date = sCurrentDate.Split("-");
            sYear = date[0];
            sMonth = date[1];
            sDay = date[2];

            try
            {
                iYear =  Int32.Parse(sYear);
                iMonth = Int32.Parse(sMonth);
                iDay = Int32.Parse(sDay);

                iHour = Int32.Parse(sHour);
                iMin = Int32.Parse(sMinute);
            }
            catch (Exception)
            {
                throw;
            }

            //Remove after fixing update
            DateTime t = DateTime.Now;
            DateTime end = new DateTime(iYear, iMonth, iDay, iHour, iMin, 0);
            int result = DateTime.Compare(t, end);
            TimeSpan timeSpan = end.Subtract(t);
            sCountdown = timeSpan.Days + " Days " + timeSpan.Hours + " Hours " + timeSpan.Minutes + " Minutes " + timeSpan.Seconds + " Seconds";
        }

        public void OnGet()
        {
            ViewData["Message"] = "Welcome to my page!";
            ViewData["Date"] = DateTime.Now;
        }

        public void APICall()
        {
            var url = "http://ergast.com/api/f1/2022/" + iCurrentRound;

            var httpRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);


            var httpResponse = (System.Net.HttpWebResponse)httpRequest.GetResponse();
            var result = "";
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            if (result != null)
            {
                XDocument xml = XDocument.Parse(result);
                XElement root = xml.Root;
                XNamespace ns = root.GetDefaultNamespace();
                
                IEnumerable<XElement> xElements = root.Descendants(ns + "Race").Elements();
                if (xElements != null)
                {
                    sCurrentRaceName = (string)xElements.ElementAt(0);
                    sCurrentCircuit = (string)xElements.ElementAt(1);
                    sCurrentDate = (string)xElements.ElementAt(2);
                    sCurrentTime = (string)xElements.ElementAt(3);
                    IEnumerable<XElement> FPElements = xElements.ElementAt(4).Elements();
                    if(FPElements != null)
                    {
                        sFP1Date = (string)FPElements.ElementAt(0);
                        sFP1Time = (string)FPElements.ElementAt(1);
                    }
                    IEnumerable<XElement> QualiElements = xElements.ElementAt(7).Elements();
                    if (QualiElements != null)
                    {
                        sQualiDate = (string)QualiElements.ElementAt(0);
                        sQualiTime = (string)QualiElements.ElementAt(1);
                    }
                }
            }

        }


    }

}