namespace Cartif.IO {

    ///------------------------------------------------------------------------------------------------------
    /// <summary> Overwriting policy. </summary>
    /// <remarks> Oscvic, 2016-01-04. </remarks>
    ///------------------------------------------------------------------------------------------------------
    public enum Overwrite {
        /// <summary>
        /// Always overwrite the destination if it exists.
        /// </summary>
        Always, /* An enum constant representing the always option */
        /// <summary>
        /// Never overwrite the destination and leave existing files as they are.
        /// <remarks>
        /// Methods should not throw if the destination file exists and
        /// do nothing instead. There is a throw value for that.
        /// </remarks>
        /// </summary>
        Never,  /* An enum constant representing the never option */
        /// <summary>
        /// Only overwrite an existing destination file with the same name
        /// if the source file is newer.
        /// <remarks>This value is recommended as the default.</remarks>
        /// </summary>
        IfNewer,    /* An enum constant representing if newer option */
        /// <summary>
        /// Throw if the destination file already exists.
        /// </summary>
        Throw   /* An enum constant representing the throw option */
    }
}