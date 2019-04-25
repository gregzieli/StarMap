using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StarMap.Cll.Abstractions
{
    /// <summary>
    /// Basic interface for the query object.
    /// </summary>
    /// <typeparam name="T">Source or result of the query. Can be a domain/persistance entity or an api/view model.</typeparam>
    public interface IQuery<T>
    {
        IEnumerable<Expression<Func<T, bool>>> AllFilters { get; }
    }
}
