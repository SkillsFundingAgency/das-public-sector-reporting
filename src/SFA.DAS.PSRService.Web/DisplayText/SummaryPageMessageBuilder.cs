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
            return $"You can check or change your information.{Environment.NewLine} You do not have the admin rights needed to submit this report.";
        }
    }
}