using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PIA.DotNet.Interview.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PIA.DotNet.Interview.BloazorWebUI.Data
{
    public class WeatherForecastService
    {
        private readonly HttpClient _httpClient= new HttpClient();
        private readonly string _remoteServiceBaseUrl = "http://localhost:5001/";
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            var rng = new Random();
            return System.Threading.Tasks.Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray());
        }

        public async Task<List<TaskViewModel>> Get()
        {
            try
            {
                var responseString = await _httpClient.GetStringAsync(String.Format("{0}api/Task/GetTasks", _remoteServiceBaseUrl));
                var TaskViewModel = JsonConvert.DeserializeObject<List<TaskViewModel>>(responseString);
                return TaskViewModel;
            }
            catch (Exception ex)
            {
                return new List<TaskViewModel>();
            }
        }
    }
}
