using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace StarMap.Cll.Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Filters a sequence of values based on multiple predicates evaluated in an "AndAlso" fashion.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="AsyncTableQuery{T}"/> to filter.</param>
        /// <param name="predicates">Functions to test each element for a different condition.</param>
        /// <returns>An <see cref="AsyncTableQuery{T}"/> that contains elements from the input sequence that satisfy the conditions specified by the predicates.</returns>
        /// <exception cref="ArgumentNullException">source or predicate is null</exception>
        public static AsyncTableQuery<T> Where<T>(this AsyncTableQuery<T> source, IEnumerable<Expression<Func<T, bool>>> predicates)
            where T : new()
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicates is null)
            {
                throw new ArgumentNullException(nameof(predicates));
            }

            foreach (var predicate in predicates)
            {
                source = source.Where(predicate);
            }

            return source;
        }
    }
}
