using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CatFact
{
    public class App : IApp
    {
        IApiConnector _apiConnector;
        IFileConnector _fileConnector;
        public EventHandler<System.ConsoleKey> KeyPressed;

        private readonly ILogger<App> _logger;
        public App(IApiConnector apiConnector, IFileConnector fileConnector, ILogger<App> logger)
        {
            _apiConnector = apiConnector;
            _fileConnector = fileConnector;
            _logger = logger;
        }

        public void Run()
        {
            ConsoleKeyInfo cki;
            Console.WriteLine("*ENTER* - new request    *ESC* - quit");
            KeyPressed += ProcessData;
            while (true)
            {
                while (Console.KeyAvailable) Console.ReadKey(false); //clear buffer
                cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Enter)
                {
                    KeyPressed?.Invoke(this, cki.Key); //Request and Save CatFact
                }
                if (cki.Key == ConsoleKey.Escape)
                {
                    break; //End program
                }
                //Thread.Sleep(100); //block ReadKey for moment
            }


        }

        private async void ProcessData(object sender, ConsoleKey e)
        {
            try
            {
                CatFactModel catFactModel = await _apiConnector.GetCatFactAsync();
                string json = JsonConvert.SerializeObject(catFactModel);
                await _fileConnector.SaveToFileAsync(json);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Request Failed: {ex.Message}");
            }
            
        }

    }
}
