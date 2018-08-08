using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    [ExcludeFromCodeCoverage]
    public class QuestionVerifier
    {
        private readonly JToken _question;

        public QuestionVerifier(JToken question)
        {
            _question = question;
        }

        public void HasAnswer(string expectedAnswer)
        {
            Assert.AreEqual(
                expectedAnswer,
                _question["Answer"].Value<String>());
        }
    }
}