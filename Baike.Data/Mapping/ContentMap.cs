namespace Baike.Repository.Mapping
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    using Baike.Entity;

    /// <summary>
    /// BlockMap
    /// </summary>
    public class ContentMap : EntityTypeConfiguration<Content>
    {
        /// <summary>
        /// BlockMap
        /// </summary>
        public ContentMap()
        {
            //// Primary Key
            this.HasKey(t => t.Id);

            //// Properties
            this.Property(t => t.Summary).HasMaxLength(500);

            this.Property(t => t.Title).IsRequired().HasMaxLength(100);

            this.Property(t => t.ImageUrl).HasMaxLength(100);
            this.Property(t => t.ImageUrlse).HasMaxLength(100);

            this.Property(t => t.SEOdescription).HasMaxLength(400);

            //// Table & Column Mappings
            this.ToTable("Content");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ImageUrl).HasColumnName("ImageUrl");
            this.Property(t => t.SEOdescription).HasColumnName("SEOdescription");
            this.Property(t => t.SEOkeywords).HasColumnName("SEOkeywords");
            this.Property(t => t.SourceID).HasColumnName("SourceID");
            this.Property(t => t.SourceName).HasColumnName("SourceName");
            this.Property(t => t.SourceUrl).HasColumnName("SourceUrl");
            this.Property(t => t.Summary).HasColumnName("Summary");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Subtitle).HasColumnName("Subtitle");
            this.Property(t => t.ContentText).HasColumnName("ContentText");
            this.Property(t => t.PreviousId).HasColumnName("PreviousId");
            this.Property(t => t.IsHot).HasColumnName("IsHot");
            this.Property(t => t.IsRecommend).HasColumnName("IsRecommend");
            this.Property(t => t.ImageUrlse).HasColumnName("ImageUrlse");
            
            
            this.HasRequired(c => c.NodeInfo)
                .WithMany(t => t.Contents)
                .HasForeignKey(c => c.NodeId)
                .WillCascadeOnDelete(true);
        }
    }
}
