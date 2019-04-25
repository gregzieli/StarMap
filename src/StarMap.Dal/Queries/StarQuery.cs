using StarMap.Cll.Abstractions;
using StarMap.Core.Extensions;
using StarMap.Dal.Database.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StarMap.Dal.Queries
{
    public class StarQuery : IQuery<Star>
    {
        public double Distance { get; set; }

        public double Magnitude { get; set; }

        public string Designation { get; set; }

        public IEnumerable<Expression<Func<Star, bool>>> AllFilters
        {
            get
            {
                yield return _isCloserThan;

                yield return _isSmallerThan;

                if (!Designation.IsNullOrWhiteSpace())
                    yield return _hasDesignationLike;
            }
        }

        private Expression<Func<Star, bool>> _isCloserThan =>
            x => x.ParsecDistance <= Distance;

        private Expression<Func<Star, bool>> _isSmallerThan =>
            x => x.ApparentMagnitude <= Magnitude;

        private Expression<Func<Star, bool>> _hasDesignationLike => x =>
            x.ProperName.Contains(Designation) ||
            x.BayerName.Contains(Designation) ||
            x.FlamsteedName.Contains(Designation) ||
            Designation.Contains(x.ProperName) ||
            Designation.Contains(x.BayerName) ||
            Designation.Contains(x.FlamsteedName);
    }
}
