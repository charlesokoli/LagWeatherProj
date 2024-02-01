using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using LagWeatherProj.ViewModel;
using Newtonsoft.Json;

namespace LagWeatherProj.Pages
{
    public class ReportPageModel : PageModel
    {
        public ReportVm reportVm { get; set; }
        public async Task OnGet()
        {
            await LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            string cityName = "Lagos";
            string apiKey = "0a69a9a22e384d59a85101244240102";
            string apiUrl = $"http://api.weatherapi.com/v1/current.json?key={apiKey}&q={cityName}&aqi=no";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                        JObject jsonData = JObject.Parse(data);

                        // Extract details 
                        string locationname = jsonData["location"]["name"].ToString();
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public async Task<IActionResult> OnPostRepowsync(string toDo)
        {
            return Page();

        }
    }
}





