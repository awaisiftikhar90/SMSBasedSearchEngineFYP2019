using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoogleSearcAPClient
{
    public class GoogleSearchAPIProxy
    {
        private HttpClient _client;
        public GoogleSearchAPIProxy()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:51343/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<SearchResponseDto> GetGoogleSearchData(string keyword)
        {
            var body = new SearchRequestDto
            {
                Keyword = keyword
            };

            var inputJson = JsonConvert.SerializeObject(body);
            HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
            // Call Your API
            var response = _client.PostAsync("api/v1/search", inputContent).Result;

            // Extract Data from API the API response
            var finalResult = JsonConvert.DeserializeObject<List<SearchResponseDto>>(response.Content.ReadAsStringAsync().Result);
            return finalResult;
        }
    }
}
