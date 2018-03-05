﻿using Newtonsoft.Json;

namespace SFA.DAS.PSRService.Settings
{
    public class ApiAuthentication : IApiAuthentication
    {
        [JsonRequired]
        public string ClientId { get; set; }
        [JsonRequired]
        public string Instance { get; set; }
        
        [JsonRequired]
        public string TenantId { get; set; }
        [JsonRequired]
        public string Audience { get; set; }

    }
}