using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchCenter.Application.DTOs
{
    public class UpdateContentDto
    {
        [Required]
        public string ContentId { get; set; }
        [Required]
        public string GenreId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string PosterUrl { get; set; }
        public string TrailerUrl { get; set; }
    }
}
