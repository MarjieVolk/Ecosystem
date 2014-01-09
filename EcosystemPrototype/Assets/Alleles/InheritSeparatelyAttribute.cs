using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Alleles
{
    public class InheritSeparatelyAttribute : Attribute
    {
        public string InheritanceGroup;

        public InheritSeparatelyAttribute(string inheritanceGroup)
        {
            this.InheritanceGroup = inheritanceGroup;
        }
    }
}
