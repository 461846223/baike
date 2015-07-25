namespace Baike.Repository
{
    using System;
    using System.Configuration;
    using System.Data.Common;
    using System.Linq;
    using EFProviderWrapperToolkit;
    using EFTracingProvider;

    /// <summary>
    /// EF跟踪SQL方法
    /// </summary>
    public class EFTracing
    {
        /// <summary>
        /// 或者链接字符串
        /// </summary>
        /// <param name="nameOrConnectionString">nameOrConnectionString</param>
        /// <returns>wrapperConnectionString</returns>
        public static DbConnection GetConnection(string nameOrConnectionString)
        {
            try
            {
                return EntityConnectionWrapperUtils.CreateEntityConnectionWithWrappers(nameOrConnectionString, "EFTracingProvider", "EFCachingProvider");
            }
            catch (ArgumentException)
            {
                if (nameOrConnectionString.Contains('='))
                {
                    nameOrConnectionString = nameOrConnectionString.Substring(nameOrConnectionString.IndexOf('=') + 1);
                }
                //// an invalid entity connection string is assumed to be a normal connection string name or connection string (Code First)  
                ConnectionStringSettings connectionStringSetting =
                    ConfigurationManager.ConnectionStrings[nameOrConnectionString];
                string connectionString;
                string providerName;

                if (connectionStringSetting != null)
                {
                    connectionString = connectionStringSetting.ConnectionString;
                    providerName = connectionStringSetting.ProviderName;
                }
                else
                {
                    providerName = "System.Data.SqlClient";
                    connectionString = nameOrConnectionString;
                }

                return CreateTracingConnection(connectionString, providerName);
            }
        }

        /// <summary>
        /// 封装成DbConnection
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="providerInvariantName">wappername</param>
        /// <returns>wrapperConnectionString</returns>
        private static EFTracingConnection CreateTracingConnection(
            string connectionString,
            string providerInvariantName)
        {
            string wrapperConnectionString = string.Format(
                @"wrappedProvider={0};{1}",
                providerInvariantName,
                connectionString);
            var connection = new EFTracingConnection { ConnectionString = wrapperConnectionString };
            return connection;
        }
    }
}
