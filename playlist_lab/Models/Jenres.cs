using System;
using System.Collections.Generic;

namespace playlist_lab
{
    public partial class Jenres
    {
        public Jenres()
        {
            Tracks = new HashSet<Tracks>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Tracks> Tracks { get; set; }
    }
}
