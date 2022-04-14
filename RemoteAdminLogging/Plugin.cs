// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace RemoteAdminLogging
{
    using System;
    using System.Reflection;
    using CommandSystem;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using HarmonyLib;
    using RemoteAdmin;
    using RemoteAdminLogging.Patches;

    /// <inheritdoc />
    public class Plugin : Plugin<Config, Translation>
    {
        private Harmony harmony;

        /// <summary>
        /// Gets a static instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static Plugin Instance { get; private set; }

        /// <inheritdoc/>
        public override string Author => "Build";

        /// <inheritdoc/>
        public override string Name => "RemoteAdminLogging";

        /// <inheritdoc/>
        public override string Prefix => "RemoteAdminLogging";

        /// <inheritdoc />
        public override PluginPriority Priority => PluginPriority.Last;

        /// <inheritdoc/>
        public override Version Version { get; } = new Version(1, 0, 0);

        /// <inheritdoc/>
        public override Version RequiredExiledVersion { get; } = new Version(5, 1, 3);

        /// <summary>
        /// Gets an instance of the <see cref="RemoteAdminLogging.WebhookController"/> class.
        /// </summary>
        public WebhookController WebhookController { get; private set; }

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            if (string.IsNullOrEmpty(Config.WebhookUrl))
            {
                Log.Error($"The webhook url cannot be empty! {Name} will not be loaded.");
                return;
            }

            Instance = this;

            WebhookController = new WebhookController(this);

            harmony = new Harmony($"raLogging.{DateTime.UtcNow.Ticks}");
            PatchCommands();

            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            harmony?.UnpatchAll(harmony.Id);
            harmony = null;

            WebhookController?.Dispose();
            WebhookController = null;

            Instance = null;
            base.OnDisabled();
        }

        private void PatchCommands()
        {
            foreach (ICommand command in CommandProcessor.RemoteAdminCommandHandler.AllCommands)
                PatchCommand(command);
        }

        private void PatchCommand(ICommand command)
        {
            if (command is ParentCommand parentCommand)
            {
                PatchParent(parentCommand);
                return;
            }

            harmony.Patch(command.GetType().GetMethod("Execute", BindingFlags.Public | BindingFlags.Instance), postfix: new HarmonyMethod(typeof(ProcessQueryPatch).GetMethod(nameof(ProcessQueryPatch.Postfix), BindingFlags.Public | BindingFlags.Static)));
        }

        private void PatchParent(ParentCommand parentCommand)
        {
            foreach (ICommand command in parentCommand.AllCommands)
                PatchCommand(command);
        }
    }
}