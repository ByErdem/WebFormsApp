using LinqKit;
using System;
using System.Linq;
using System.Linq.Expressions;
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