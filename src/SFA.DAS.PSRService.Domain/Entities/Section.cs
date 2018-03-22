using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SFA.DAS.PSRService.Domain.Entities
{
    using System;
    using Enums;

    public class Section
    {
       public IEnumerable<Section> SubSections { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string SummaryText { get; set; }
    }
}
