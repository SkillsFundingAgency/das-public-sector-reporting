﻿using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification
{
    [ExcludeFromCodeCoverage]
    public class OrganisationNameVerifier
    {
        private readonly String _organisationName;

        public OrganisationNameVerifier(string reportData)
        {
            _organisationName = JObject.Parse(reportData)["OrganisationName"].Value<String>();
        }

        public void HasAnswer(string expectedAnswer)
        {
            Assert.AreEqual(
                expectedAnswer,
                _organisationName);
        }
    }
}
