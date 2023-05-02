using Microsoft.EntityFrameworkCore;
using MyCinemaAPI.Models;

namespace MyCinemaAPI.Data
{
	public class ApiContext : DbContext
	{
		public DbSet<Cinema> Cinemas { get; set; }
		public ApiContext(DbContextOptions<ApiContext> options)
				:base(options)
		{

		}
	}
}
