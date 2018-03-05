using JWT;
using JWT.Algorithms;
using JWT.Builder;
using SFA.DAS.PSRService.Application.Api.Client;
using SFA.DAS.PSRService.Settings;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Security.Claims;

namespace SFA.DAS.PSRService.Web.UnitTests.TokenServiceTests
{
    //public class JWTTestBase
    //{
    //    protected const string TokenEncodingKey = "Wt+69DPlA9wjXl79V9N67bR4cpn9+3zZmgLJHBXy2aQ=";
    //    protected static Mock<ICache> Cache;
    //    protected static Mock<IHttpContextAccessor> ContextAccessor;
    //    protected static TokenService TokenService;
    //    public static FakeDateTimeProvider DateService { get; set; }

    //    public static void Setup()
    //    {
    //        //SystemTime.UtcNow = () => new DateTime(2018, 02, 15, 13, 0, 0);

    //        Cache = new Mock<ICache>();

    //        ContextAccessor = new Mock<IHttpContextAccessor>();

    //        ContextAccessor
    //            .Setup(c => c.HttpContext)
    //            .Returns(new DefaultHttpContext()
    //            {
    //                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
    //                {
    //                    new Claim("http://schemas.portal.com/ukprn", "12345")
    //                }))
    //            });
            
    //        DateService = new FakeDateTimeProvider();
    //        DateService.SetNow(new DateTime(2018, 02, 15, 13, 0, 0));
    //        TokenService = new TokenService(Cache.Object, new WebConfiguration()
    //        {
    //            ApiAuthentication = new ApiAuthentication()
    //            {
    //                TokenEncodingKey = TokenEncodingKey
    //            }
    //        }, DateService);
    //    }

    //    protected static string GenerateJwt(DateTime? expires = null)
    //    {
    //        var expireDateTime = expires ?? new DateTime(2018, 02, 15, 13, 30, 0);

    //        var jwt = new JwtBuilder().WithAlgorithm(new HMACSHA256Algorithm())
    //            .WithSecret(TokenEncodingKey)
    //            .Issuer("SFA.DAS.PSRService")
    //            .Audience("SFA.DAS.PSRService.api")
    //            .ExpirationTime(expireDateTime)
    //            .AddClaim("ukprn", "USERID")
    //            .Build();
    //        return jwt;
    //    }
    //}

    //public class FakeDateTimeProvider : IDateTimeProvider
    //{
    //    private DateTime _now;

    //    public void SetNow(DateTime now)
    //    {
    //        _now = now;
    //    }

    //    public DateTimeOffset GetNow()
    //    {
    //        return _now;
    //    }
    //}
}
