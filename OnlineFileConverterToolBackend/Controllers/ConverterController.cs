using CloudConvert.API;
using CloudConvert.API.Models;
using CloudConvert.API.Models.ExportOperations;
using CloudConvert.API.Models.ImportOperations;
using CloudConvert.API.Models.JobModels;
using CloudConvert.API.Models.TaskOperations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using OnlineFileConverterToolBackend;

namespace New_Folder.Controllers;

[ApiController]
[Route("[controller]")]
public class ConverterController : ControllerBase
{

    private readonly ILogger<ConverterController> _logger;
    private readonly CloudConvertAPI _cloudConvert;
    public ConverterController(ILogger<ConverterController> logger)
    {
        _logger = logger;
        _cloudConvert = new CloudConvertAPI(KeyToken.key);
    }

    [HttpPost(Name = "PostWeatherForecast")]
    public async Task<ActionResult> PostFile()
    {
        // Check if the request contains multipart/form-data.
        // if (!Request.ContentType.StartsWith("multipart/form-data", StringComparison.OrdinalIgnoreCase))
        // {
        //     return BadRequest("Media type not supported");
        // }

        try
        {
            // var formCollection = await Request.ReadFormAsync();

            // // Access form data.
            // foreach (var (key, value) in formCollection)
            // {
            //     // You can handle additional form data here.
            //     Console.WriteLine($"{key}: {value}");
            // }

            // // Access file data.
            // var file = formCollection.Files.FirstOrDefault();

            // if (file == null)
            // {
            //     return BadRequest("No file received");
            // }

            // // Basic security measure: Check file size.
            // if (file.Length > 1024 * 1024) // 1 MB limit
            // {
            //     return BadRequest("File size exceeds the limit");
            // }

            // // Basic security measure: Check file extension.
            // var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            // var fileExtension = Path.GetExtension(file.FileName).ToLower();
            // if (!allowedExtensions.Contains(fileExtension))
            // {
            //     return BadRequest("Invalid file extension");
            // }

            // Process the file (save, database entry, etc.).
            // For demonstration purposes, let's just return the file name.
            _logger.LogInformation("get");
            string filePath = "data-entry.pdf";
            var job = await CreateJob("pdf", "docx");

            var uploadTask = job.Data.Tasks.FirstOrDefault(t => t.Name == "upload_my_file");
            using Stream stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (uploadTask is null) throw new NullReferenceException();
            await _cloudConvert.UploadAsync(uploadTask.Result.Form.Url.ToString(), stream, filePath, uploadTask.Result.Form.Parameters);

            var ExFile = await ExportFile(job);
            Console.WriteLine($"Url: {ExFile.Url}\nName:{ExFile.Filename}");
            MyFile myFile = new() { Url = ExFile.Url, FileName = ExFile.Filename };

            // return Ok(file.FileName);
            return Ok(myFile);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

    }

    /// <summary>
    /// Asynchronously exports a file from the CloudConvert job result.
    /// </summary>
    /// <param name="job">The CloudConvert job response containing information about the exported file.</param>
    /// <returns>The exported file information as a <see cref="CloudConvert.API.Models.TaskModels.TaskResultFile"/>.</returns>
    /// <exception cref="NullReferenceException">Thrown if the necessary information is not found in the CloudConvert job response.</exception>
    private async Task<CloudConvert.API.Models.TaskModels.TaskResultFile> ExportFile(Response<JobResponse> job)
    {
        var job2 = await _cloudConvert.WaitJobAsync(job.Data.Id);
        var exportTask = job2.Data.Tasks.FirstOrDefault() ?? throw new NullReferenceException();
        var file = exportTask.Result.Files.FirstOrDefault() ?? throw new NullReferenceException();
        return file;
    }

    /// <summary>
    /// Asynchronously creates a CloudConvert job for converting a file.
    /// </summary>
    /// <param name="url">The URL of the file to be converted.</param>
    /// <param name="input_Format">The input format of the file to be converted.</param>
    /// <param name="output_Format">The desired output format of the converted file.</param>
    /// <returns>
    /// A <see cref="Response{T}"/> containing information about the CloudConvert job response,
    /// where T is <see cref="JobResponse"/>.
    /// </returns>
    /// <remarks>
    /// The method creates a CloudConvert job with tasks for importing the file from the specified URL,
    /// converting it to the desired output format, and exporting the converted file.
    /// </remarks>
    private async Task<Response<JobResponse>> CreateJob(string Input_Format, string Output_Format)
    {
        return await _cloudConvert.CreateJobAsync(new JobCreateRequest
        {
            Tasks = new
            {
                upload_my_file = new ImportUploadCreateRequest(),
                convert = new ConvertCreateRequest
                {
                    Input = "upload_my_file",
                    Input_Format = Input_Format,
                    Output_Format = Output_Format
                },
                export = new ExportUrlCreateRequest
                {
                    Input = "convert",
                    Archive_Multiple_Files = true
                }
            },
            Tag = "Test"
        });
    }
}

class MyFile
{
    public string FileName { get; set; }
    public Uri Url { get; set; }
}

