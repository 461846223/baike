using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baike.Repository.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using Baike.Entity.DBModel;

    public  class MingxingMap: EntityTypeConfiguration<Mingxing>
    {
        /// <summary>
        /// BlockMap
        /// </summary>
        public MingxingMap()
        {

            //// Primary Key
            this.HasKey(t => t.Id);

            //// Properties
            this.Property(t => t.Name).HasMaxLength(50);

            //// Table & Column Mappings
            this.ToTable("Mingxing");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
