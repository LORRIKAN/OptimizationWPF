using System;

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

        public static explicit operator Variable(double value)
        {
            return new Variable(new VariableInfo(), value);
        }

        public static implicit operator double(Variable variable)
        {
            return variable.Value;
        }
    }
}