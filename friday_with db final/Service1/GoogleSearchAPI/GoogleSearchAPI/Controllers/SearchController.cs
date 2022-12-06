using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GoogleSearchAPI.Dtos;
using GoogleSearchAPI.Services;
using Microsoft.Ajax.Utilities;

namespace GoogleSearchAPI.Controllers
{
    [RoutePrefix("api")] // address binding contruct    adress local host  url   binding which protocol http  contruct which service provide we use search   arranging  end point  kis content  ko arrange

    public class SearchController : ApiController
    {
        [Route("v1/search")]       //contruct route  
        [HttpPost]                              // get put post delet      Action  hai ,,    Restfull k differnt action hain   bcz  db  entry krni thi 
        public List<SearchResponseDto> Post(SearchRequestDto request)
        {
            ISearchService service = new SearchService();
            return service.SerchData(request.Keyword);
        }
    }
}


//interface contruct this class have this spessification