using AccessControl.Api.Models.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessControl.Api.Models.Entity
{
    public class Lock : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid LockId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsLocked { get; set; }
        public bool AllowUnlocking { get; set; }
    }
}
