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
        private IMemoryCache _memoryCache;
        public IndexModel(IMemoryCache memoryCache) => _memoryCache = memoryCache;

        public Rootobject CachedData;

        public void OnGet()
        {
            // REST API

            var response = new RestClient("https://randomuser.me/api/").Execute(new RestRequest(""));
            Rootobject UserData = null;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                UserData = JsonConvert.DeserializeObject<Rootobject>(response.Content);
            }


            // CACHE

            if (!_memoryCache.TryGetValue("CachedTime", out Rootobject cacheValue))
            {
                cacheValue = UserData;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3));

                _memoryCache.Set("CachedTime", cacheValue, cacheEntryOptions);
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
        public Location location { get; set; }
        public string email { get; set; }
        public Dob dob { get; set; }
        public Registered registered { get; set; }
        public string phone { get; set; }
        public Picture picture { get; set; }
    }

    public class Name
    {
        public string title { get; set; }
        public string first { get; set; }
        public string last { get; set; }
    }

    public class Location
    {
        public Street street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string postcode { get; set; }
    }

    public class Street
    {
        public int number { get; set; }
        public string name { get; set; }
    }


    public class Dob
    {
        public DateTime date { get; set; }
        public int age { get; set; }
    }

    public class Registered
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
