using System;

namespace SFA.DAS.PSRService.Web.DisplayText
{
    public class SummaryPageMessageBuilder
    {
        public static SummaryPageMessageBuilder GetSubtitle()
        {
            return new SummaryPageMessageBuilder();
        }

        public string ForViewOnlyUser()
        {
            return String.Empty;
        }

        public string ForUserWhoCanSubmit()
        {
            return "Check or amend your information before you continue.";
        }

        public string ForUserWhoCanEditButNotSubmit()
        {
            return "Check or amend your information.";
        }
    }
}