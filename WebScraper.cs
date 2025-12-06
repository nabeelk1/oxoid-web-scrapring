using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;


namespace oxoid_web_scrapring
{
    public class WebScraper
    {

        private readonly string[] _proxies;
        private int _currentProxyIndex = 0;

        private readonly string _flareSolverrUrl;

        public WebScraper(string proxyFilePath, string flareSolverrUrl = "http://localhost:8191/v1")
        {
            _flareSolverrUrl = flareSolverrUrl;

            if (!File.Exists(proxyFilePath))
                throw new FileNotFoundException("Proxy file not found", proxyFilePath);

            _proxies = File.ReadAllLines(proxyFilePath);
            if (_proxies.Length == 0)
                throw new Exception("Proxy file is empty!");
        }

        // Rotate proxies automatically
        private string GetNextProxy()
        {
            var proxy = _proxies[_currentProxyIndex];
            _currentProxyIndex = (_currentProxyIndex + 1) % _proxies.Length;
            return proxy.Trim();
        }

        public async Task<string> GetPageAsync(string url)
        {
            string proxy = GetNextProxy();

            using (var client = new HttpClient())
            {
                var body = new
                {
                    cmd = "request.get",
                    url = url,
                    maxTimeout = 60000,
                    proxy = new
                    {
                        url = $"http://{proxy}"
                    }
                };

                string jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(body);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(_flareSolverrUrl, content);
                string responseText = await response.Content.ReadAsStringAsync();

                var json = JObject.Parse(responseText);

                if (json["solution"] == null)
                    throw new Exception("FlareSolverr returned no solution. Response: " + responseText);

                string html = json["solution"]["response"]?.ToString() ?? "";

                return html;
            }
        }



    }
}
