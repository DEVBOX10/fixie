﻿namespace Fixie.Execution
{
    public class CasePassed : CaseCompleted
    {
        public CasePassed(Case @case)
            : base(
                methodGroup: @case.MethodGroup,
                name: @case.Name,
                status: CaseStatus.Passed,
                duration: @case.Duration,
                output: @case.Output
                )
        {
        }
    }
}