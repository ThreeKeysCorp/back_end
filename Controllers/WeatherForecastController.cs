using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace DevOpsBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private const string ConnectionString = "Server=localhost;Port=5432;Database=PostgresTestDb;User Id=postgres;Password=12453265";

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();
                    var command = new NpgsqlCommand("SELECT * FROM users", connection);
                    var reader = command.ExecuteReader();

                    var data = new List<User>();

                    while (reader.Read())
                    {
                        var user = new User
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2)
                        };

                        data.Add(user);
                    }

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}