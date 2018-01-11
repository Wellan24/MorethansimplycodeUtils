using GenericForms.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericForms.Settings
{
    class TypeGCComboSettings : ITypeGCComboSettings
    {

        public object[] InnerValues { get; set; }
        public ITypeGCComboSettings SetInnerValues(object[] newInnerValues)
        {
            // TypeGCComboSettings this = new TypeGCComboSettings(this);
            this.InnerValues = newInnerValues;
            return this;
        }

        public string Path { get; set; }
        public ITypeGCComboSettings SetPath(string newPath)
        {
            // TypeGCComboSettings this = new TypeGCComboSettings(this);
            this.Path = newPath;
            return this;
        }

        public string DisplayPath { get; set; }
        public ITypeGCComboSettings SetDisplayPath(string newDisplayPath)
        {
            // TypeGCComboSettings this = new TypeGCComboSettings(this);
            this.DisplayPath = newDisplayPath;
            return this;
        }

        public TypeGCComboSettings() { }

        public TypeGCComboSettings(TypeGCComboSettings copy)
        {
            InnerValues = copy.InnerValues;
            Path = copy.Path;
            DisplayPath = copy.DisplayPath;
        }

    }
}
