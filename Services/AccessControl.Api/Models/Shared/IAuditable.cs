namespace AccessControl.Api.Models.Shared
{
    public interface IAuditable
    {
        DateTime? CreatedDate { get; set; }
        string? CreatedBy { get; set; }
        string? LastModifiedBy { get; set; }
        DateTime? LastModifiedDate { get; set; }
    }
}
