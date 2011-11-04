namespace Kigg.Domain.Entities
{
    using System.Collections.Generic;
    
    using AutoMapper;

    public static class EnumerableExtensions
    {
        public static IEnumerable<TViewModel> Map<TViewModel>(this IEnumerable<object> source)
        {
            return Mapper.Map<IEnumerable<TViewModel>>(source);
        }
    }
}
