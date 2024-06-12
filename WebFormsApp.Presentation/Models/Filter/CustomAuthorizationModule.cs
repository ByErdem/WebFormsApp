using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebFormsApp.Entity.Dtos;
using WebFormsApp.Service.Abstract;

namespace WebFormsApp.Presentation.Models.Filter
{
    public class CustomAuthorizationModule : IHttpModule
    {
        private IRedisCacheService _redisCacheService;
        private ITokenService _tokenService;
        private ISessionService _sessionService;
        private readonly List<string> _blacklistedUrls = new List<string>
        {
            "/About",
            // Buraya erişimin engellenmesini istediğimiz url'leri ekleyeceğiz
        };

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new System.EventHandler(OnBeginRequest);
        }

        private void OnBeginRequest(object sender, System.EventArgs e)
        {
            var application = (HttpApplication)sender;
            var context = application.Context;

            _redisCacheService = DependencyResolver.Current.GetService<IRedisCacheService>();
            _tokenService = DependencyResolver.Current.GetService<ITokenService>();
            _sessionService = DependencyResolver.Current.GetService<ISessionService>();

            var requestedUrl = context.Request.Url.AbsolutePath;

            // Eğer istenen URL kara listedeyse yetki kontrolünü yap
            if (_blacklistedUrls.Any(url => requestedUrl.EndsWith(url, StringComparison.OrdinalIgnoreCase)))
            {
                var authHeader = context.Request.Headers["Authorization"];

                if (authHeader != null)
                {
                    var guid = authHeader.Replace("Bearer ", "");
                    var parameters = Task.Run(async () => await _redisCacheService.GetAsync(guid)).Result;
                    var userParameter = JsonConvert.DeserializeObject<UserParameter>(parameters);
                    var validated = _tokenService.ValidateToken(userParameter.Token, userParameter.SecretKey);
                    if (validated)
                    {
                        return;
                    }
                }

                // Bearer token olmadan istek gelirse session'da oturumumuz olup olmadığın kontrol edelim.
                var status = _sessionService.Validate();
                if (!status)
                {
                    context.Response.StatusCode = 401;
                    context.Response.End();
                }
            }
        }

        public void Dispose() { }
    }
}