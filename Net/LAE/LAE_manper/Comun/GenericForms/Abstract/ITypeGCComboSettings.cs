using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericForms.Abstract
{
    public interface ITypeGCComboSettings
    {
        Object[] InnerValues { get; set; }
        ITypeGCComboSettings SetInnerValues(Object[] newInnerValues);

        String Path { get; set; }
        ITypeGCComboSettings SetPath(String newPath);

        String DisplayPath { get; set; }
        ITypeGCComboSettings SetDisplayPath(String newDisplayPath);
    }
}
