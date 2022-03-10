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

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            var url = "http://ergast.com/api/f1/2022/1";

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

                
                var raceName = xml.Element("MRData").Element("RaceTable").Element("Race").Element("RaceName").FirstNode;
                Debug.WriteLine(raceName);
            }
        }


    }
}