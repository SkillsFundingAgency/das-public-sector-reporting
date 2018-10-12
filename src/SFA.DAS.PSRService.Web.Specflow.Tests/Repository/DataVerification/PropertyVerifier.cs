using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    [ExcludeFromCodeCoverage]
    public sealed class PropertyVerifier
    {
        private readonly JToken _property;

        public PropertyVerifier(JToken property)
        {
            _property = property;
        }

        public void Is<TExpected>(TExpected expectedEmploymentStarts)
        {
            Assert
                .AreEqual(
                    expectedEmploymentStarts,
                    _property.Value<TExpected>());
        }
    }
}