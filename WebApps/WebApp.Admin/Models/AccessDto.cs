using Shared.Lib.Models;

namespace WebApp.Admin.Models
{
    public class AccessDto
    {
        public string AccessId { get; set; }
        public string LockId { get; set; }
        public LockDto? Lock { get; set; }
        public string AccessorId { get; set; }
        public AccessorDto? Accessor { get; set; }
        public LockStatus Accesstype { get; set; }
        public bool IsSuccessful { get; set; }
        public string? Reason { get; set; }
    }
}
