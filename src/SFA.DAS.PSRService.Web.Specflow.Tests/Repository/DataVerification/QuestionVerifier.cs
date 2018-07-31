﻿using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    public class QuestionVerifier
    {
        private readonly JToken _question;

        public QuestionVerifier(JToken question)
        {
            _question = question;
        }

        public void HasAnswer(string expectedValue)
        {
            Assert.AreEqual(
                expectedValue,
                _question["Answer"].Value<String>());
        }
    }
}