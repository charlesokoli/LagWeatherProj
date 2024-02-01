using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using LagWeatherProj.ViewModel;
using Newtonsoft.Json;
using System.IO;
namespace LagWeatherProj.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _environment;
        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }
        [BindProperty]
        public ReportVm ReportVm { get; set; }
        public async Task OnGet()
        {
            string cityName = "Lagos";
            await LoadDataAsync(cityName);
        }

        public async Task LoadDataAsync(string cityName)
        {
           // string cityName = "Lagos";

            //get the api key
            string apiKey = "0a69a9a22e384d59a85101244240102";
            string apiUrl = $"http://api.weatherapi.com/v1/current.json?key={apiKey}&q={cityName}&aqi=no";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        //read data from url
                        string data = await response.Content.ReadAsStringAsync();
                        JObject jsonData = JObject.Parse(data);

                        if(jsonData.Count > 0)
                        {
                            // Extract details 
                            var locationname = jsonData["location"]["name"].ToString();
                            var region = jsonData["location"]["region"].ToString();
                            var countryname = jsonData["location"]["country"].ToString();
                            var lat = jsonData["location"]["lat"].ToString();
                            var lon = jsonData["location"]["lon"].ToString();
                            var tz_id = jsonData["location"]["tz_id"].ToString();
                            var localtime_epoch = jsonData["location"]["localtime_epoch"].ToString();
                            var localtime = jsonData["location"]["localtime"].ToString();


                            //passing your data to your view model
                            ReportVm = new ReportVm()
                            {
                                name = locationname,
                                region = region,
                                country = countryname,
                                lat = lat,
                                lon = lon,
                                tz_id = tz_id,
                                localtime_epoch = localtime_epoch,
                                localtime = localtime,
                            };
                        }
                        
                    }
                    else
                    {
                        //error to console log
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    //error to console log
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public async Task OnPostAsync()
        {
            //get data on btn click
            var reportvm = new ReportVm();
            //var cityName = reportvm.name;
            string cityName = "Lagos";
            await LoadDataAsync(cityName);

            if (ReportVm != null)
            {
                //sending to txt file 
                var detail = JsonConvert.SerializeObject(ReportVm, Formatting.Indented);
                var folderPath = Path.Combine(_environment.ContentRootPath, "txtFolder");
                if (Directory.Exists(folderPath) == false)
                    Directory.CreateDirectory(folderPath);
                var filepathd = Path.Combine(folderPath, "weatherfile.txt");
                System.IO.File.AppendAllText(filepathd, detail + Environment.NewLine);
                // return Page();
            }
        }
    }
}