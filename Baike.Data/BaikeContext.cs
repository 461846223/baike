namespace Baike.Repository.Impl
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration.Conventions;

    using Baike.Entity;
    using Baike.Entity.DBModel;
    using Baike.Repository;
    using Baike.Repository.Mapping;

    /// <summary>
    /// CmsContent
    /// </summary>
    public class BaikeContext : DbContext
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        static BaikeContext()
        {         
            Database.SetInitializer<BaikeContext>(null);         
        }

        /// <summary>
        /// CmsContent();
        /// </summary>
        public BaikeContext()
            : base("ConnString")
        {
        }

        /// <summary>
        /// Gets or sets the channels.
        /// </summary>
        public DbSet<Content> Channels { get; set; }

        /// <summary>
        /// Gets or sets the web sites.
        /// </summary>
        public DbSet<WebSite> WebSites { get; set; }

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        public DbSet<Node> Nodes { get; set; }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        public DbSet<Attribute> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the attribute values.
        /// </summary>
        public DbSet<AttributeValue> AttributeValues { get; set; }

        /// <summary>
        /// Gets or sets the data url histories.
        /// </summary>
        public DbSet<DataUrlHistory> DataUrlHistories { get; set; }


        /// <summary>
        /// Gets or sets the MingxingHistories
        /// </summary>
        public DbSet<Mingxing> MingxingHistories { get; set; }

        /// <summary>
        /// Gets or sets the MingxingHistories
        /// </summary>
        public DbSet<KeywordImg> KeywordImgs { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="BaikeContext"/> class.
        /// </summary>
        /// <param name="nameOrConnectionString">
        /// The name or connection string.
        /// </param>
        public BaikeContext(string nameOrConnectionString)
            : base(EFTracing.GetConnection(nameOrConnectionString), true)
        {
            /*var ctx = ((IObjectContextAdapter)this).ObjectContext;
            //ctx.GetTracingConnection().CommandExecuting += (s, e) =>  
            //{  
            //    //跟踪SQL时打开
            //    //logger.Info(e.ToTraceString());
            //    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt"), e.ToTraceString()+"\r\n");
            //}; */
        }

        /// <summary>
        /// The on model creating.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //// 移除复数表名的契约
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //// 防止黑幕交易 要不然每次都要访问 EdmMetadata这个表
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            modelBuilder.Configurations.Add(new ContentMap());

        }
    }
}
