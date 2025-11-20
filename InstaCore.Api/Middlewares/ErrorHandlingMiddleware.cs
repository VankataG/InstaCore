using InstaCore.Core.Exceptions;

namespace InstaCore.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException ex)
            {
                await WriteProblem(context, 404, "Not found", ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                await WriteProblem(context, 401, "Unauthorized", ex.Message);
            }
            catch (ConflictException ex)
            {
                await WriteProblem(context, 409, "Conflict", ex.Message);
            }
            catch (BadRequestException ex)
            {
                await WriteProblem(context, 400, "Bad request", ex.Message);
            }
            catch (Exception)
            {
                await WriteProblem(context, 500, "Server error", "Something went wrong.");
            }
        }

        public static Task WriteProblem(HttpContext context, int status, string title, string message)
        {
            context.Response.StatusCode = status;
            context.Response.ContentType = "application/json";
            var problem = new { title, status, message };
            return context.Response.WriteAsJsonAsync(problem);
        }
    }
}
