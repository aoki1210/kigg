namespace Kigg.Infrastructure.EntityFramework
{
    using System;
    using System.Diagnostics;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration;
    using System.Collections.Generic;
    using System.Linq;

    public class KiggDbFactory : Disposable, IKiggDbFactory
    {
        private static readonly object syncObject = new object();

        private readonly DbProviderFactory providerFactory;
        private readonly string connectionString;

        private static volatile DbCompiledModel model;

        private KiggDbContext dbContext;

        public KiggDbFactory(DbProviderFactory providerFactory, string connectionString)
        {
            Check.Argument.IsNotNull(providerFactory, "providerFactory");
            Check.Argument.IsNotNullOrEmpty(connectionString, "connectionString");

            this.providerFactory = providerFactory;
            this.connectionString = connectionString;
        }

        public KiggDbContext Get()
        {
            if (dbContext == null)
            {
                DbConnection connection = providerFactory.CreateConnection();

                if (connection != null)
                {
                    connection.ConnectionString = connectionString;

                    if (model == null)
                    {
                        lock (syncObject)
                        {
                            if (model == null)
                            {
                                model = CreateDbModel(connection);
                            }
                        }
                    }

                    dbContext = new KiggDbContext(connection, model);
                }

                return dbContext;
            }

            return dbContext;
        }

        //[DebuggerStepThrough]
        protected override void DisposeCore()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }

        private static DbCompiledModel CreateDbModel(DbConnection connection)
        {
            var modelBuilder = new DbModelBuilder();

            IEnumerable<Type> configurationTypes = typeof(KiggDbFactory).Assembly
                .GetTypes()
                .Where(
                    type =>
                    type.IsPublic && type.IsClass && !type.IsAbstract && !type.IsGenericType && type.BaseType != null &&
                    type.BaseType.IsGenericType &&
                    (type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>) ||
                     type.BaseType.GetGenericTypeDefinition() == typeof(ComplexTypeConfiguration<>)) && (type.GetConstructor(Type.EmptyTypes) != null));

            foreach (var configuration in configurationTypes.Select(Activator.CreateInstance))
            {
                modelBuilder.Configurations.Add((dynamic)configuration);
            }

            return modelBuilder.Build(connection).Compile();
        }

    }
}
