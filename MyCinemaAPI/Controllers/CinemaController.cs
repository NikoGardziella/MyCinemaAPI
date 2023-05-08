using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCinemaAPI.Models;
using MyCinemaAPI.Data;
using System.Globalization;

namespace MyCinemaAPI.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class CinemaController : ControllerBase
	{
		

		private readonly ApiContext _context;
		private CinemaController _cinemaController { get; set; } = null!;

		public CinemaController(ApiContext context)
		{
			_context = context;
		}


		//CREATE NEW

		[HttpPost]
		public JsonResult Post(string name, int openingHour, int ClosingHour, int ShowDuration)
		{
			Cinema cinema = new Cinema();
			cinema.Name = name;
			cinema.OpeningHour = openingHour;
			cinema.ClosingHour = ClosingHour;
			cinema.ShowDuration = ShowDuration;
			_context.Cinemas.Add(cinema);		
			_context.SaveChanges();

			return new JsonResult(Ok(cinema));
		}


		// UPDATE EXISTING
		[HttpPut]
		public JsonResult Put(Cinema cinema)
		{
			if(cinema.Id == 0)
			{
				_context.Cinemas.Add(cinema);
			}
			else
			{
				var cinemaInDB = _context.Cinemas.Find(cinema.Id);
				if(cinemaInDB == null)
				{
					return new JsonResult(NotFound());
				}

				cinemaInDB.Name = cinema.Name;
				cinemaInDB.OpeningHour = cinema.OpeningHour;
				cinemaInDB.ClosingHour	= cinema.ClosingHour;
				cinemaInDB.ShowDuration = cinema.ShowDuration;
			}
			_context.SaveChanges();

			return new JsonResult(Ok(cinema));
		}

		//GET
		[HttpGet]
		public JsonResult Get(int id)
		{
			var result = _context.Cinemas.Find(id);
			if(result == null)
				return new JsonResult(NotFound());
			return new JsonResult(Ok(result));
		}

		//DELETE
		[HttpDelete]
		public bool Delete(int id)
		{
			bool success = false;
			var result = _context.Cinemas.Find(id);
			if(result == null)
				return success;
			_context.Cinemas.Remove(result);
			_context.SaveChanges();
			success = true;
			return success;
		}

		// GET ALL

		[HttpGet()]
		public JsonResult GetAll()
		{
			var result = _context.Cinemas.ToList();
			return new JsonResult(Ok(result));
		}

		//GET SHOWTIMES
		[HttpGet]
		public JsonResult GetShowtimes(int openingHour, int closingHour, int duration)
		{
			if(duration < 0 || openingHour < 0 || closingHour < 0)
				return new JsonResult(NotFound());
			if(openingHour > 24 || closingHour > 24)
				return new JsonResult(NotFound());
			TimeSpan breakBetweenShows = TimeSpan.FromMinutes(15);
			DateTime closingTime;
			if (closingHour <= 3)
				closingTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, closingHour, 0, 0);
			else
				closingTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, closingHour, 0, 0);

			DateTime currentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, openingHour, 0, 0);
			TimeSpan movieDuration = TimeSpan.FromMinutes(duration);

			List<string> showtimes = new List<string>();
			while (currentTime + movieDuration <= closingTime)
			{
				showtimes.Add(currentTime.ToString("HH:mm"));
				currentTime += movieDuration + breakBetweenShows;
				
			}

			return new JsonResult (Ok(showtimes));

		}

	}
}
