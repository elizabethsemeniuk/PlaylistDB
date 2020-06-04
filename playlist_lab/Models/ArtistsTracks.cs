using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace playlist_lab
{
    public partial class ArtistsTracks
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
      //  [Display(Name = "Artists")]
        public int ArtistId { get; set; }

        [Display(Name = "Artists")]
        public virtual Artists Artist { get; set; }
        public virtual Tracks Track { get; set; }
    }
}
