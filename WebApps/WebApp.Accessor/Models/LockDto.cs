namespace WebApp.Accessor.Models
{
    public class LockDto
    {
        public string LockId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsLocked { get; set; }
        public bool AllowUnlocking { get; set; }
    }
}
