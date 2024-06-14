using System;

namespace WebFormsApp.Service.Helpers
{
    public static class PredicateExtensions
    {
        public static Func<T, bool> And<T>(this Func<T, bool> first, Func<T, bool> second)
        {
            return x => first(x) && second(x);
        }
    }
}
