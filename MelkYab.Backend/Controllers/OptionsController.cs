using MelkYab.Backend.Data.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MelkYab.Backend.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/options")]
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

        private string ApiVersion => HttpContext.GetRequestedApiVersion()?.ToString() ?? "1";

        [HttpGet("tables")]
        public IActionResult GetTablesList()
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

                var response = new
                {
                    tables,
                    links = new[]
                    {
                        new { rel = "self", href = Url.Action(nameof(GetTablesList), new { version = ApiVersion }), method = "GET" },
                        new { rel = "last-migration", href = Url.Action(nameof(GetLastMigrationInfo), new { version = ApiVersion }), method = "GET" },
                        new { rel = "run-migration", href = Url.Action(nameof(RunMigration), new { version = ApiVersion }), method = "POST" },
                        new { rel = "test", href = Url.Action(nameof(Test), new { version = ApiVersion }), method = "GET" }
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error getting tables", message = ex.Message });
            }
        }

        [HttpGet("migration/last")]
        public IActionResult GetLastMigrationInfo()
        {
            try
            {
                var lastMigration = _context.Database
                    .GetAppliedMigrations()
                    .LastOrDefault();

                var response = new
                {
                    lastMigration,
                    checkedAt = DateTime.UtcNow,
                    links = new[]
                    {
                        new { rel = "self", href = Url.Action(nameof(GetLastMigrationInfo), new { version = ApiVersion }), method = "GET" },
                        new { rel = "run-migration", href = Url.Action(nameof(RunMigration), new { version = ApiVersion }), method = "POST" },
                        new { rel = "tables", href = Url.Action(nameof(GetTablesList), new { version = ApiVersion }), method = "GET" }
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Error getting last migration", message = ex.Message });
            }
        }

        [HttpPost("migration/run")]
        public IActionResult RunMigration()
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

                    var response = new
                    {
                        message = "✅ Migration completed successfully.",
                        links = new[]
                        {
                            new { rel = "last-migration", href = Url.Action(nameof(GetLastMigrationInfo), new { version = ApiVersion }), method = "GET" },
                            new { rel = "tables", href = Url.Action(nameof(GetTablesList), new { version = ApiVersion }), method = "GET" }
                        }
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    if (i == maxRetries - 1)
                    {
                        return StatusCode(500, new
                        {
                            error = "❌ Migration failed after maximum retries.",
                            exception = ex.Message
                        });
                    }

                    Thread.Sleep(delaySeconds * 1000);
                }
            }

            return StatusCode(500, new { error = "❌ Migration failed unexpectedly." });
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new
            {
                status = "🧪 OptionsController is running correctly.",
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(Test), new { version = ApiVersion }), method = "GET" },
                    new { rel = "tables", href = Url.Action(nameof(GetTablesList), new { version = ApiVersion }), method = "GET" },
                    new { rel = "last-migration", href = Url.Action(nameof(GetLastMigrationInfo), new { version = ApiVersion }), method = "GET" }
                }
            });
        }
    }
}
