using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFormsApp.Presentation.Models;
using WebFormsApp.Service.Abstract;

namespace WebFormsApp.Presentation
{
    public partial class _Default : Page
    {
        private readonly ISessionService _sessionService;

        public _Default()
        {
            _sessionService = DependencyResolverHelper.Resolve<ISessionService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _sessionService.SetSessionValue("GuidKey",Guid.NewGuid().ToString());
            var value = _sessionService.GetSessionValue("GuidKey");
        }
    }
}