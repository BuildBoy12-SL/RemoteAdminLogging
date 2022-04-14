// -----------------------------------------------------------------------
// <copyright file="Translation.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace RemoteAdminLogging
{
    using System.ComponentModel;
    using Exiled.API.Interfaces;

    /// <inheritdoc />
    public class Translation : ITranslation
    {
        /// <summary>
        /// Gets or sets the header of the embed.
        /// </summary>
        [Description("The header of the embed.")]
        public string Header { get; set; } = "Remote Admin Logging";

        /// <summary>
        /// Gets or sets the literal translation for User.
        /// </summary>
        [Description("The literal translation for User.")]
        public string User { get; set; } = "User";

        /// <summary>
        /// Gets or sets the literal translation for User Id.
        /// </summary>
        [Description("The literal translation for User Id.")]
        public string UserId { get; set; } = "User Id";

        /// <summary>
        /// Gets or sets the literal translation for Command Executed.
        /// </summary>
        [Description("The literal translation for Command Executed.")]
        public string CommandExecuted { get; set; } = "Command Executed";
    }
}