using bookstore_restapi.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using bookstore_frontendASPNETCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace bookstore_frontendASPNETCoreMVC.Services
{
    public class BookRepositoryFromAPI : IBookRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookRepositoryFromAPI(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task DeleteBookById(int bookId)
        {
            //string idToken = await HttpContext.GetTokenAsync("id_token");
            string accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            // if you need to check the Access Token expiration time, use this value
            // provided on the authorization response and stored.
            // do not attempt to inspect/decode the access token
            //DateTime accessTokenExpiresAt = DateTime.Parse(
            //    await HttpContext.GetTokenAsync("expires_at"),
            //    CultureInfo.InvariantCulture,
            //    DateTimeStyles.RoundtripKind);

            // Now you can use them. For more info on when and how to use the
            // Access Token and ID Token, see https://auth0.com/docs/tokens

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await httpClient.DeleteAsync($"https://localhost:3001/api/Books/{bookId}");
            }
        }

        public async Task<BookDTO> GetBookById(int bookId)
        {
            BookDTO book = null;

            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
                using var response = await httpClient.GetAsync($"https://localhost:3001/api/Books/{bookId}");
                string apiResponse = await response.Content.ReadAsStringAsync();
                book = JsonConvert.DeserializeObject<BookDTO>(apiResponse);
            }
            return book;
        }

        public async Task<IEnumerable<BookDTO>> GetBooks()
        {
            IEnumerable<BookDTO> books = new List<BookDTO>();

            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
                using var response = await httpClient.GetAsync("https://localhost:3001/api/Books");
                string apiResponse = await response.Content.ReadAsStringAsync();
                books = JsonConvert.DeserializeObject<List<BookDTO>>(apiResponse);
            }
            return books;
        }
    }
}
