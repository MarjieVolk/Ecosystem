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

        /// <summary>
        /// Modifies this to have the same fields as original, each with its default value (NOT the value from original).
        /// Original is not mutated.
        /// </summary>
        /// <param name="original">
        /// The DataAllele to copy type information from.
        /// </param>
        public void CopyFrom(DataAllele original)
        {
            gene = original.gene;

            foreach (FieldInfo field in original.Data.Keys)
            {
                Data[field] = original.Data[field];
            }
        }

        public void ResetToDefaultValues()
        {
            foreach (FieldInfo field in Data.Keys)
            {
                //assume there's exactly one attribute of this type
                //(a valid assumption, since that's the only way this field could be here)
                InheritSeparatelyAttribute attribute = (InheritSeparatelyAttribute)field.GetCustomAttributes(typeof(InheritSeparatelyAttribute), false)[0];
                Data[field] = attribute.DefaultValue;
            }
        }
    }
}
