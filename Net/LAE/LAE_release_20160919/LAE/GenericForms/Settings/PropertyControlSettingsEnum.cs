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

        public static PropertyControlSettings TextBoxEmptyToNull
        { get { return new PropertyControlSettings(textBoxEmptyToNull); } }

        private static readonly PropertyControlSettings textBoxEmptyToNull = new PropertyControlSettings
        {
            Type = typeof(PropertyControlTextBox),
            TargetNull=String.Empty
        };


        public static PropertyControlSettings TextBoxDefaultLarge
        { get { return new PropertyControlSettings(textBoxDefaultLarge); } }

        private static readonly PropertyControlSettings textBoxDefaultLarge = new PropertyControlSettings
        {
            Type = typeof(PropertyControlTextBoxLarge)
        };


        public static PropertyControlSettings TextBoxDefaultLargeNoEmpty
        { get { return new PropertyControlSettings(textBoxDefaultLargeNoEmpty); } }

        private static readonly PropertyControlSettings textBoxDefaultLargeNoEmpty = new PropertyControlSettings
        {
            Type = typeof(PropertyControlTextBoxLarge),
            Validate = ValidateEnum.noEmpty,
            OnValid = ValidationsEnum.RightWithoutMessage,
            OnInvalid = ValidationsEnum.DefaultWrong
        };

        public static PropertyControlSettings TextBoxLargeEmptyToNull
        { get { return new PropertyControlSettings(textBoxLargeEmptyToNull); } }

        private static readonly PropertyControlSettings textBoxLargeEmptyToNull = new PropertyControlSettings
        {
            Type = typeof(PropertyControlTextBoxLarge),
            TargetNull = String.Empty
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


        public static PropertyControlSettings ComboBoxDefaultLarge
        { get { return new PropertyControlSettings(comboBoxDefaultLarge); } }

        private static readonly PropertyControlSettings comboBoxDefaultLarge = new PropertyControlSettings
        {
            Type = typeof(PropertyControlComboBoxLarge)
        };


        public static PropertyControlSettings ComboBoxDefaultLargeNoEmpty
        { get { return new PropertyControlSettings(comboBoxDefaultLargeNoEmpty); } }

        private static readonly PropertyControlSettings comboBoxDefaultLargeNoEmpty = new PropertyControlSettings
        {
            Type = typeof(PropertyControlComboBoxLarge),
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

        public static PropertyControlSettings CheckBoxDefault
        { get { return new PropertyControlSettings(checkBoxDefault); }  }

        private static readonly PropertyControlSettings checkBoxDefault = new PropertyControlSettings {
            Type=typeof(PropertyControlCheckBox)
        };

        public static PropertyControlSettings LabelDefault
        { get { return new PropertyControlSettings(labelDefault); } }

        private static readonly PropertyControlSettings labelDefault = new PropertyControlSettings
        {
            Type = typeof(PropertyControlLabel)
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


        public static PropertyControlSettings TextBoxIsValidNumber
        { get { return new PropertyControlSettings(textBoxIsValidNumber); } }

        private static readonly PropertyControlSettings textBoxIsValidNumber = new PropertyControlSettings
        {
            Type = typeof(PropertyControlTextBox),
            Validate = ValidateEnum.isValidNumber,
            OnValid = ValidationsEnum.RightWithoutMessage,
            OnInvalid = ValidationsEnum.DefaultWrong
        };


        public static PropertyControlSettings TextBoxIsValidNumberOrDefault
        { get { return new PropertyControlSettings(textBoxIsValidNumberOrDefault); } }

        private static readonly PropertyControlSettings textBoxIsValidNumberOrDefault = new PropertyControlSettings
        {
            Type = typeof(PropertyControlTextBox),
            Validate = ValidateEnum.isValidNumberOrEmpty,
            OnValid = ValidationsEnum.RightWithoutMessage,
            OnInvalid = ValidationsEnum.DefaultWrong
        };

    }
}
