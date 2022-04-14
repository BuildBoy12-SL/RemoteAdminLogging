// -----------------------------------------------------------------------
// <copyright file="WebhookController.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace RemoteAdminLogging
{
    using System;
    using DSharp4Webhook.Core;
    using DSharp4Webhook.Core.Constructor;
    using Exiled.API.Features;
    using RemoteAdminLogging.Models;

    /// <summary>
    /// Handles the sending of messages via a webhook.
    /// </summary>
    public class WebhookController : IDisposable
    {
        private static readonly EmbedBuilder EmbedBuilder = ConstructorProvider.GetEmbedBuilder();
        private static readonly EmbedFieldBuilder FieldBuilder = ConstructorProvider.GetEmbedFieldBuilder();
        private static readonly MessageBuilder MessageBuilder = ConstructorProvider.GetMessageBuilder();
        private readonly Plugin plugin;
        private readonly IWebhook webhook;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebhookController"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public WebhookController(Plugin plugin)
        {
            this.plugin = plugin;
            webhook = WebhookProvider.CreateStaticWebhook(plugin.Config.WebhookUrl);
        }

        /// <summary>
        /// Sends a message via the <see cref="webhook"/>.
        /// </summary>
        /// <param name="commandLog">The log to log.</param>
        public void SendMessage(CommandLog commandLog)
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(WebhookController));

            MessageBuilder messageBuilder = PrepareMessage(commandLog);
            if (messageBuilder is null)
                return;

            webhook.SendMessage(messageBuilder.Build()).Queue((result, isSuccessful) =>
            {
                if (!isSuccessful)
                    Log.Warn("Failed to send message.");
            });
        }

        /// <inheritdoc />
        public void Dispose()
        {
            isDisposed = true;
            webhook?.Dispose();
        }

        private static string Codeline(string line) => $"```{line}```";

        private MessageBuilder PrepareMessage(CommandLog commandLog)
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(WebhookController));

            EmbedBuilder.Reset();
            FieldBuilder.Reset();
            MessageBuilder.Reset();

            FieldBuilder.Inline = false;

            FieldBuilder.Name = plugin.Translation.User;
            FieldBuilder.Value = Codeline(commandLog.Name);
            EmbedBuilder.AddField(FieldBuilder.Build());

            if (plugin.Config.ShowUserId)
            {
                FieldBuilder.Name = plugin.Translation.UserId;
                FieldBuilder.Value = Codeline(commandLog.Id);
                EmbedBuilder.AddField(FieldBuilder.Build());
            }

            FieldBuilder.Name = plugin.Translation.CommandExecuted;
            FieldBuilder.Value = Codeline(commandLog.Query);
            EmbedBuilder.AddField(FieldBuilder.Build());

            EmbedBuilder.Title = plugin.Translation.Header;
            EmbedBuilder.Color = (uint)DSharp4Webhook.Util.ColorUtil.FromHex(plugin.Config.EmbedColor);
            EmbedBuilder.Timestamp = DateTimeOffset.UtcNow;
            MessageBuilder.AddEmbed(EmbedBuilder.Build());

            return MessageBuilder;
        }
    }
}