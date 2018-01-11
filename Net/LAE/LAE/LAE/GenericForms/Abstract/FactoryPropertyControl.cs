using GenericForms.Implemented;
using GenericForms.Settings;
using LAE.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GenericForms.Abstract
{
    class FactoryPropertyControl
    {
        public static PropertyControl Build(Object innerValue, String propertyName, IPropertyControlSettings settings, IPropertyControlSettings defaultSettings)
        {
            if (defaultSettings == null)
                defaultSettings = PropertyControlSettingsEnum.TextBoxDefault;

            PropertyControl control = Activator.CreateInstance(settings.Type) as PropertyControl;

            if (control != null)
            {
                control.Name = propertyName;
                control.InnerValues = settings.InnerValues;
                control.PathValue = settings.PathValue;
                control.PropertyName = propertyName;
                control.Label = (settings.Label != null) ? settings.Label : propertyName;
                control.ControlToolTipText = settings.ControlToolTipText;
                /* el campo del Id siempre desactivado salvo que se indique lo contrario para el expresamente */
                if (!propertyName.Equals("Id"))
                    control.Enabled = settings.Enabled ?? defaultSettings.Enabled ?? true;
                else
                    control.Enabled = settings.Enabled ?? false;
                control.ReadOnly = settings.ReadOnly ?? defaultSettings.ReadOnly;
                control.HeightMultiline = settings.HeightMultiline;

                control.OnInvalid = settings.OnInvalid ?? defaultSettings.OnInvalid;
                control.OnValid = settings.OnValid ?? defaultSettings.OnValid;
                control.Validate = settings.Validate ?? defaultSettings.Validate;
                control.Type = settings.Type;
                /* el último para que ya se definan las validaciones */
                control.SetContentBinding(innerValue);
                control.AddSelectionChanged(settings.SelectionChanged);
            }

            return control;
        }
    }
}
