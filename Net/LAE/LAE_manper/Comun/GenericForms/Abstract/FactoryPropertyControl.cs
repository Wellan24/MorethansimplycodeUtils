using GenericForms.Implemented;
using GenericForms.Settings;
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
                control.DisplayMemberPath = settings.DisplayMemberPath;
                control.PathValue = settings.PathValue;
                control.PropertyName = propertyName;
                control.Label = (settings.Label != null) ? settings.Label : propertyName;
                control.MinWidthLabel = settings.MinWidthLabel;
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
                if (innerValue.GetType().GetProperty(propertyName) != null)
                {
                    if (settings.TargetNull != null)
                        control.SetContentBinding(innerValue, settings.TargetNull);
                    else
                        control.SetContentBinding(innerValue);
                }
                control.AddSelectionChanged(settings.SelectionChanged ?? defaultSettings.SelectionChanged);
                control.AddValueChanged(settings.ValueChanged ?? defaultSettings.ValueChanged);
                control.AddCheckedChanged(settings.CheckedChanged ?? defaultSettings.CheckedChanged);
                control.AddTextChanged(settings.TextChanged ?? defaultSettings.TextChanged);
                control.AddKeyDown(settings.KeyDown ?? defaultSettings.KeyDown);

                control.DesignPath = settings.DesignPath;
                control.Click = settings.Click;
                control.KeyDownCombo = settings.KeyDownCombo;
                control.BackgroundColor = settings.BackgroundColor;
                control.HorizontalAlignment = settings.HorizontalAlignment;

                /*control.SelectedIndex = settings.SelectedIndex;*/ /* después de bindear objeto*/ /* no usar porque se ejecuta el selectionchanged antes de construir todo el panel */
            }

            return control;
        }
    }
}
