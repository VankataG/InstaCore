namespace InstaCore.Core.Exceptions
{
    public sealed class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "Invalid credentials.") { }
    }
}
