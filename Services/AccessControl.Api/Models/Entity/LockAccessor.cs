using AccessControl.Api.Models.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessControl.Api.Models.Entity
{
    public class LockAccessor : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid LockAccessorId { get; set; }
        public Guid LockId { get; set; }
        [ForeignKey("LockId")]
        public virtual Lock? Lock { get; set; }
        public string AccessorId { get; set; }
        [ForeignKey("AccessorId")]
        public virtual ApplicationUser? Accessor { get; set; }
    }
}
