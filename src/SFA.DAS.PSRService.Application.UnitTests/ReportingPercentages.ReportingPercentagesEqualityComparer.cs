using System.Collections.Generic;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.UnitTests
{
    public sealed class ReportingPercentagesEqualityComparer : IEqualityComparer<ReportingPercentages>
    {
        public bool Equals(ReportingPercentages x, ReportingPercentages y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return string.Equals((string) x.EmploymentStarts, (string) y.EmploymentStarts) &&
                   string.Equals((string) x.TotalHeadCount, (string) y.TotalHeadCount) &&
                   string.Equals((string) x.NewThisPeriod, (string) y.NewThisPeriod);
        }

        public int GetHashCode(ReportingPercentages obj)
        {
            unchecked
            {
                var hashCode = obj.EmploymentStarts.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.TotalHeadCount.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.NewThisPeriod.GetHashCode();
                return hashCode;
            }
        }
    }
}