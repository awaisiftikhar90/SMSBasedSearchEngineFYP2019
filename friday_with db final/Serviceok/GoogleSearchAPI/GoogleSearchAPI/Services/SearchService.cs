using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GoogleSearchAPI.Dtos;
using GoogleSearchAPI.Proxy;

namespace GoogleSearchAPI.Services
{
    public interface ISearchService
    {
        List<SearchResponseDto> SerchData(string keyword);
    }

    public class SearchService : ISearchService
    {
        public List<SearchResponseDto> SerchData(string keyword)
        {
            IGoogleProxy proxy = new GoogleProxy();
            var result = proxy.GetSerchData(keyword);
            
            // TODO: Select top 10 results here.. 

            // TODO: Database save call..

            return result;
        }
    }
}