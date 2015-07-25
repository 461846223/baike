using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baike.Repository.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using Baike.Entity;

    public class WebSiteMap: EntityTypeConfiguration<WebSite>
    {
        /// <summary>
        /// BlockMap
        /// </summary>
        public WebSiteMap()
        {
            //// Primary Key
            this.HasKey(t => t.Id);


            //// Table & Column Mappings
            this.ToTable("WebSite");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Keywords).HasColumnName("Keywords");
            this.Property(t => t.PhysicalPath).HasColumnName("PhysicalPath");
            this.Property(t => t.SiteName).HasColumnName("SiteName");

            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.DomainName).HasColumnName("DomainName");
        }
    }
}