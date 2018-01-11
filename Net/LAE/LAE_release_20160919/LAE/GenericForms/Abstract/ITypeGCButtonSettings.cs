using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GenericForms.Abstract
{
    public interface ITypeGCButtonSettings
    {
        String DesingPath { get; set; }
        ITypeGCButtonSettings SetPath(String newPath);

        Color Color { get; set; }
        ITypeGCButtonSettings SetColor(Color newColor);
        
        double Size { get; set; }
        ITypeGCButtonSettings SetSize(double newSize);
        
        double Margin { get; set; }
        ITypeGCButtonSettings SetMargin(double newMargin);

        Action<object, RoutedEventArgs> Click { get; set; }
        ITypeGCButtonSettings SetClick(Action<object, RoutedEventArgs> newClick);

        bool? Enabled { get; set; }
        ITypeGCButtonSettings SetEnabled(bool newEnabled);

        /* checkbox */
        //Object[] DesignPaths { get; set; }
        //ITypeGCButtonSettings SetDesignPaths(Object[] newDesigPaths);
    }
}
