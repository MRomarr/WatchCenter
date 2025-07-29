
using System.ComponentModel.DataAnnotations;

namespace WatchCenter.Domain.Entites
{
    public class Content : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterUrl { get; set; }
        public string TrailerUrl { get; set; }
        public ContentType Type { get; set; } // 0 for Movie, 1 for Series
        
        
        public string GenreId { get; set; }
        public Genre genre { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public Movie? Movie { get; set; }
        public Series? Series { get; set; } 

    }
    public enum ContentType : byte
    {
        
        Movie = 0,
        Series = 1
    }
}
