namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    
    using DomainObjects;
    
    public class CountStoryViewsQuery : QueryBase<int>
    {
        private readonly Expression<Func<StoryView, bool>> predicate;

        public CountStoryViewsQuery(KiggDbContext context, Expression<Func<StoryView, bool>> predicate)
            : base(context)
        {
            Check.Argument.IsNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override int Execute()
        {
            return context.Views.Count(predicate);
        }
    }
}
