using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baike.Repository.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using Baike.Entity.DBModel;

    public class DataUrlHistoryMap : EntityTypeConfiguration<DataUrlHistory>
    {
        /// <summary>
        /// DataSourceMap
        /// </summary>
        public DataUrlHistoryMap()
        {
            this.ToTable("DataUrlHistory");
            this.HasKey(c => c.Id);
            this.Property(c => c.Url).IsRequired();
            this.Property(c => c.InserTime).IsRequired();
        }
    }
}