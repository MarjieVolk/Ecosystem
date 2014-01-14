using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Assets.Alleles
{
    public class DataAllele : Allele
    {
        [GeneticallyInheritable]
        public Dictionary<FieldInfo, object> Data;

        public DataAllele()
        {
            Data = new Dictionary<FieldInfo, object>();
        }

        public void GenerateDefaultCopy(DataAllele original)
        {
            gene = original.gene;

            //for each field
            foreach (FieldInfo field in original.Data.Keys)
            {
                //assume there's exactly one InheritSeparatelyAttribute on each field (that's how they got here!)
                InheritSeparatelyAttribute attribute = (InheritSeparatelyAttribute)field.GetCustomAttributes(typeof(InheritSeparatelyAttribute), false)[0];
                //use the default value for that field
                Data[field] = attribute.DefaultValue;
            }
        }
    }
}
