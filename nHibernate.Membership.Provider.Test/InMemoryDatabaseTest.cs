using System;
using System.Reflection;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using nHibernate.Membership.Provider.Entities;
using NHibernate.Tool.hbm2ddl;

namespace nHibernate.Membership.Provider.Test
{
    /*
	 Code Courtesy of ayende.
	 * http://ayende.com/Blog/archive/2009/04/28/nhibernate-unit-testing.aspx
	 */

    public class InMemoryDatabaseTest : IDisposable
    {
        private static Configuration Configuration;
        private static ISessionFactory SessionFactory;
        protected ISession session;

        public InMemoryDatabaseTest(Assembly assemblyContainingMapping)
        {
//              This Works
//            if (Configuration == null)
//            {
//                Configuration = new Configuration()
//                    .SetProperty(NHibernate.Cfg.Environment.ReleaseConnections, "on_close")
//                    .SetProperty(NHibernate.Cfg.Environment.Dialect, typeof(SQLiteDialect).AssemblyQualifiedName)
//                    .SetProperty(NHibernate.Cfg.Environment.ConnectionDriver, typeof(SQLite20Driver).AssemblyQualifiedName)
//                    .SetProperty(NHibernate.Cfg.Environment.ConnectionString, "data source=:memory:")
//                    .SetProperty(NHibernate.Cfg.Environment.ProxyFactoryFactoryClass, typeof(ProxyFactoryFactory).AssemblyQualifiedName)
//                    .AddAssembly(assemblyContainingMapping);
//
//                SessionFactory = Configuration.BuildSessionFactory();
//            }
//
//            session = SessionFactory.OpenSession();
            //new SchemaExport(Configuration).Execute(true, false, true, session.Connection, Console.Out);

            if (Configuration == null)
            {
                var cfg = new StoreConfiguration();
                Configuration = Fluently.Configure()
                    .Database(
                        SQLiteConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey("MyConnectionString"))
                        )
                    .Mappings(m =>m.AutoMappings.Add(AutoMap.AssemblyOf<User>(cfg)))
                    .BuildConfiguration();

                SessionFactory = Configuration.BuildSessionFactory();
            }

            session = SessionFactory.OpenSession();
            new SchemaExport(Configuration).Execute(true, false, true, session.Connection, Console.Out);
        }

        #region IDisposable Members

        public void Dispose()
        {
            session.Dispose();
        }

        #endregion
    }

    public class StoreConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.Namespace == " nHibernate.Membership.Provider.Entities";
        }
    }
}