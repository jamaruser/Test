namespace NotinoHomework.Controllers;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public class ErrorController : ControllerBase
{
    [Route("/error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult HandleErrorDevelopment(
        [FromServices] IHostEnvironment hostEnvironment)
    {
        var exceptionHandlerFeature =
            HttpContext.Features.Get<IExceptionHandlerFeature>()!;

        var problem = Problem(
            hostEnvironment.IsDevelopment() ? exceptionHandlerFeature.Error.StackTrace : null,
            title: exceptionHandlerFeature.Error.Message);
        return problem;
    }
}