using AccessControl.Api.Models.Shared;
using Shared.Lib.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessControl.Api.Models.Entity
{
    public class Access : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid AccessId { get; set; }
        public Guid LockId { get; set; }
        [ForeignKey("LockId")]
        public virtual Lock? Lock { get; set; }
        public string AccessorId { get; set; }
        [ForeignKey("AccessorId")]
        public virtual ApplicationUser? Accessor { get; set; }
        public LockStatus Accesstype { get; set; }
        public bool IsSuccessful { get; set; }
        public string? Reason { get; set; }
    }
}
