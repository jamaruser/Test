namespace NotinoHomework.Controllers;

using System.ComponentModel.DataAnnotations;
using Abstraction;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Models;

[ApiController]
[Route("[controller]/[action]")]
public class FileConverterController : ControllerBase
{
    private readonly IFileProcessor _fileProcessor;

    public FileConverterController(IFileProcessor fileProcessor)
    {
        _fileProcessor = fileProcessor;
    }

    [HttpPost]
    [Route("{target}")]
    public async Task<IActionResult> Convert([FromForm] [Required] IFormFile file, [FromForm] MailRequest mailRequest, [Required] [FromRoute] string target)
    {
        var convertResult = await _fileProcessor.ProcessIncomingFile(file, mailRequest, target);
        return File(convertResult.ConvertedFile, Constants.OctetStreamContentType, convertResult.FileName);
    }
    
    [HttpGet]
    public async Task<IActionResult> ConvertDownloadedFile([FromQuery] ConvertRequest request)
    {
        var convertResult = await _fileProcessor.ProcessFileFromUrl(request.FileName, request.Target);
        return File(convertResult.ConvertedFile, Constants.OctetStreamContentType, convertResult.FileName);
    }
    
    [HttpPost]
    public async Task<IActionResult> BlobStorage([FromBody] ConvertRequest request)
    {
        await _fileProcessor.ProcessFileFromBlobStorage(request.FileName, request.Target);
        return new OkResult();
    }
}