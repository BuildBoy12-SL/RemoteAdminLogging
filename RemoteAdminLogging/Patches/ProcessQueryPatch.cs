// -----------------------------------------------------------------------
// <copyright file="ProcessQueryPatch.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace RemoteAdminLogging.Patches
{
#pragma warning disable SA1313
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using RemoteAdminLogging.Models;

    /// <summary>
    /// Patches commands to log the query.
    /// </summary>
    public static class ProcessQueryPatch
    {
        /// <summary>
        /// Logs the successfully executed command.
        /// </summary>
        /// <param name="arguments">The command's arguments.</param>
        /// <param name="sender">The sender of the command.</param>
        /// <param name="__result">A value indicating whether the command executed successfully.</param>
        public static void Postfix(ArraySegment<string> arguments, ICommandSender sender, ref bool __result)
        {
            if (__result && Player.Get(sender) is Player player)
                Plugin.Instance.WebhookController.SendMessage(new CommandLog(player, arguments));
        }
    }
}