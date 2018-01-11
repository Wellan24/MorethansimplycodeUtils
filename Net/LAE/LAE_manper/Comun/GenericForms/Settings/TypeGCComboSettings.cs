using GenericForms.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericForms.Settings
{
    public class TypeGCComboSettings : ITypeGCComboSettings
    {

        public object[] InnerValues { get; set; }
        public ITypeGCComboSettings SetInnerValues(object[] newInnerValues)
        {
            TypeGCComboSettings tgcs = new TypeGCComboSettings(this);
            tgcs.InnerValues = newInnerValues;
            return tgcs;
        }

        public string Path { get; set; }
        public ITypeGCComboSettings SetPath(string newPath)
        {
            TypeGCComboSettings tgcs = new TypeGCComboSettings(this);
            tgcs.Path = newPath;
            return tgcs;
        }

        public string DisplayPath { get; set; }
        public ITypeGCComboSettings SetDisplayPath(string newDisplayPath)
        {
            TypeGCComboSettings tgcs = new TypeGCComboSettings(this);
            tgcs.DisplayPath = newDisplayPath;
            return tgcs;
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
