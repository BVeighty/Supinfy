using _432Supinfy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _432Supinfy.ViewModels
{
    public class PlaylistsViewModel
    {
        public List<Playlist> Playlists { get; set; }

        public PlaylistsViewModel()
        {
            Playlists = new List<Playlist>();
        }
    }
}
