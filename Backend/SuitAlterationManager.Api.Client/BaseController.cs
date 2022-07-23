using Microsoft.AspNetCore.Mvc;

namespace SuitAlterationManager.Extensions
{
    [Controller]
	public abstract class BaseController : ControllerBase
	{
		protected IActionResult NOk() => base.Ok(Envelope.Success(false));
		protected new IActionResult Ok() => base.Ok(Envelope.Success(true));
		protected IActionResult Ok<T>(T result) => base.Ok(Envelope.Success(result));
		protected IActionResult Created<T>(T result) => base.Created(string.Empty, Envelope.Success(result));
		protected new IActionResult BadRequest(object error) => base.BadRequest(Envelope.Failure(error));
		protected new IActionResult Unauthorized(object error) => base.Unauthorized(Envelope.Failure(error));
		protected new IActionResult NotFound(object error) => base.NotFound(Envelope.Failure(error));
	}
}