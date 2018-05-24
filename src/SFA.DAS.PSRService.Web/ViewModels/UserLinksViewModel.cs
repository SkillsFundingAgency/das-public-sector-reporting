using System;
using IdentityModel.Client;
using Microsoft.AspNetCore.WebSockets.Internal;

namespace SFA.DAS.EAS.Web.ViewModels
{
    public static class UserLinksViewModel
    {
        public static string ChangePasswordLink { get; set; }
        public static string ChangeEmailLink { get; set; }
    }
}