using System;

namespace SFA.DAS.PSRService.Domain.Enums
{
    [Flags]
    public enum CompletionStatus
    {
        Incomplete = 1 << 0,
        InProgress = 1 << 1,
        Completed = 1<< 2,
        Optional = 1 << 3
    }
}
