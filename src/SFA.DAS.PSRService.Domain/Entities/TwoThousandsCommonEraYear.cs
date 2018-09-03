using System;
using System.Globalization;

namespace SFA.DAS.PSRService.Domain.Entities
{
    public class TwoThousandsCommonEraYear : IEquatable<TwoThousandsCommonEraYear>
    {
        public static TwoThousandsCommonEraYear FromYearAsNumber(int year)
        {
            if (year < 2000 || year > 2099)
                throw new ArgumentException($"{year} is not a 21st century common era year", nameof(year));

            return new TwoThousandsCommonEraYear(year);
        }

        public static TwoThousandsCommonEraYear ParseTwoDigitYear(string twoDigitYear)
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
                return new TwoThousandsCommonEraYear(parsedInstant);
            }

            throw new ArgumentException(
                twoDigitYear + " is not a valid two digit year",
                nameof(twoDigitYear));
        }

        public int AsInt => firstOfJanThisYear.Year;

        public string AsTwoDigitString => firstOfJanThisYear.ToString("yy");

        public string AsFourDigitString => firstOfJanThisYear.ToString("yyyy");

        public bool Equals(TwoThousandsCommonEraYear other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return AsInt.Equals(other.AsInt);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TwoThousandsCommonEraYear) obj);
        }

        public override int GetHashCode()
        {
            return AsInt.GetHashCode();
        }

        private DateTime firstOfJanThisYear;

        private TwoThousandsCommonEraYear(DateTime instantInYear)
            : this(instantInYear.Year)
        {
        }

        private TwoThousandsCommonEraYear(int year)
        {
            firstOfJanThisYear
                =
                new DateTime(
                    year: year,
                    month: 1,
                    day: 1);
        }
    }
}