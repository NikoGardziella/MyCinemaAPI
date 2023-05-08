using MyCinemaAPI.Controllers;
using MyCinemaAPI.Data;

namespace CineAPI.NUnitTests
{
	public class CinemaControllerTests
	{
		private ApiContext context;

		private CinemaController _cinemaController { get; set; } = null!;

		[SetUp]
		public void Setup()
		{
			
			_cinemaController = new CinemaController(context);
		}

		[TestCase(12,18,60)]
		[TestCase(12, 18, -1)]
		[TestCase(-1, 18, 60)]
		[TestCase(12, -1, 60)]
		[TestCase(12, 0, 60)]
		[TestCase(0, 18, 60)]

		public void GetShowtimesTests(int openingHour, int closingHour,int duration)
		{

			//act
			var result = _cinemaController.GetShowtimes(openingHour, closingHour, duration);

			//assets
			Assert.IsNotNull(result);
		}
	}
	
}