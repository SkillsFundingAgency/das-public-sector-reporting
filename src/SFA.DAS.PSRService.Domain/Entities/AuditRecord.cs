using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.PSRService.Domain.Entities
{
    public class AuditRecord
    {
        public int Id { get; set; }
        public DateTime UpdatedUtc { get; set; }
        public IEnumerable<Section> Sections { get; set; }

    }
}
