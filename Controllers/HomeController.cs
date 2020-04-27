using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static GoHiking.Models.Db_Models;
using Activity = System.Diagnostics.Activity;
using GoHiking.APIHandlerManager;
using GoHiking.DataAccess;
using System.Net.Http;
using Newtonsoft.Json;
using GoHiking.Models;

namespace GoHiking.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext dbContext;

        //Base URL for the IEXTrading API. Method specific URLs are appended to this base URL.
        string BASE_URL = "https://developer.nps.gov/api/v1/";
        HttpClient httpClient;

        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new
                System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<MyPark> GetParks()
        {
            string IEXTrading_API_PATH = BASE_URL + "/parks";
            string parkList = "";
            List<MyPark> parks = null;

            // connect to the IEXTrading API and retrieve information
            httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();

            // read the Json objects in the API response
            if (response.IsSuccessStatusCode)
            {
                parkList = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            // now, parse the Json strings as C# objects
            if (!parkList.Equals(""))
            {
                
                parks = JsonConvert.DeserializeObject<List<MyPark>>(parkList);
                parks = parks.GetRange(0, 50);
            }

            return parks;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ParkLists()
        {
            APIHandler webHandler = new APIHandler();
            ParkLists parks = webHandler.GetParks();

            return View(parks);
        }

        public IActionResult MyPark()
        {
            //Set ViewBag variable first
            ViewBag.dbSucessComp = 0;
            List<MyPark> parks = GetParks();

            //Save parks in TempData, so they do not have to be retrieved again
            TempData["Parks"] = JsonConvert.SerializeObject(parks);


            return View(parks);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
