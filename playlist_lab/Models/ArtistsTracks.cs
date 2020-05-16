using System;
using System.Collections.Generic;

namespace playlist_lab
{
    public partial class ArtistsTracks
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public int ArtistId { get; set; }

        public virtual Artists Artist { get; set; }
        public virtual Tracks Track { get; set; }
    }
}
