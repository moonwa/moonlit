namespace Joinme.Models
{
    public class Tenant : IEntityObject
    {
        public int TenantId { get; set; }
        public string AppName { get; set; }
        public string RedirectUrl { get; set; }
    }
}