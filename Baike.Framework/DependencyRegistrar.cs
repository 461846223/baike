using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Integration.Mvc;
using Baike.Data;
using Baike.Framework;

namespace Baike.Framework
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //data layer
          
            builder.Register(x => new EfDataProviderManager(x.Resolve<DataSettings>())).As<BaseDataProviderManager>().InstancePerDependency();


            builder.RegisterType(SqlServerDataProvider).As<IDataProvider>().InstancePerDependency();

            if (dataProviderSettings != null && dataProviderSettings.IsValid())
            {
                var efDataProviderManager = new EfDataProviderManager(dataSettingsManager.LoadSettings());
                var dataProvider = efDataProviderManager.LoadDataProvider();
                dataProvider.InitConnectionFactory();

                builder.Register<IDbContext>(c => new NopObjectContext(dataProviderSettings.DataConnectionString)).InstancePerHttpRequest();
            }
            else
            {
                builder.Register<IDbContext>(c => new NopObjectContext(dataSettingsManager.LoadSettings().DataConnectionString)).InstancePerHttpRequest();
            }

            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerHttpRequest();


            ////builder.RegisterType<ContentService>().As<IContentService>().InstancePerHttpRequest();
            ////builder.RegisterType<NodeService>().As<INodeService>().InstancePerHttpRequest();
        }

        public int Order { get; private set; }
    }
}
