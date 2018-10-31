using System;

namespace SFA.DAS.PSRService.Domain.Entities.QuestionHelpers
{
    public class FactorsAnswerFinder
    {
        private Lazy<FactorAnswerFinder> outlineActions;
        private Lazy<FactorAnswerFinder> challenges;
        private Lazy<FactorAnswerFinder> targetPlans;
        private Lazy<FactorAnswerFinder> anythingElse;

        public FactorsAnswerFinder(Section factorsSection)
        {
            if (factorsSection == null) throw new ArgumentNullException(nameof(factorsSection));

            outlineActions = new Lazy<FactorAnswerFinder>(() => new OutlineActionsAnswerFinder(factorsSection));
            challenges = new Lazy<FactorAnswerFinder>(() => new ChallengesAnswerFinder(factorsSection));
            targetPlans = new Lazy<FactorAnswerFinder>(() => new TargetPlansAnswerFinder(factorsSection));
            anythingElse = new Lazy<FactorAnswerFinder>(() => new AnythingElseAnswerFinder(factorsSection));
        }

        public FactorAnswerFinder OutlineActions => outlineActions.Value;
        public FactorAnswerFinder Challenges => challenges.Value;
        public FactorAnswerFinder TargetPlans => targetPlans.Value;
        public FactorAnswerFinder AnythingElse => anythingElse.Value;
    }
}