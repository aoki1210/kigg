namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    
    using Domain.Entities;
    
    public class CountStoriesQuery : QueryBase<int>
    {
        private readonly Expression<Func<Story, bool>> predicate;

        public CountStoriesQuery(KiggDbContext context, Expression<Func<Story, bool>> predicate)
            : base(context)
        {
            Check.Argument.IsNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override int Execute()
        {
            return context.Stories.Count(predicate);
        }
    }
}
