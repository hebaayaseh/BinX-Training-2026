namespace CardioTrack.ExceptionService
{
    public abstract class Exceptions : Exception
    {
        public int StatusCode { get; }
        protected Exceptions(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }

    public class NotFoundException : Exceptions
    {
        public NotFoundException(string message) : base(message, StatusCodes.Status404NotFound) { }
    }

    public class ConflictException : Exceptions
    {
        public ConflictException(string message) : base(message, StatusCodes.Status409Conflict) { }
    }

    public class ForbiddenException : Exceptions
    {
        public ForbiddenException(string message) : base(message, StatusCodes.Status403Forbidden) { }
    }

    public class BadRequestException : Exceptions
    {
        public BadRequestException(string message) : base(message, StatusCodes.Status400BadRequest) { }
    }

    public class InvalidTokenException : Exceptions
    {
        public InvalidTokenException(string message) : base(message, StatusCodes.Status401Unauthorized) { }
    }
}