using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebFormsApp.Entity.Dtos;
using WebFormsApp.Service.Abstract;

namespace WebFormsApp.Presentation.Models.Filter
{
    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ITokenService _tokenService;
        private readonly ISessionService _sessionService;
        private readonly List<string> _whitelistedUrls;

        public CustomAuthorizeFilter(IRedisCacheService redisCacheService, ITokenService tokenService, ISessionService sessionService)
        {
            _redisCacheService = redisCacheService;
            _tokenService = tokenService;
            _sessionService = sessionService;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {

        }
    }
}