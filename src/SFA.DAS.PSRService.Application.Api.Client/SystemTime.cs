using System;

namespace SFA.DAS.PSRService.Web.Infrastructure
{
    public static class SystemTime
    {
        /// <summary> Normally this is a pass-through to DateTime.Now, but it can be overridden with SetDateTime( .. ) for testing or debugging.
        /// </summary>
        public static Func<DateTime> UtcNow = () => DateTime.UtcNow;
    }
}