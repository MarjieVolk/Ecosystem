using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

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

        public static void GenerateDefaultDataAlleles(GameObject gameObject, Dictionary<string, Allele> halfOne, Dictionary<string, Allele> halfTwo)
        {
            IEnumerable<string> allGenes = Enumerable.Union<string>(halfOne.Keys, halfTwo.Keys);
            generateDefaultAlleles(gameObject, allGenes, halfOne, halfTwo);
            generateDefaultAlleles(gameObject, allGenes, halfTwo, halfOne);
        }

        private static void generateDefaultAlleles(GameObject gameObject, IEnumerable<string> genes, Dictionary<string, Allele> target, Dictionary<string, Allele> source)
        {
            foreach (string gene in genes)
            {
                if (!target.ContainsKey(gene))
                {
                    if (source[gene].GetType().Equals(typeof(DataAllele)))
                    {
                        DataAllele defaultCopy = gameObject.AddComponent<DataAllele>();
                        defaultCopy.CopyFrom((DataAllele)source[gene]);
                        defaultCopy.ResetToDefaultValues();
                        target[gene] = defaultCopy;
                    }
                }
            }
        }

        /// <summary>
        /// Organizes the fields in the given type tagged with InheritSeparately by their inheritance group.
        /// </summary>
        /// <param name="allele">
        /// The type to reflect upon.
        /// </param>
        /// <returns>
        /// The tagged fields in allele, organized by inheritance group.
        /// </returns>
        private static Dictionary<string, List<FieldInfo>> collectFieldsByInheritanceGroup(Type allele)
        {
            Dictionary<string, List<FieldInfo>> fields = new Dictionary<string, List<FieldInfo>>();
            foreach (FieldInfo field in allele.GetFields())
            {
                InheritSeparatelyAttribute[] attributes = (InheritSeparatelyAttribute[])field.GetCustomAttributes(typeof(InheritSeparatelyAttribute), false);
                if (attributes.Length > 1) throw new InvalidAttributeException();
                foreach (InheritSeparatelyAttribute attribute in attributes)
                {
                    string inheritanceGroup = attribute.InheritanceGroup;
                    if (!fields.ContainsKey(inheritanceGroup))
                    {
                        fields[inheritanceGroup] = new List<FieldInfo>();
                    }
                    fields[inheritanceGroup].Add(field);
                }
            }
            return fields;
        }

        public static string nameDataAllele(Allele functionalAllele, string inheritanceGroup)
        {
            return functionalAllele.gene + Genome.GENE_DELIMITER + inheritanceGroup;
        }
        
        /// <summary>
        /// Generates the data alleles needed to allow the configuration of the given functional allele to be genetically inherited as intended.
        /// Adds them to the organism in the process.
        /// </summary>
        /// <param name="functionalAllele"></param>
        public static void generateDataAlleles(GameObject gameObject, Allele functionalAllele)
        {
            Type alleleType = functionalAllele.GetType();

            //for each inheritance group
            Dictionary<string, List<FieldInfo>> fields = collectFieldsByInheritanceGroup(alleleType);
            foreach (string inheritanceGroup in fields.Keys)
            {
                //create a data allele
                DataAllele dataAllele = gameObject.AddComponent<DataAllele>();
                dataAllele.gene = nameDataAllele(functionalAllele, inheritanceGroup);

                //for each field in the inheritance group
                foreach (FieldInfo field in fields[inheritanceGroup])
                {
                    //put that data in the data allele
                    dataAllele.Data[field] = field.GetValue(functionalAllele);
                }
            }
        }
    }
}
