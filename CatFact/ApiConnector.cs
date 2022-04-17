using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CatFact
{
    class ApiConnector : IApiConnector
    {
        private readonly HttpClient _apiClient;
        private const string _factRequest = "/fact";

        private readonly ILogger<ApiConnector> _logger;

        public ApiConnector(ILogger<ApiConnector> logger)
        {
            _logger = logger;
            //Initialize client
            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(Program.CatFactUrl);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            //_apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<CatFactModel> GetCatFactAsync()
        {
            _logger.LogInformation("Sending request...");

            using (HttpResponseMessage response = await _apiClient.GetAsync(_factRequest))
            {
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    CatFactModel catFactModel = JsonConvert.DeserializeObject<CatFactModel>(json);

                    //Show first 50 characters of fact.
                    if (catFactModel.Fact.Length<=50) 
                    {
                        _logger.LogInformation($"Catfact received: {catFactModel.Fact} Length:{catFactModel.Length}");
                    }
                    else
                    {
                        _logger.LogInformation($"Catfact received: {catFactModel.Fact.Substring(0, 50)}... Length:{catFactModel.Length}");
                    }
                    
                    return catFactModel;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }


    }
}
