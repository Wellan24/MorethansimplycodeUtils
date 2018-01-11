using GenericForms.Implemented;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericForms.Settings
{
    class PropertyControlSettingsEnum
    {

        public static PropertyControlSettings TextBoxDefault
        { get { return new PropertyControlSettings(textBoxDefault); } }

        private static readonly PropertyControlSettings textBoxDefault = new PropertyControlSettings
        {
            Type = typeof(PropertyControlTextBox)
        };


        public static PropertyControlSettings TextBoxDefaultNoEmpty
        { get { return new PropertyControlSettings(textBoxDefaultNoEmpty); } }

        private static readonly PropertyControlSettings textBoxDefaultNoEmpty = new PropertyControlSettings
        {
            Type = typeof(PropertyControlTextBox),
            Validate = ValidateEnum.noEmpty,
            OnValid = ValidationsEnum.DefaultRight,
            OnInvalid = ValidationsEnum.DefaultWrong
        };


        public static PropertyControlSettings ComboBoxDefault
        { get { return new PropertyControlSettings(comboBoxDefault); } }

        private static readonly PropertyControlSettings comboBoxDefault = new PropertyControlSettings
        {
            Type = typeof(PropertyControlComboBox)
        };


        public static PropertyControlSettings ComboBoxDefaultNoEmpty
        { get { return new PropertyControlSettings(comboBoxDefaultNoEmpty); } }

        private static readonly PropertyControlSettings comboBoxDefaultNoEmpty = new PropertyControlSettings
        {
            Type = typeof(PropertyControlComboBox),
            Validate = ValidateEnum.noEmpty,
            OnValid = ValidationsEnum.DefaultRight,
            OnInvalid = ValidationsEnum.DefaultWrong
        };

    }
}
