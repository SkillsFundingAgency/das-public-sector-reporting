using System;

namespace SFA.DAS.PSRService.Domain.Entities.QuestionHelpers
{
    public class FactorsAnswerFinder
    {
        private Lazy<IFactorAnswerFinder> outlineActions;
        private Lazy<IFactorAnswerFinder> challenges;
        private Lazy<IFactorAnswerFinder> targetPlans;
        private Lazy<IFactorAnswerFinder> anythingElse;

        public FactorsAnswerFinder(Section factorsSection)
        {
            if (factorsSection != null)
            {
                outlineActions = new Lazy<IFactorAnswerFinder>(() => new OutlineActionsAnswerFinder(factorsSection));
                challenges = new Lazy<IFactorAnswerFinder>(() => new ChallengesAnswerFinder(factorsSection));
                targetPlans = new Lazy<IFactorAnswerFinder>(() => new TargetPlansAnswerFinder(factorsSection));
                anythingElse = new Lazy<IFactorAnswerFinder>(() => new AnythingElseAnswerFinder(factorsSection));
            }
            else
            {
                outlineActions = new Lazy<IFactorAnswerFinder>(() => new AlwaysEmptyStringFactorAnswerFinder());
                challenges = new Lazy<IFactorAnswerFinder>(() => new AlwaysEmptyStringFactorAnswerFinder());
                targetPlans = new Lazy<IFactorAnswerFinder>(() => new AlwaysEmptyStringFactorAnswerFinder());
                anythingElse = new Lazy<IFactorAnswerFinder>(() => new AlwaysEmptyStringFactorAnswerFinder());
            }
        }

        public IFactorAnswerFinder OutlineActions => outlineActions.Value;
        public IFactorAnswerFinder Challenges => challenges.Value;
        public IFactorAnswerFinder TargetPlans => targetPlans.Value;
        public IFactorAnswerFinder AnythingElse => anythingElse.Value;
    }
}