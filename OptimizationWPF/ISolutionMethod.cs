using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationWPF
{
    public interface ISolutionMethod
    {
        Func<ITargetFunction, double, Tuple<Variable[], double>> GetSolution { get; set; }
    }
}