using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _432Supinfy.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }

        public ICollection<PlaylistItem> Items { get; set; }
    }

    public class PlaylistItem
    {
        public int Id { get; set; }
        public string TrackId { get; set; }

        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
