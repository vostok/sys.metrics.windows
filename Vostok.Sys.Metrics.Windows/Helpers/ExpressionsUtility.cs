using System;
using System.Linq.Expressions;

namespace Vostok.Sys.Metrics.Windows.Helpers
{
    internal static class ExpressionsUtility
    {
        public static Func<T> GenerateFactory<T>() where T : new ()
            => Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
    }
}