using System;

namespace SFA.DAS.PSRService.Domain.Entities.QuestionHelpers
{
    public sealed class AlwaysEmptyStringFactorAnswerFinder
    :IFactorAnswerFinder
    {
        public string Answer => String.Empty;
    }
}