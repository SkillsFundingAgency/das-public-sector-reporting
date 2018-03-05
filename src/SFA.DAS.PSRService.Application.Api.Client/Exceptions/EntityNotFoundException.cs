using System;

namespace SFA.DAS.PSRService.Application.Api.Client.Exceptions
{
    public class EntityNotFoundException : ApplicationException
    {
        public EntityNotFoundException()
        {
            
        }
        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}