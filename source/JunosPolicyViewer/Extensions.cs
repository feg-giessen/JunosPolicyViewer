using System;

namespace JunosPolicyViewer
{
    internal static class Extensions
    {
        public static TResult Try<TSource, TResult>(this TSource source, Func<TSource, TResult> selector)
            where TSource: class
        {
            if (source == null)
                return default(TResult);

            return selector(source);
        }
    }
}
