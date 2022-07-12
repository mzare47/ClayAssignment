using AccessControl.Api.Models.Shared;
using Microsoft.AspNetCore.Identity;

namespace AccessControl.Api.Models.Entity
{
    public class ApplicationUser : IdentityUser, IAuditable
    {
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
