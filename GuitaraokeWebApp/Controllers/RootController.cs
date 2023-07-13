using GuitaraokeWebApp.Data;
using GuitaraokeWebApp.Model;
using Microsoft.AspNetCore.Mvc;

namespace GuitaraokeWebApp.Controllers;

public class RootController : Controller {
	private readonly ILogger<RootController> logger;
	private readonly ISongDatabase db;
	private readonly IUserTracker tracker;

	public RootController(ILogger<RootController> logger, ISongDatabase db, IUserTracker tracker) {
		this.logger = logger;
		this.db = db;
		this.tracker = tracker;
	}

	public async Task<IActionResult> Index() {
		var userGuid = tracker.GetUserGuid();
		var stars = db.ListStarredSongs(userGuid);
		var selection = db.ListSongs()
			.Select(song => new SongSelection(song) { IsStarred = stars.Contains(song) });
		return View(selection);
	}

	[HttpPost]
	public async Task<IActionResult> Star(string id) {
		var userGuid = tracker.GetUserGuid();
		var song = db.FindSong(id);
		return Json(db.ToggleStar(userGuid, song));
	}

	public async Task<IActionResult> Song(string id) {
		var userGuid = tracker.GetUserGuid();
		var song = db.FindSong(id);
		var stars = db.ListStarredSongs(userGuid);
		var model = new SongSelection(song) { IsStarred = stars.Contains(song) };
		return View(model);
	}

	public async Task<IActionResult> Queue() {
		var songQueue = new SongQueue {
			StarredSongs = db.ListStarredSongs()
				.ToDictionary(pair => pair.Key, pair => pair.Value.Count)
		};
		return View(songQueue);
	}
}

