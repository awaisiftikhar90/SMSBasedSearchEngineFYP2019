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
    [RoutePrefix("api")]
    public class SearchController : ApiController
    {
        [Route("v1/search")]
        [HttpPost]
        public List<SearchResponseDto> Post(SearchRequestDto request)
        {
            ISearchService service = new SearchService();
            return service.SerchData(request.Keyword);
        }
    }
}
