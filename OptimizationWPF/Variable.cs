using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationWPF
{
    public class Variable
    {
        public Variable(VariableInfo variableInfo, double value)
        {
            VariableInfo = variableInfo ?? throw new ArgumentNullException(nameof(variableInfo));
            Value = value;
        }

        public VariableInfo VariableInfo { get; set; }

        public double Value { get; set; }

        public static implicit operator Variable(double value)
        {
            return new Variable(new VariableInfo(), value);
        }

        public static implicit operator double(Variable variable)
        {
            return variable.Value;
        }
    }
}