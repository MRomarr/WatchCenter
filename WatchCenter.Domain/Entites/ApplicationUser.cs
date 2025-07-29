using Microsoft.AspNetCore.Identity;

namespace WatchCenter.Domain.Entites
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<RefreshToken>? refreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<Content> Contents { get; set; } = new List<Content>();

    }

}
