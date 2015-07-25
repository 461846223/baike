using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baike.Repository.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using Baike.Entity.DBModel;

    public class KeywordImgMap : EntityTypeConfiguration<KeywordImg>
    {
        /// <summary>
        /// BlockMap
        /// </summary>
        public KeywordImgMap()
        {

            //// Primary Key
            this.HasKey(t => t.Id);

            //// Properties
            this.Property(t => t.Keyword).HasMaxLength(50);

            //// Table & Column Mappings
            this.ToTable("KeywordImg");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Keyword).HasColumnName("Keyword");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.ImgUrl).HasColumnName("ImgUrl");
            this.Property(t => t.SiteId).HasColumnName("SiteId");
            this.Property(t => t.Width).HasColumnName("Width");
            this.Property(t => t.Height).HasColumnName("Height");
        }
    }
}
