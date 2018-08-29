using System;
using System.Globalization;

namespace SFA.DAS.PSRService.Domain.Entities
{
    public class Period
    {
        private readonly DateTime _periodDateTime;
        private int _startYear => GetReportPeriodStartYear();
        private int _endYear => GetReportPeriodEndYear();

        public string StartYear => (_startYear).ToString();
        public string EndYear => (_endYear).ToString();
        public string FullString => GetCurrentReportPeriodName();
        public string PeriodString => GetReportPeriod();

        public Period(DateTime period)
        {
            _periodDateTime = period;
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

            if(periodString.Length != 4)
                throw new ArgumentException("Period string has to be 4 chars", nameof(periodString));

            return new Period(DateTime.MinValue);
        }

        public Period(string period)
        {
            var year = ConvertPeriodStringToYear(period);

            _periodDateTime = new DateTime(year, 4, 1);
        }

        public string GetReportPeriod()
        {
            return string.Concat(_startYear.ToString(CultureInfo.InvariantCulture).Substring(2),
                _endYear.ToString(CultureInfo.InvariantCulture).Substring(2));
        }

        private int GetReportPeriodEndYear()
        {
            var year = _periodDateTime.Year;
            if (_periodDateTime.Month < 4) year--;
            return year;
        }

        private int GetReportPeriodStartYear()
        {
            return _endYear - 1;
        }

        private string GetCurrentReportPeriodName()
        {
            return $"1 April {_startYear} to 31 March {_endYear}";
        }
    }
}
