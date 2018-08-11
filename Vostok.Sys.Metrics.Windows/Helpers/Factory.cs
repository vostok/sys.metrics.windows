using System;

namespace Vostok.Sys.Metrics.Windows.Helpers
{
    internal static class Factory
    {
        public static T Create<T>() where T : new () => Cache<T>.Creator();

        private static class Cache<T> where T : new ()
        {
            public static readonly Func<T> Creator;

            static Cache() => Creator = ExpressionsUtility.GenerateFactory<T>();
        }
    }
}