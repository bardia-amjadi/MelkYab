using MelkYab.Backend.Data.DbContexts;
using MelkYab.Backend.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MelkYab.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET : api/bookings
        [HttpGet]
        public ActionResult Test()
        {
            return BadRequest("Test loaded");
        }

        [HttpGet("migrate")]
        [Route("/options/migrate")]
        public IActionResult MigrateDatabase()
        {
            int maxRetries = 5;
            int delaySeconds = 5;

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    using var scope = HttpContext.RequestServices.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.Migrate();
                    return Ok("Migration completed");
                }
                catch (Exception ex)
                {
                    if (i == maxRetries - 1)
                        return StatusCode(500, $"Migration failed after retries: {ex}");
                    System.Threading.Thread.Sleep(delaySeconds * 1000);
                }
            }
            return StatusCode(500, "Migration failed");
        }
    }
}
