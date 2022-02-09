using System;
using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExtension
{
    private sealed class CommonSelector<T, Tkey> : IEqualityComparer<T>
    {
        private readonly Func<T, Tkey> m_selector;

        public CommonSelector(Func<T, Tkey> selector)
        {
            m_selector = selector;
        }

        public bool Equals(T x, T y)
        {
            return m_selector(x).Equals(m_selector(y));
        }

        public int GetHashCode(T obj)
        {
            return m_selector(obj).GetHashCode();
        }
    }

    public static IEnumerable<T> Distinct<T, Tkey>(
        this IEnumerable<T> source,
        Func<T, Tkey> selector
        )
    {
        return source.Distinct(new CommonSelector<T, Tkey>(selector));
    }
}
