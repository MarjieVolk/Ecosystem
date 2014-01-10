using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Alleles
{
    public class InheritSeparatelyAttribute : Attribute
    {
        public string InheritanceGroup;
        public object DefaultValue;

        public InheritSeparatelyAttribute(string inheritanceGroup, object defaultValue)
        {
            this.InheritanceGroup = inheritanceGroup;
            this.DefaultValue = defaultValue;
        }
    }
}
