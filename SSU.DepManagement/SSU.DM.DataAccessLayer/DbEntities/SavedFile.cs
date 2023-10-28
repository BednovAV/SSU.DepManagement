using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SSU.DM.DataAccessLayer.DbEntities;

[EntityTypeConfiguration(typeof(SavedFileConfiguration))]
public class SavedFile
{
    public string Key { get; set; }
    
    public string FileName { get; set; }

    public byte[] Bytes { get; set; }
}

internal class SavedFileConfiguration : IEntityTypeConfiguration<SavedFile>
{
    public void Configure(EntityTypeBuilder<SavedFile> builder)
    {
        builder.HasKey(f => f.Key);
        builder.Property(f => f.Key).ValueGeneratedNever();
    }
}
