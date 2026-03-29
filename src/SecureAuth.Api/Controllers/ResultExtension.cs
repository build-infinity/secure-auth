using Microsoft.AspNetCore.Mvc;
using SecureAuth.Application.Common;

namespace SecureAuth.Api
{
    public static class ResultExtension
    {
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            return result.Match<IActionResult>(
                success => new OkObjectResult(success),
                error => error.Type switch
                {
                    ErrorType.Validation => new BadRequestObjectResult(error),
                    ErrorType.Conflict => new ConflictObjectResult(error),
                    ErrorType.Forbidden => new ObjectResult(error) { StatusCode = StatusCodes.Status403Forbidden },
                    ErrorType.Unauthorized => new UnauthorizedObjectResult(error),
                    _ => new BadRequestObjectResult(error)
                }
            );
        }
    }
}