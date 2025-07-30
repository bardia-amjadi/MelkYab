using MelkYab.Backend.Data.DbContexts;
using MelkYab.Backend.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MelkYab.Backend.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    public class OptionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public OptionsController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Options/Tables
        [HttpGet("Tables")]
        public IActionResult TablesLists()
        {
            try
            {
                var connection = _context.Database.GetDbConnection();
                connection.Open();

                var tables = new List<string>();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader.GetString(0));
                        }
                    }
                }

                return Ok(tables);
            }
            catch (Exception ex)
            {
                return BadRequest($"❌ Error getting tables: {ex.Message}");
            }
        }

        // GET: api/Options/Last
        [HttpGet("Last")]
        public IActionResult LastMigrateInformation()
        {
            try
            {
                var lastMigration = _context.Database
                    .GetAppliedMigrations()
                    .LastOrDefault();

                if (lastMigration == null)
                    return Ok("📭 No migrations applied yet.");

                return Ok(new
                {
                    LastMigration = lastMigration,
                    CheckedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"❌ Error getting last migration: {ex.Message}");
            }
        }

        // GET: api/Options/Test
        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok("🧪 OptionsController is running correctly.");
        }


        [HttpGet("Migrate")]
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
                    Thread.Sleep(delaySeconds * 1000);
                }
            }
            return StatusCode(500, "Migration failed");
        }
    }
}
