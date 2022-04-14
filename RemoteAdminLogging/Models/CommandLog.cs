// -----------------------------------------------------------------------
// <copyright file="CommandLog.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace RemoteAdminLogging.Models
{
    using System;
    using Exiled.API.Features;

    /// <summary>
    /// Represents a log of an executed command.
    /// </summary>
    public readonly struct CommandLog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLog"/> struct.
        /// </summary>
        /// <param name="player">The player that executed the command.</param>
        /// <param name="query">The executed command.</param>
        public CommandLog(Player player, ArraySegment<string> query)
        {
            Name = player.Nickname;
            Id = player.UserId;
            Query = string.Join(" ", query.Array);
        }

        /// <summary>
        /// Gets the player's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the player's id.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the executed command.
        /// </summary>
        public string Query { get; }
    }
}