using Microsoft.AspNetCore.Identity;

namespace ResumeAutoCheckker.Domain.Entities.Auth
{
    public class AppHr : IdentityUser<long>
    {
        public string Login { get; set; }
    }
}
