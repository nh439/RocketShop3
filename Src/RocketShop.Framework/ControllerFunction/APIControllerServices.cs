using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.ControllerFunction
{
    public class APIControllerServices : ControllerBase
    {
        readonly ILogger logger;
        public APIControllerServices(ILogger<APIControllerServices> logger)
        {
            this.logger = logger;
        }
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
                if(result.IsNull())
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
        protected async Task< IActionResult> RespondAsync<T>(Func<Task<T>> handler,
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
