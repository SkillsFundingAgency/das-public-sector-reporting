using System;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepArgumentTransformations
{
    public class StepArgumentTransformations
    {
        [StepArgumentTransformation(@"at time (\d\d:\d\d)")]
        public DateTime DateTimeFromTimeAsStringTransform(string twentyFourHourRimeAsString)
        {
            return
                DateTime
                    .ParseExact(
                        twentyFourHourRimeAsString,
                        "HH:mm",
                        System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}