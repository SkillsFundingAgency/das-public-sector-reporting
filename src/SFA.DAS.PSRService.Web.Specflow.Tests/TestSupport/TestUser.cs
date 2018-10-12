
using System.Diagnostics;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport
{
    [DebuggerDisplay("{" + nameof(DisplayName) + "}")]
    public class TestUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }

        public string ToJson()
        {
            return $"{{\"Id\":\"{Id}\"  ,\"Name\":\"{DisplayName}\"}}";
        }
    }
}
