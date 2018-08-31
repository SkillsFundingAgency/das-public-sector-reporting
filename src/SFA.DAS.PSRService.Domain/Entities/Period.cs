using System;

namespace SFA.DAS.PSRService.Domain.Entities
{
    public class Period : IEquatable<Period>
    {
        public TwentyFirstCenturyCommonEraYear StartYear { get; }

        public TwentyFirstCenturyCommonEraYear EndYear { get; }

        public string FullString => $"1 April {StartYear.AsFourDigitString} to 31 March {EndYear.AsFourDigitString}";

        public string PeriodString => GetReportPeriod();

        private Period(
            TwentyFirstCenturyCommonEraYear startYear,
            TwentyFirstCenturyCommonEraYear endYear)
        {
            EndYear = endYear;
            StartYear = startYear;
        }

        public static Period FromInstantInPeriod(DateTime instantInPeriod)
        {
            var endYear =
                instantInPeriod.Month < 4
                    ? instantInPeriod.Year - 1
                    : instantInPeriod.Year;

            return new Period(
                startYear: TwentyFirstCenturyCommonEraYear.FromYearAsNumber(endYear - 1),
                endYear: TwentyFirstCenturyCommonEraYear.FromYearAsNumber(endYear));
        }

        private static int ConvertPeriodStringToYear(string period)
        {
            if (period == null || period.Length != 4)
                throw new ArgumentException("Period string has to be 4 chars", nameof(period));

            var year = int.Parse(period.Substring(0, 2)) + 2001;
            return year;
        }

        public static Period ParsePeriodString(string periodString)
        {
            if (string.IsNullOrWhiteSpace(periodString))
                throw new ArgumentException("Period string has to be 4 chars", nameof(periodString));

            if (periodString.Length != 4)
                throw new ArgumentException("Period string has to be 4 chars", nameof(periodString));

            return new Period(
                startYear:
                TwentyFirstCenturyCommonEraYear
                    .ParseTwoDigitYear(periodString.Substring(0, 2)),
                endYear:
                TwentyFirstCenturyCommonEraYear
                    .ParseTwoDigitYear(periodString.Substring(2, 2)));
        }

        public string GetReportPeriod()
        {
            return StartYear.AsTwoDigitString + EndYear.AsTwoDigitString;
        }

        public bool Equals(Period other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return StartYear.Equals(other.StartYear) && EndYear.Equals(other.EndYear);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Period) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((StartYear != null ? StartYear.GetHashCode() : 0) * 397) ^ (EndYear != null ? EndYear.GetHashCode() : 0);
            }
        }
    }
}
