using System;

namespace OptimizationWPF
{
    public class SolutionMethod : ISolutionMethod
    {
        public SolutionMethod(Func<ITargetFunction, double, Tuple<Variable[], double>> getSolution)
        {
            GetSolution = getSolution ?? throw new ArgumentNullException(nameof(getSolution));
        }

        public Func<ITargetFunction, double, Tuple<Variable[], double>> GetSolution { get; set; }
    }
}