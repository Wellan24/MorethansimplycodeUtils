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
    class TypeGCButtonSettings : ITypeGCButtonSettings
    {
        public string DesingPath { get; set; }
        public ITypeGCButtonSettings SetPath(string newPath)
        {
            // TypeGCButtonSettings this = new TypeGCButtonSettings(this);
            this.DesingPath = newPath;
            return this;
        }

        public Color Color { get; set; }
        public ITypeGCButtonSettings SetColor(Color newColor)
        {
            // TypeGCButtonSettings this = new TypeGCButtonSettings(this);
            this.Color = newColor;
            return this;
        }

        public double Size { get; set; }
        public ITypeGCButtonSettings SetSize(double newSize)
        {
            // TypeGCButtonSettings this = new TypeGCButtonSettings(this);
            this.Size = newSize;
            return this;
        }

        public double Margin { get; set; }
        public ITypeGCButtonSettings SetMargin(double newMargin)
        {
            // TypeGCButtonSettings this = new TypeGCButtonSettings(this);
            this.Margin = newMargin;
            return this;
        }

        public Action<object, RoutedEventArgs> Click { get; set; }
        public ITypeGCButtonSettings SetClick(Action<object, RoutedEventArgs> newClick)
        {
            // TypeGCButtonSettings this = new TypeGCButtonSettings(this);
            this.Click = newClick;
            return this;
        }

        public bool? Enabled { get; set; }
        public ITypeGCButtonSettings SetEnabled(bool newEnabled)
        {
            // TypeGCButtonSettings this = new TypeGCButtonSettings(this);
            this.Enabled = newEnabled;
            return this;
        }

        /* checkbox */
        //public Object[] DesignPaths { get; set; }
        //public ITypeGCButtonSettings SetDesignPaths(Object[] newDesigPaths)
        //{
        //    // TypeGCButtonSettings this = new TypeGCButtonSettings(this);
        //    this.DesignPaths = newDesigPaths;
        //    return this;
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
