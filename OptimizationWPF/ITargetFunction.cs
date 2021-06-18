using System;
using System.Collections.Generic;

namespace OptimizationWPF
{
    public interface ITargetFunction
    {
        Func<Variable[], bool>[] RestrictionsOfSecondKind { get; set; }

        VariableInfo[] VariablesInfo { get; set; }

        Func<Variable[], double> Func { get; set; }

        Func<IEnumerable<double>, int> BestValue { get; set; }

        Func<IEnumerable<double>, int> WorstValue { get; set; }
    }
}