using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Assets.Alleles
{
    public class NumericDataAllele : Allele
    {
        [GeneticallyInheritable]
        public int value;

        public static void GenerateNumericDataAlelles(GameObject gameObject, Allele functionalAllele)
        {
            Type alleleType = functionalAllele.GetType();

            //for each field in functionalAllele's Type
            foreach (FieldInfo field in alleleType.GetFields())
            {
                NumericInheritSeparatelyAttribute[] attributes = (NumericInheritSeparatelyAttribute[])field.GetCustomAttributes(typeof(NumericInheritSeparatelyAttribute), false);
                if (attributes.Length < 1) continue;
                if (attributes.Length > 1) throw new InvalidAttributeException();
                if (field.GetType() != typeof(int)) throw new InvalidAttributeException();

                NumericInheritSeparatelyAttribute attribute = attributes[0];
                int value = (int)field.GetValue(functionalAllele);

                //now create some actual NumericDataAlleles
                //there will need to be NumGenes + 1 of them
                //one will always have precisely the minimum value of the property
                //numGenes - 1 of them will be able to take on values between 0 and something
                //the last one will pick up the remainder
            }
        }

        public static string NameNumericDataAllele(string gene, string inheritanceGroup, int index)
        {
            return gene + Genome.GENE_DELIMITER + inheritanceGroup + Genome.GENE_DELIMITER + index;
        }
    }
}
