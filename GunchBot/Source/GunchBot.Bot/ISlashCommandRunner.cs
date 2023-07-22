using Discord;
using Discord.WebSocket;

namespace GunchBot.Bot
{
    internal interface ISlashCommandRunner
    {
        /// <summary>
        /// The name to reference the command.
        /// </summary>
        string CommandName { get; }

        /// <summary>
        /// A user readable description of the command.
        /// </summary>
        string CommandDescription { get; }

        /// <summary>
        /// Builds the command to be used in <see cref="DiscordSocketClient.CreateGlobalApplicationCommandAsync"/>.
        /// </summary>
        /// <returns>A built <see cref="ApplicationCommandProperties"/>.</returns>
        ApplicationCommandProperties Build();

        Task RunCommand(SocketSlashCommand slashCommand);
    }
}
