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
            if(keyword != null)
            {
                keyword = keyword.Trim();
            }
            else
            {
                keyword = "";
            }

            List<SearchResponseDto> result=new List<SearchResponseDto>();//result list

            const string apiKey1 = "AIzaSyDBX9mmTFd0Zz9Bqf5FtlPii0awL17N7Ew";
            const string searchEngineId1 = "000644843603944200383:d_mutuemxd0";//"010253900752585946578:7zobse4mu6a"

            const string apiKey = "AIzaSyACspQmT6Y8p1ikhNONO-G4QtTUDz59BHg";
            const string searchEngineId = "010253900752585946578:bztsm5jt7b4"; //"010253900752585946578:bztsm5jt7b4"; //"010253900752585946578:7zobse4mu6a";
            var customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
            var listRequest = customSearchService.Cse.List(keyword);
            listRequest.Cx = searchEngineId;
            IList<Result> paging = new List<Result>(); //pagging  is controll more result paggination 
            var count = 0;
            if (paging != null) //while
            {
                Console.WriteLine($"Page {count}");
                listRequest.Start = count * 20 + 1;
                try
                {
                    paging = listRequest.Execute().Items;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro :: {e}");
                    paging = null;
                    try
                    {
                        customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey1 });
                        listRequest = customSearchService.Cse.List(keyword);
                        listRequest.Cx = searchEngineId1;
                        listRequest.Start = count * 20 + 1;
                        paging = listRequest.Execute().Items;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro :: {ex}");
                        paging = null;
                    }

                }
                
                if (paging != null)
                {
                    foreach (var item in paging)
                    {
                        result.Add(new SearchResponseDto
                        {
                            Text = item.Snippet,
                            Link = item.DisplayLink, 
                            
                        });
                    }
                }

                //count++;
            }

            result = result.Take(5).ToList();
            return result;
        }
    }
}

//For consuming Google Custom Search API, luckily we don’t have to write our own code for reading/writing REST object. 
//    Google has also provided a Nuget package Google.Apis.Customsearch.v1 for .NET client. 
//    In your project, just simply add a reference to it over Nuget.



