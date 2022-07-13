namespace WebApp.Admin.Models
{
    public class LockToAddDto
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsLocked { get; set; }
        public bool AllowUnlocking { get; set; }
    }
}
