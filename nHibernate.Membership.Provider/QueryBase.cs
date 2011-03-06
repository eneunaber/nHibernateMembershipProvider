using System;
using System.Linq;
using System.Linq.Expressions;

namespace nHibernate.Membership.Provider
{
    public abstract class QueryBase<T>
    {
        public abstract Expression<Func<T, bool>> MatchingCriteria { get; }

        public T SatisfyingElementFrom(IQueryable<T> candidates)
        {
            return SatisfyingElementsFrom(candidates).Single();
        }

        public IQueryable<T> SatisfyingElementsFrom(IQueryable<T> candidates)
        {
            return candidates.Where(MatchingCriteria).AsQueryable();
        }
    }
}