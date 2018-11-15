using System;
using System.Collections.Generic;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.UnitTests
{
    public sealed class SubmittedEqualityComparer : IEqualityComparer<Submitted>
    {
        public bool Equals(Submitted x, Submitted y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.SubmittedAt.Equals(y.SubmittedAt) && String.Equals(x.SubmttedBy, y.SubmttedBy) && String.Equals(x.SubmittedName, y.SubmittedName) && String.Equals(x.SubmittedEmail, y.SubmittedEmail) && String.Equals(x.UniqueReference, y.UniqueReference);
        }

        public int GetHashCode(Submitted obj)
        {
            unchecked
            {
                var hashCode = obj.SubmittedAt.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.SubmttedBy.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.SubmittedName.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.SubmittedEmail.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.UniqueReference.GetHashCode();
                return hashCode;
            }
        }
    }
}