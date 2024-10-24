using System;
using SFA.DAS.PSRService.Domain.Values;

namespace SFA.DAS.PSRService.Domain.Entities;

public sealed class Period : IEquatable<Period>
{
    public TwoThousandsCommonEraYear StartYear { get; }

    public TwoThousandsCommonEraYear EndYear { get; }

    public string FullString => $"1 April {StartYear.AsFourDigitString} to 31 March {EndYear.AsFourDigitString}";

    public string PeriodString => GetReportPeriod();

    public static Period FromInstantInPeriod(DateTime instantInPeriod)
    {
        var endYear = instantInPeriod.Month < 4
                ? instantInPeriod.Year - 1
                : instantInPeriod.Year;

        return new Period(
            startYear: TwoThousandsCommonEraYear.FromYearAsNumber(endYear - 1),
            endYear: TwoThousandsCommonEraYear.FromYearAsNumber(endYear));
    }

    public static Period ParsePeriodString(string periodString)
    {
        if (string.IsNullOrWhiteSpace(periodString))
        {
            throw new ArgumentException($"{periodString} cannot be parsed; should be 4 chars e.g. 1516 to represent period 2015/2016", nameof(periodString));
        }

        if (periodString.Length != 4)
        {
            throw new ArgumentException($"{periodString} cannot be parsed; should be 4 chars e.g. 1516 to represent period 2015/2016", nameof(periodString));
        }

        return new Period(
            startYear: TwoThousandsCommonEraYear.ParseTwoDigitYear(periodString.Substring(0, 2)),
            endYear: TwoThousandsCommonEraYear.ParseTwoDigitYear(periodString.Substring(2, 2)));
    }

    private string GetReportPeriod()
    {
        return StartYear.AsTwoDigitString + EndYear.AsTwoDigitString;
    }

    public bool Equals(Period other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return StartYear.Equals(other.StartYear) && EndYear.Equals(other.EndYear);
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

        return obj.GetType() == GetType() && Equals((Period) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((StartYear != null ? StartYear.GetHashCode() : 0) * 397) ^ (EndYear != null ? EndYear.GetHashCode() : 0);
        }
    }

    private Period(TwoThousandsCommonEraYear startYear, TwoThousandsCommonEraYear endYear)
    {
        EndYear = endYear;
        StartYear = startYear;
    }
}