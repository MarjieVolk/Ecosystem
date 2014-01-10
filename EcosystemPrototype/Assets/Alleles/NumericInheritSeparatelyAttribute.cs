using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Alleles
{
    public class NumericInheritSeparatelyAttribute : Attribute
    {
        public string InheritanceGroup;
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int NumGenes { get; set; }

        public NumericInheritSeparatelyAttribute(string inheritanceGroup)
        {
            this.InheritanceGroup = inheritanceGroup;
            MinValue = 0;
            MaxValue = 1;
            NumGenes = 1;
        }
    }
}
