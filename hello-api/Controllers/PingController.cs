using hello_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace hello_api;

[ApiController]
[Route("[controller]")]
public class PingController : ControllerBase
{
	[HttpGet]
	public IActionResult Get()
	{
		return Ok(new
		{
			DateTimeUtc = DateTime.UtcNow,
			MachineName = Environment.MachineName,
			DevelopedBy = Environment.GetEnvironmentVariable("DevelopedBy") ?? "-",
			Environment = Environment.GetEnvironmentVariable("Environment") ?? "-",
			Secret = Environment.GetEnvironmentVariable("Secret") ?? "-",
			Version = "v1"
		});
	}

	[HttpPost]
	public IActionResult Post([FromBody] PingRequest request)
	{
		return Ok(new PingResponse
		{
			Message = request.Message ?? "Hello world!",
			MachineName = Environment.MachineName,
			Version = "v1"

		});
	}
}
