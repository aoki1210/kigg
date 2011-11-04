namespace Kigg.Web.BoostrapperTasks
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    
    using MvcExtensions;
    
    using Domain.Entities;

    public class AutoMapperBootstrapper : BootstrapperTask
    {
        public override TaskContinuation Execute()
        {
            const string mappingNamespace = "Kigg.DomainModel.Mapping";

            IEnumerable<Type> mappingTypes = typeof(IDomainObject).Assembly
                .GetTypes()
                .Where(
                    type =>
                    type.IsPublic &&
                    type.IsClass &&
                    !type.IsAbstract &&
                    !type.IsGenericType &&
                    type.Namespace == mappingNamespace);

            mappingTypes.ForEach(t => Activator.CreateInstance(t));
            
            return TaskContinuation.Continue;
        }
    }
}
