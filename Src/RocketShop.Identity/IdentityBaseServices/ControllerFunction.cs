using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using RocketShop.Framework.Extension;

namespace RocketShop.Identity.IdentityBaseServices
{
    public class ControllerFunction(ILogger<ControllerFunction> logger) : Controller
    {
        protected IActionResult Respond<T>(Func<T> handler,
            Action<Exception>? catchOperation = null,
            int returnCodeWhenServerError = 500,
            string? returnErrorMessage = null,
            bool showStackTrace = true
            )
        {
            try
            {
                var result = handler();
                if (result.IsNull())
                    return NoContent();
                if (result is IActionResult)
                    return (result as IActionResult)!;
                return Ok(result);
            }
            catch (Exception x)
            {
                if (catchOperation.IsNotNull())
                    catchOperation!(x);
                dynamic errorObject;
                if (returnErrorMessage.HasMessage())
                    errorObject = returnErrorMessage!;
                else
                    errorObject = showStackTrace ? x : x.Message;
                logger.LogError(x, x.Message);
                return StatusCode(returnCodeWhenServerError, errorObject);
            }
        }
        protected async Task<IActionResult> RespondAsync<T>(Func<Task<T>> handler,
            Action<Exception>? catchOperation = null,
            int returnCodeWhenServerError = 500,
            string? returnErrorMessage = null,
            bool showStackTrace = true
            )
        {
            try
            {
                var result = await handler();
                if (result.IsNull())
                    return NoContent();
                return Ok(result);
            }
            catch (Exception x)
            {
                if (catchOperation.IsNotNull())
                    catchOperation!(x);
                dynamic errorObject;
                if (returnErrorMessage.HasMessage())
                    errorObject = returnErrorMessage!;
                else
                    errorObject = showStackTrace ? x : x.Message;
                logger.LogError(x, x.Message);
                return StatusCode(returnCodeWhenServerError, errorObject);
            }

        }
    }
}
