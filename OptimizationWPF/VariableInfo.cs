using System;
using System.Collections.Generic;

namespace OptimizationWPF
{
    public class VariableInfo : IEquatable<VariableInfo>
    {
        public string Name { get; set; }

        public double ValueLowerBound { get; set; }

        public double ValueUpperBound { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as VariableInfo);
        }

        public bool Equals(VariableInfo other)
        {
            return other != null &&
                   Name == other.Name &&
                   ValueLowerBound == other.ValueLowerBound &&
                   ValueUpperBound == other.ValueUpperBound;
        }

        public override int GetHashCode()
        {
            int hashCode = 1451490849;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + ValueLowerBound.GetHashCode();
            hashCode = hashCode * -1521134295 + ValueUpperBound.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(VariableInfo left, VariableInfo right)
        {
            return EqualityComparer<VariableInfo>.Default.Equals(left, right);
        }

        public static bool operator !=(VariableInfo left, VariableInfo right)
        {
            return !(left == right);
        }
    }
}