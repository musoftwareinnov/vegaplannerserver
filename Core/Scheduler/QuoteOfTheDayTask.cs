using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using vega;
using vega.Core;
using vega.Core.Utils;
using vega.Persistence;
using vegaplannerserver.Core;

namespace Scheduler.Code
{

    
    public class QuoteOfTheDayTask : IScheduledTask
    {
        //public string Schedule => "* */6 * * *";

        private readonly IBusinessDateRepository businessDateRepository;
        private readonly IUnitOfWork unitOfWork;
        // public QuoteOfTheDayTask(
        //                              IBusinessDateRepository businessDateRepository, 
        //                              IUnitOfWork unitOfWork )
        // {
        //     this.businessDateRepository = businessDateRepository;
        //     this.unitOfWork = unitOfWork;
        // }

        public string Schedule => "*/10 * * * *";       
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Cron every minute!!!!");

            //var dt = new DateTest();

            //SystemDate.Instance.date = 
            var httpClient = new HttpClient();
            //GetMeSomeServiceLocator.Instance.GetService<IUnitOfWork>();
            
            var quoteJson = JObject.Parse(await httpClient.GetStringAsync("http://quotes.rest/qod.json"));

            QuoteOfTheDay.Current = JsonConvert.DeserializeObject<QuoteOfTheDay>(quoteJson["contents"]["quotes"][0].ToString());
        }
    }
    
    public class QuoteOfTheDay
    {
        public static QuoteOfTheDay Current { get; set; }

        static QuoteOfTheDay()
        {
            Current = new QuoteOfTheDay { Quote = "No quote", Author = "Maarten" };
        }
        
        public string Quote { get; set; }
        public string Author { get; set; }
    }
}