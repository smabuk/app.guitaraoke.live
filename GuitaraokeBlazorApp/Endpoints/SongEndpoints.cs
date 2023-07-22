namespace GuitaraokeBlazorApp.Endpoints;

public static partial class SongEndpoints {

	public static void MapSongEndpoints(this WebApplication? app) {
		app?.MapPost("/star/{slug}", ToggleSongStar);
		app?.MapPost("/song/{slug}", SignUpForSong);
	}

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
	public static async Task<IResult> ToggleSongStar(string slug, ISongDatabase db, IUserTracker tracker) {
		var song = db.FindSong(slug);
		if (song == default) return Results.NotFound();
		var user = tracker.GetUser();
		return Results.Ok(db.ToggleStar(user, song));
	}

	public static async Task<IResult> SignUpForSong(string slug, string name, Instrument[] instruments, ISongDatabase db, IUserTracker tracker) {
		var song = db.FindSong(slug);
		if (song == default) return Results.NotFound();
		var user = tracker.GetUser();
		if (user.SignUp(song, instruments)) {
			if (!String.IsNullOrEmpty(name)) user.Name = name;
			db.AddSongToQueue(song);
		} else {
			db.PruneQueue();
		}
		return Results.Ok();
	}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

}
