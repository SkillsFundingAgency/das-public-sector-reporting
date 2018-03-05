namespace SFA.DAS.PSRService.Application.Exceptions
{
    using System;

    public sealed class ValidationException : Exception
    {
        public ValidationException() : base("") { }
        public ValidationException(string message) : base(message) { }
    }
}
