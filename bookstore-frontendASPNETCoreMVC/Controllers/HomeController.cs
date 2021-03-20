using bookstore_frontendASPNETCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CommonProj;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using System.Globalization;

namespace bookstore_frontendASPNETCoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<BookDTO> booksList = new List<BookDTO>();
            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
                using (var response = await httpClient.GetAsync("http://localhost:3000/api/Books"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    booksList = JsonConvert.DeserializeObject<List<BookDTO>>(apiResponse);
                }
            }
            //if (User.Identity.IsAuthenticated)
            //{
            //    string idToken = await HttpContext.GetTokenAsync("id_token");
            //    string accessToken = await HttpContext.GetTokenAsync("access_token");

            //    // if you need to check the Access Token expiration time, use this value
            //    // provided on the authorization response and stored.
            //    // do not attempt to inspect/decode the access token
            //    //DateTime accessTokenExpiresAt = DateTime.Parse(
            //    //    await HttpContext.GetTokenAsync("expires_at"),
            //    //    CultureInfo.InvariantCulture,
            //    //    DateTimeStyles.RoundtripKind);


            //    booksList.Add(new() { Author = idToken, Name = accessToken, Id = 333, Price = 777 });
            //    // Now you can use them. For more info on when and how to use the
            //    // Access Token and ID Token, see https://auth0.com/docs/tokens
            //}
            return View(booksList);
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
