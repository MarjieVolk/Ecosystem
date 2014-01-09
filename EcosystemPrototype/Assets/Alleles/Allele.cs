using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using Assets.Alleles;

namespace Assets.Alleles
{
    public abstract class Allele : MonoBehaviour
    {

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                SetActive(value);
            }
        }

        public Gene gene;

        private Genome genome;
        private bool _isActive;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        protected void SetActive(bool active) { }

        public void clone(Allele template)
        {
            if (template == null || template.GetType() != this.GetType())
                throw new ArgumentException();
            //for each field in the shared type
            FieldInfo[] fields = this.GetType().GetFields();
            foreach (FieldInfo field in fields)
            {
                //if it should be copied
                if (field.GetCustomAttributes(typeof(GeneticallyInheritableAttribute), false).Length > 0)
                {
                    //(shallow) copy it.  Assumed to be fine b/c this is genetic configuration information and should not mutate.
                    field.SetValue(this, field.GetValue(template));
                }
            }
        }

        public void setGenome(Genome g)
        {
            this.genome = g;
        }
    }
}
