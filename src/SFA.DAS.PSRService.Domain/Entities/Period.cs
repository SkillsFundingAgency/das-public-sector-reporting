using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SFA.DAS.PSRService.Domain.Entities
{
    public class Period
    {
        private DateTime _periodDateTime { get; set; }
        private int _startYear => GetReportPeriodStartYear();
        private int _endYear => GetReportPeriodEndYear();

        public string StartYear => (_startYear).ToString();
        public string EndYear => (_endYear).ToString();
        public string FullString => GetCurrentReportPeriodName();
        public bool IsCurrent => IsCurrentPeriod();
        public string PeriodString => GetReportPeriod();
        
        public Period(DateTime period)
        {
            _periodDateTime = period;
        }

        public Period(string periodString)
        {
            var year = ConvertPeriodStringToYear(periodString);

            _periodDateTime = new DateTime(year,4,1);
        }

        public string GetReportPeriod()
        {
           
            return string.Concat((_startYear).ToString(CultureInfo.InvariantCulture).Substring(2), (_endYear).ToString(CultureInfo.InvariantCulture).Substring(2));
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

        private Period GetCurrentReportPeriod()
        {
            return new Period(DateTime.UtcNow.Date);
        }
        
        private string GetCurrentReportPeriodName()
        {

            return $"1 April {_startYear} to 31 March {_endYear}";
        }

        private static int ConvertPeriodStringToYear(string period)
        {
            if (period == null || period.Length != 4)
                throw new ArgumentException("Period string has to be 4 chars", nameof(period));

            var year = int.Parse(period.Substring(0, 2)) + 2001;
            return year;
        }

        private bool IsCurrentPeriod()

        {
            return (GetCurrentReportPeriod().PeriodString == PeriodString);
        }


    }
}
