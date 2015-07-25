using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baike.Repository.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using Baike.Entity;

    public class DataSourceMap : EntityTypeConfiguration<DataSource>
    {
        /// <summary>
        /// DataSourceMap
        /// </summary>
        public DataSourceMap()
        {
            this.ToTable("DataSource");
            this.HasKey(c => c.Id);
            this.Property(c => c.ContentRegEx).IsRequired();
            this.Property(c => c.Description).IsRequired();
            this.Property(c => c.IntoUrl).IsRequired();
            this.Property(c => c.Kewords);
            this.Property(c => c.Logo);
            this.Property(c => c.WebsiteId);


            this.HasRequired(c => c.WebSiteInfo)
              .WithMany()
              .HasForeignKey(c => c.WebsiteId).WillCascadeOnDelete(true);

        }
    }
}