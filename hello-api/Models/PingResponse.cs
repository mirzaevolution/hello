namespace hello_api.Models;
public class PingResponse
{
	public DateTime DateTimeUtc { get; private set; } = DateTime.UtcNow;
	public string Message { get; set; }
	public string MachineName { get; set; }

	public string Version { get; set; }
}
