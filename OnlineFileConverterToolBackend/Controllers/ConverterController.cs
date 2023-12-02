using System.ComponentModel.DataAnnotations;
using CloudConvert.API;
using CloudConvert.API.Models;
using CloudConvert.API.Models.ExportOperations;
using CloudConvert.API.Models.ImportOperations;
using CloudConvert.API.Models.JobModels;
using CloudConvert.API.Models.TaskOperations;
using Microsoft.AspNetCore.Mvc;
using OnlineFileConverterToolBackend;

namespace New_Folder.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class ConverterController : ControllerBase
{

    private readonly ILogger<ConverterController> _logger;
    private readonly CloudConvertAPI _cloudConvert;
    private const long MaxFileSize = 50 * 1024 * 1024; // 50 MB limit

    public ConverterController(ILogger<ConverterController> logger)
    {
        _logger = logger;
        //generate API key for cloud convert here: https://cloudconvert.com/dashboard/api/v2/keys
        _cloudConvert = new CloudConvertAPI(FileExtensions.key);
    }

    [HttpPost]
    [RequestSizeLimit(MaxFileSize)]
    public async Task<IActionResult> Upload(IFormFile file, [FromQuery] string from, [FromQuery] string to)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please enter all required fields");
            }
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { statusCode = 400, error = new { message = "inavlid file" } });
            }
            // check file size.
            if (file.Length > MaxFileSize)
            {
                return BadRequest(new { statusCode = 400, error = new { message = "File size exceeds the limit" } });
            }

            // check file extension.
            var fileExtension = Path.GetExtension(file.FileName).ToLower().TrimStart('.');
            if (!FileExtension.AllowedFileExtensions.Contains(fileExtension))
            {
                return BadRequest(new { statusCode = 400, error = new { message = "Invalid file extension" } });
            }

            if (fileExtension != from)
            {
                return BadRequest(new { statusCode = 400, error = new { message = "File extension does not match with the 'from' field." } });
            }
            if (!FileExtension.AllowedFileExtensions.Contains(to))
            {
                return BadRequest(new { statusCode = 400, error = new { message = "the 'to' field is invalid" } });
            }

            //get file name and byte array of file 
            byte[] fileBytes;
            using MemoryStream ms = new();
            file.CopyTo(ms);
            fileBytes = ms.ToArray();
            string fileName = file.FileName;
            _logger.LogInformation($"{DateTime.Now}:{fileName} was successfully uploaded");

            //create API job
            var job = await CreateJob(from, to);

            //get upload specific task
            var uploadTask = job.Data.Tasks.FirstOrDefault(t => t.Name == "upload_my_file");
            if (uploadTask is null) throw new NullReferenceException();

            //upload file as byte array 
            await _cloudConvert.UploadAsync(uploadTask.Result.Form.Url.ToString(), fileBytes, fileName, uploadTask.Result.Form.Parameters);

            //export the uploaded file
            var ExFile = await ExportFile(job);
            MyFile myFile = new() { Url = ExFile.Url, FileName = ExFile.Filename };

            return Ok(myFile);
        }
        catch (Exception ex)
        {
            var e = new { statusCode = 500, error = new { message = "something went wrong. could not process file" } };
            _logger.LogWarning(e.error.message);
            _logger.LogError(ex.Message);
            return StatusCode(500, e);
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
        try
        {
            var job2 = await _cloudConvert.WaitJobAsync(job.Data.Id);
            var exportTask = job2.Data.Tasks.FirstOrDefault() ?? throw new NullReferenceException();
            var file = exportTask.Result.Files.FirstOrDefault() ?? throw new NullReferenceException();
            return file;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
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
    /// The method creates a CloudConvert job with tasks for importing the file,
    /// converting it to the desired output format, and exporting the converted file.
    /// </remarks>
    private async Task<Response<JobResponse>> CreateJob(string Input_Format, string Output_Format)
    {
        try
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
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}

class MyFile
{
    public string FileName { get; set; }
    public Uri Url { get; set; }
}