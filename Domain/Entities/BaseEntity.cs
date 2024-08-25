using System.ComponentModel.DataAnnotations;

namespace Domain;

public abstract class BaseEntity
{
    public DateTimeOffset? DateCreated { get; set; }
    [MaxLength(50)]
    public string? CreatedBy { get; set; } = string.Empty;
    public DateTimeOffset? DateUpdated { get; set; }
    [MaxLength(50)]
    public string? UpdatedBy { get; set; } = string.Empty;
    public EntityStatus EntityStatus { get; set; }

    public void SetDataCreate()
    {
        DateCreated = DateTimeOffset.Now;
        ActivateEntity();
    }

    public void SetDataUpdate()
    {
        DateUpdated = DateTimeOffset.Now;
    }

    public void ArchiveEntity()
    {
        EntityStatus = EntityStatus.Archived;
    }

    public void DeleteEntity()
    {
        EntityStatus = EntityStatus.Deleted;
    }

    public void ActivateEntity()
    {
        EntityStatus = EntityStatus.Active;
    }

}

public enum EntityStatus
{
    Active,
    Archived,
    Deleted
}
