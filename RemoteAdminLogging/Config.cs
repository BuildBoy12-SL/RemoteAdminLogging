// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace RemoteAdminLogging
{
    using System.ComponentModel;
    using Exiled.API.Interfaces;

    /// <inheritdoc />
    public class Config : IConfig
    {
        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the url of the discord webhook.
        /// </summary>
        [Description("The url of the discord webhook.")]
        public string WebhookUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the user id will be shown next to the player's name.
        /// </summary>
        [Description("Whether the user id will be shown next to the player's name.")]
        public bool ShowUserId { get; set; } = true;

        /// <summary>
        /// Gets or sets the color of the embed.
        /// </summary>
        [Description("The color of the embed.")]
        public string EmbedColor { get; set; } = "#808080";
    }
}