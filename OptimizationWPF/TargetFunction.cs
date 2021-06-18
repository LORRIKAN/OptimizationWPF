using System;
using System.Collections.Generic;

namespace OptimizationWPF
{
    public class TargetFunction : ITargetFunction
    {
        public TargetFunction(Func<Variable[], bool>[] restrictionsOfSecondKind, VariableInfo[] variablesInfo,
            Func<Variable[], double> func, Func<IEnumerable<double>, int> bestValue,
            Func<IEnumerable<double>, int> worstValue)
        {
            RestrictionsOfSecondKind = restrictionsOfSecondKind ?? throw new ArgumentNullException(nameof(restrictionsOfSecondKind));
            VariablesInfo = variablesInfo ?? throw new ArgumentNullException(nameof(variablesInfo));
            Func = func ?? throw new ArgumentNullException(nameof(func));
            BestValue = bestValue ?? throw new ArgumentNullException(nameof(bestValue));
            WorstValue = worstValue ?? throw new ArgumentNullException(nameof(worstValue));
        }

        public Func<Variable[], bool>[] RestrictionsOfSecondKind { get; set; }
        public VariableInfo[] VariablesInfo { get; set; }
        public Func<Variable[], double> Func { get; set; }
        public Func<IEnumerable<double>, int> BestValue { get; set; }
        public Func<IEnumerable<double>, int> WorstValue { get; set; }
    }
}
