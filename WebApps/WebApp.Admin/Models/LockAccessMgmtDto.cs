namespace WebApp.Admin.Models
{
    public class LockAccessMgmtDto
    {
        public LockDto Lock { get; set; }
        public List<string> AllowedAccessorIds { get; set; }
    }
}
