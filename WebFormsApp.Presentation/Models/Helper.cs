using System.Web.Mvc;

namespace WebFormsApp.Presentation.Models
{
    public static class DependencyResolverHelper
    {
        public static T Resolve<T>()
        {
            return DependencyResolver.Current.GetService<T>();
        }
    }
}