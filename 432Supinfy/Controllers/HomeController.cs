using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _432Supinfy.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using _432Supinfy.Data;
using _432Supinfy.ViewModels;

namespace _432Supinfy.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext _context { get; }

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Playlists()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetPlaylists()
        {
            var model = new PlaylistsViewModel();

            model.Playlists = _context.Playlists.ToList();

            return Json(model.Playlists);
        }

        [HttpGet]
        public string CreatePlaylist(string name)
        {
            var playlist = new Playlist
            {
                Name = name
            };

            _context.Playlists.Add(playlist);
            _context.SaveChanges();

            return "done";
        }

        [HttpGet]
        public JsonResult GetPlaylistContent(int id)
        {
            return Json(_context.Playlists.FirstOrDefault(p => p.Id == id));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public JsonResult GetTrackById(string id)
        {
            var resultStr = GetTrackByIdAsync(id).Result;
            var result = JObject.Parse(resultStr);
            var resultList = result["results"].Children().ToList();

            var track = resultList[0].ToObject<Track>();

            return Json(track);
        }

        [HttpGet]
        public JsonResult SearchTracks(string search)
        {
            var resultStr = SearchTracksAsync(search).Result;
            var result = JObject.Parse(resultStr);
            var resultList = result["results"].Children().ToList();

            var tracksList = new List<Track>();
            foreach (var item in resultList)
            {
                var track = item.ToObject<Track>();
                tracksList.Add(track);
            }

            return Json(tracksList);
        }

        [NonAction]
        public async Task<string> GetTrackByIdAsync(string id)
        {
            string result;

            using (var httpClient = new HttpClient())
            {
                var test = await httpClient.GetAsync($"https://api.jamendo.com/v3.0/tracks/?client_id=0ff2e3f8&id={id}&format=jsonpretty");

                result = await test.Content.ReadAsStringAsync();
            }

            return result;
        }

        [NonAction]
        public async Task<string> SearchTracksAsync(string search)
        {
            string result;

            using (var httpClient = new HttpClient())
            {
                var test = await httpClient.GetAsync($"https://api.jamendo.com/v3.0/tracks/?client_id=0ff2e3f8&search={search}&format=jsonpretty");

                result = await test.Content.ReadAsStringAsync();
            }

            return result;
        }
    }
}
