using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;

namespace Egress_Scraping_Test.Pages
{
    public class IndexModel : PageModel
    {
        private IMemoryCache Cache;
        public IndexModel(IMemoryCache memoryCache) => Cache = memoryCache;

        public Rootobject CachedData;

        public void OnGet()
        {

            // CACHE

            if (!Cache.TryGetValue("UserData", out Rootobject cacheValue))                                  // check if chache contains user data
            {
                // REST API                                                                                 // having REST API here will only execute when we need new data in cache

                var response = new RestClient("https://randomuser.me/api/").Execute(new RestRequest(""));
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    cacheValue = JsonConvert.DeserializeObject<Rootobject>(response.Content);
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3));                                         // can be longer than  seconds 

                Cache.Set("UserData", cacheValue, cacheEntryOptions);                                       // set new user data
            }
            
            CachedData = cacheValue;
        }

    }

    public class Rootobject
    {
        public Result[] results { get; set; }
    }

    public class Result
    {
        public string gender { get; set; }
        public Name name { get; set; }
        public Dob dob { get; set; }
        public Picture picture { get; set; } 
    }

    public class Name
    {
        public string title { get; set; }
        public string first { get; set; }
        public string last { get; set; }
    }

    public class Dob
    {
        public DateTime date { get; set; }
        public int age { get; set; }
    }


    public class Picture
    {
        public string large { get; set; }
        public string medium { get; set; }
        public string thumbnail { get; set; }
    }
}
