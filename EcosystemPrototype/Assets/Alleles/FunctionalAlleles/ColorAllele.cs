using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Alleles.FunctionalAlleles
{
    public class ColorAllele : Allele
    {
        [GeneticallyInheritable]
        public Material Color;

        void Start()
        {
            gameObject.renderer.material = Color;
        }
    }
}
