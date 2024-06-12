using System.Threading.Tasks;
using WebFormsApp.Service.Abstract;
using System.Web;
using Newtonsoft.Json;
using WebFormsApp.Entity.Dtos;

namespace WebFormsApp.Service.Concrete
{
    public class SessionManager:ISessionService
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ITokenService _tokenService;

        public SessionManager(IRedisCacheService redisCacheService, ITokenService tokenService)
        {
            _redisCacheService = redisCacheService;
            _tokenService = tokenService;
        }

        public void SetSessionValue(string key, string value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public static bool IsSessionAvailable()
        {
            return HttpContext.Current != null && HttpContext.Current.Session != null;
        }

        public string GetSessionValue(string key)
        {
            if (IsSessionAvailable())
            {
                var status = HttpContext.Current.Session[key];
                if (status != null)
                {
                    return status.ToString();
                }
            }
            return null;
        }

        public bool Validate()
        {
            var guidKey = GetSessionValue("GuidKey");
            if (guidKey == null || guidKey == "")
            {
                return false;
            }

            var parameters = Task.Run(async () => await _redisCacheService.GetAsync(guidKey)).Result;
            var userParameter = JsonConvert.DeserializeObject<UserParameter>(parameters);
            return _tokenService.ValidateToken(userParameter.Token, userParameter.SecretKey);
        }
    }
}
