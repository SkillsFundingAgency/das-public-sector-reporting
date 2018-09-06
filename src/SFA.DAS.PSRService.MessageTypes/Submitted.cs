using System;

namespace SFA.DAS.PSRService.MessageTypes
{
    public class Submitted
    {
        public DateTime SubmittedAt { get; set; }

        public string SubmttedBy { get; set; }

        public string SubmittedName { get; set; }

        public string SubmittedEmail { get; set; }

        public string UniqueReference { get; set; }
    }
}