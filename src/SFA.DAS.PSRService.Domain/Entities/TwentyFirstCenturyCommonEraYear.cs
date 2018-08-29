using System;
using System.Globalization;

namespace SFA.DAS.PSRService.Domain.Entities
{
    public class TwentyFirstCenturyCommonEraYear
    {
        private DateTime firstOfJanThisYear;

        private TwentyFirstCenturyCommonEraYear(DateTime instantInYear)
        {
            firstOfJanThisYear
                =
               new DateTime(
                   year: instantInYear.Year,
                   month: 1,
                   day: 1);
        }
        public static TwentyFirstCenturyCommonEraYear ParseTwoDigitYear(string twoDigitYear)
        {
            var commonEra21stCenturyYearAsFourDigitString = "20" + twoDigitYear;

            DateTime parsedInstant;

            if (DateTime
                .TryParseExact(
                    commonEra21stCenturyYearAsFourDigitString,
                    "yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out parsedInstant))
            {
                return  new TwentyFirstCenturyCommonEraYear(parsedInstant);
            }

            throw new ArgumentException(
                twoDigitYear+" is not a valid two digit year", 
                nameof(twoDigitYear));
        }

        public int AsInt => firstOfJanThisYear.Year;
        public string AsTwoDigitString => firstOfJanThisYear.ToString("yy");
    }
}