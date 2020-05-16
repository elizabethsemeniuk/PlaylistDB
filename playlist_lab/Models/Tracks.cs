using System;
using System.Collections.Generic;

namespace playlist_lab
{
    public partial class Tracks
    {
        public Tracks()
        {
            ArtistsTracks = new HashSet<ArtistsTracks>();
        }

        public int Id { get; set; }
        public int JenreId { get; set; }
        public int AlbumId { get; set; }
        public string Name { get; set; }

        public virtual Album Album { get; set; }
        public virtual Jenres Jenre { get; set; }
        public virtual ICollection<ArtistsTracks> ArtistsTracks { get; set; }
    }
}
