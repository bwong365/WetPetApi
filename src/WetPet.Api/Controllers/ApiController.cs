using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WetPet.AppCore.Common.Errors;

namespace WetPet.Api.Controllers;

[ApiController]
[ProducesResponseType(typeof(ValidationProblemDetails), 400)]
public abstract class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        var firstError = errors.First();
        return Problem(errors, firstError);
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        var modelState = new ModelStateDictionary();
        foreach (var error in errors)
        {
            modelState.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem(modelState);
    }

    private IActionResult Problem(List<Error> errors, Error error)
    {
        var statusCode = error.NumericType switch
        {
            ((int) ErrorType.Validation) => StatusCodes.Status400BadRequest,
            ((int) ErrorType.Conflict) => StatusCodes.Status409Conflict,
            ((int) ErrorType.NotFound) => StatusCodes.Status404NotFound,
            (int) CustomErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails()
        {
            Status = statusCode,
            Title = error.Description,
            Instance = Request.Path,
        };
        problemDetails.Extensions["errorCodes"] = errors.Select(e => e.Code);
        return new ObjectResult(problemDetails);
    }
}