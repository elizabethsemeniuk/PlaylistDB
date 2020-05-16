using System;
using System.Collections.Generic;

namespace playlist_lab
{
    public partial class Artists
    {
        public Artists()
        {
            ArtistsTracks = new HashSet<ArtistsTracks>();
        }

        public int Id { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
        public string Fullname { get; set; }

        public virtual ICollection<ArtistsTracks> ArtistsTracks { get; set; }
    }
}
