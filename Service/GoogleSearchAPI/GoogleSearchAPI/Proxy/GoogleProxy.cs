using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;
using GoogleSearchAPI.Dtos;

namespace GoogleSearchAPI.Proxy
{
    public interface IGoogleProxy
    {
        List<SearchResponseDto> GetSerchData(string keyword);
    }

    public class GoogleProxy : IGoogleProxy
    {
        public List<SearchResponseDto> GetSerchData(string keyword)
        {
            List<SearchResponseDto> result=new List<SearchResponseDto>();

            const string apiKey = "AIzaSyDBX9mmTFd0Zz9Bqf5FtlPii0awL17N7Ew";
            const string searchEngineId = "000644843603944200383:d_mutuemxd0";
            var customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
            var listRequest = customSearchService.Cse.List(keyword);
            listRequest.Cx = searchEngineId;
            IList<Result> paging = new List<Result>();
            var count = 0;
            while (paging != null)
            {
                Console.WriteLine($"Page {count}");
                listRequest.Start = count * 10 + 1;
                paging = listRequest.Execute().Items;
                if (paging != null)
                {
                    foreach (var item in paging)
                    {
                        result.Add(new SearchResponseDto
                        {
                            Text = item.Snippet,
                            Link = item.Link,
                        });
                    }
                }

                count++;
            }
            return result;
        }
    }
}