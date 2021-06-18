using System;

namespace OptimizationWPF
{
    public interface ISolutionMethod
    {
        Func<ITargetFunction, double, Tuple<Variable[], double>> GetSolution { get; set; }
    }
}