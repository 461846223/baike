using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baike.Repository.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using Baike.Entity;

    public class NodeMap : EntityTypeConfiguration<Node>
    {
        public NodeMap()
        {
            //// Primary Key
            this.HasKey(t => t.Id);
            this.ToTable("Node");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SiteId).HasColumnName("SiteId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Keywords).HasColumnName("Keywords");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.Title).HasColumnName("Title");

        }
    }
}
