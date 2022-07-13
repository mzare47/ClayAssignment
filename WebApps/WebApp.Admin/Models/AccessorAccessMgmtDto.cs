namespace WebApp.Admin.Models
{
    public class AccessorAccessMgmtDto
    {
        public AccessorDto Accessor { get; set; }
        public List<string> AllowedLockIds { get; set; }
    }
}
