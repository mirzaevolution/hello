using hello_api.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace hello_api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
	private readonly IWebHostEnvironment _webHostEnvironment;

	public FilesController(IWebHostEnvironment webHostEnvironment)
	{
		_webHostEnvironment = webHostEnvironment;
	}
	[HttpGet]
	public IActionResult Get()
	{
		var result = QueryFiles();
		return Ok(result);
	}
	[HttpPost]
	public IActionResult Post([FromForm, Required] IList<IFormFile> files)
	{
		if (files != null && files.Count > 0)
		{
			var result = UploadFiles(files);
			return Ok(new UploadFileResponse
			{
				SuccessCounter = result.Success,
				ErrorCounter = result.Error
			});
		}
		return BadRequest(nameof(files));
	}

	private IEnumerable<string> QueryFiles()
	{
		string pathName = Environment.GetEnvironmentVariable("UploadFolderName") ?? "uploads";
		string fullPath = Path.Combine(_webHostEnvironment.ContentRootPath, pathName);

		List<string> files = new List<string>();
		if (Directory.Exists(fullPath))
		{
			foreach (string item in Directory.GetFiles(fullPath, "*", new EnumerationOptions
			{
				IgnoreInaccessible = true,
				RecurseSubdirectories = true,
			}))
			{
				files.Add(item);
			}
		}
		return files;
	}

	private (int Success, int Error) UploadFiles(IList<IFormFile> files)
	{
		int successCounter = 0, errorCounter = 0;
		string pathName = Environment.GetEnvironmentVariable("UploadFolderName") ?? "uploads";
		string fullPath = Path.Combine(_webHostEnvironment.ContentRootPath, pathName, DateTime.UtcNow.ToString("yyyy-MM-dd"));
		if (!Directory.Exists(fullPath))
		{
			Directory.CreateDirectory(fullPath);
		}
		foreach (var file in files)
		{
			try
			{
				string fileLocation = Path.Combine(fullPath, file.FileName);
				using Stream readStream = file.OpenReadStream();
				readStream.Seek(0, SeekOrigin.Begin);
				using FileStream writeStream =
					new FileStream(fileLocation, FileMode.Create, FileAccess.Write, FileShare.Write);
				readStream.CopyTo(writeStream);
				writeStream.Close();
				readStream.Close();
				successCounter++;
			}
			catch (Exception ex)
			{
				errorCounter++;
			}
		}
		return (successCounter, errorCounter);
	}
}