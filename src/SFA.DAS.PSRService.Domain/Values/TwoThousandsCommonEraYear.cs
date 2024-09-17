using System;
using System.Globalization;

namespace SFA.DAS.PSRService.Domain.Values;

public class TwoThousandsCommonEraYear : IEquatable<TwoThousandsCommonEraYear>
{
    public static TwoThousandsCommonEraYear FromYearAsNumber(int year)
    {
        if (year is < 2000 or > 2099)
        {
            throw new ArgumentException($"{year} is not a two thousands common era year", nameof(year));
        }

        return new TwoThousandsCommonEraYear(year);
    }

    public static TwoThousandsCommonEraYear ParseTwoDigitYear(string twoDigitYear)
    {
        var commonEra21stCenturyYearAsFourDigitString = "20" + twoDigitYear;

        if (DateTime.TryParseExact(commonEra21stCenturyYearAsFourDigitString, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedInstant))
        {
            return new TwoThousandsCommonEraYear(parsedInstant);
        }

        throw new ArgumentException(twoDigitYear + " is not a valid two digit year", nameof(twoDigitYear));
    }

    public int AsInt => _firstOfJanThisYear.Year;

    public string AsTwoDigitString => _firstOfJanThisYear.ToString("yy");

    public string AsFourDigitString => _firstOfJanThisYear.ToString("yyyy");

    public bool Equals(TwoThousandsCommonEraYear other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return AsInt.Equals(other.AsInt);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return Equals((TwoThousandsCommonEraYear)obj);
    }

    public override int GetHashCode()
    {
        return AsInt.GetHashCode();
    }

    private readonly DateTime _firstOfJanThisYear;

    private TwoThousandsCommonEraYear(DateTime instantInYear)
        : this(instantInYear.Year)
    {
    }

    private TwoThousandsCommonEraYear(int year)
    {
        _firstOfJanThisYear = new DateTime(year: year, month: 1, day: 1);
    }
}