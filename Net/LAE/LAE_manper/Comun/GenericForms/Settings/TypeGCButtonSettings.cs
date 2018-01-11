using GenericForms.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GenericForms.Settings
{
    public class TypeGCButtonSettings : ITypeGCButtonSettings
    {
        public string DesingPath { get; set; }
        public ITypeGCButtonSettings SetPath(string newPath)
        {
            TypeGCButtonSettings tgcs = new TypeGCButtonSettings(this);
            tgcs.DesingPath = newPath;
            return tgcs;
        }

        public Color Color { get; set; }
        public ITypeGCButtonSettings SetColor(Color newColor)
        {
            TypeGCButtonSettings tgcs = new TypeGCButtonSettings(this);
            tgcs.Color = newColor;
            return tgcs;
        }

        public double Size { get; set; }
        public ITypeGCButtonSettings SetSize(double newSize)
        {
            TypeGCButtonSettings tgcs = new TypeGCButtonSettings(this);
            tgcs.Size = newSize;
            return tgcs;
        }

        public double Margin { get; set; }
        public ITypeGCButtonSettings SetMargin(double newMargin)
        {
            TypeGCButtonSettings tgcs = new TypeGCButtonSettings(this);
            tgcs.Margin = newMargin;
            return tgcs;
        }

        public Action<object, RoutedEventArgs> Click { get; set; }
        public ITypeGCButtonSettings SetClick(Action<object, RoutedEventArgs> newClick)
        {
            TypeGCButtonSettings tgcs = new TypeGCButtonSettings(this);
            tgcs.Click = newClick;
            return tgcs;
        }

        public bool? Enabled { get; set; }
        public ITypeGCButtonSettings SetEnabled(bool newEnabled)
        {
            TypeGCButtonSettings tgcs = new TypeGCButtonSettings(this);
            tgcs.Enabled = newEnabled;
            return tgcs;
        }

        /* checkbox */
        //public Object[] DesignPaths { get; set; }
        //public ITypeGCButtonSettings SetDesignPaths(Object[] newDesigPaths)
        //{
        //    TypeGCButtonSettings tgcs = new TypeGCButtonSettings(this);
        //    tgcs.DesignPaths = newDesigPaths;
        //    return tgcs;
        //}

        public TypeGCButtonSettings() { }

        public TypeGCButtonSettings(TypeGCButtonSettings copy)
        {
            DesingPath = copy.DesingPath;
            Color = copy.Color;
            Size = copy.Size;
            Margin = copy.Margin;
            Click = copy.Click;
            Enabled = copy.Enabled;
            /* checkbox */
            //DesignPaths = copy.DesignPaths;
        }

    }
}
