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
            OnValid = ValidationsEnum.RightWithoutMessage,
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
            OnValid = ValidationsEnum.RightWithoutMessage,
            OnInvalid = ValidationsEnum.DefaultWrong
        };


        public static PropertyControlSettings DateTimeDefault
        { get { return new PropertyControlSettings(dateTimeDefault); } }

        private static readonly PropertyControlSettings dateTimeDefault = new PropertyControlSettings
        {
            Type = typeof(PropertyControlDateTime)
        };


        public static PropertyControlSettings DateTimeDefaultNoEmpty
        { get { return new PropertyControlSettings(dateTimeDefaultNoEmpty); } }

        private static readonly PropertyControlSettings dateTimeDefaultNoEmpty = new PropertyControlSettings
        {
            Type = typeof(PropertyControlDateTime),
            Validate = ValidateEnum.noEmpty,
            OnValid = ValidationsEnum.RightWithoutMessage,
            OnInvalid = ValidationsEnum.DefaultWrong
        };

        public static PropertyControlSettings TextBoxIsValidEmail
        { get { return new PropertyControlSettings(textBoxIsValidEmail); } }

        private static readonly PropertyControlSettings textBoxIsValidEmail = new PropertyControlSettings
        {
            Type = typeof(PropertyControlTextBox),
            Validate = ValidateEnum.isValidEmail,
            OnValid = ValidationsEnum.RightWithoutMessage,
            OnInvalid = ValidationsEnum.DefaultWrong
        };

        public static PropertyControlSettings TextBoxIsValidEmailOrEmpty
        { get { return new PropertyControlSettings(textBoxIsValidEmailOrEmpty); } }

        private static readonly PropertyControlSettings textBoxIsValidEmailOrEmpty = new PropertyControlSettings
        {
            Type = typeof(PropertyControlTextBox),
            Validate = ValidateEnum.isValidEmailOrEmpty,
            OnValid = ValidationsEnum.RightWithoutMessage,
            OnInvalid = ValidationsEnum.DefaultWrong
        };

    }
}
