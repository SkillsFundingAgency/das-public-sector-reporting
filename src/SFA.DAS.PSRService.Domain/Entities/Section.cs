﻿using System.Collections.Generic;
using System.Linq;
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
        public CompletionStatus CompletionStatus
        {
            get
            {
                if (Questions != null && Questions.Any())
                {
                    if (Questions.Any(w => String.IsNullOrWhiteSpace(w.Answer)))
                        return CompletionStatus.Incomplete;

                    if (Questions.Any(w => w.Optional == false && String.IsNullOrWhiteSpace(w.Answer) == false))
                        return CompletionStatus.Completed;

                    return CompletionStatus.InProgress;
                }
                else
                {
                    if (SubSections.Any(w => w.CompletionStatus == CompletionStatus.InProgress))
                        return CompletionStatus.InProgress;

                    if (SubSections.All(w => w.CompletionStatus == CompletionStatus.Incomplete))
                        return CompletionStatus.Incomplete;

                    if (SubSections.All(w => w.CompletionStatus == CompletionStatus.Completed))
                        return CompletionStatus.Completed;
                }

                return CompletionStatus.Incomplete;
            }
        }
    }
}