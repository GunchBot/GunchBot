using Discord.WebSocket;

namespace GunchBot.Bot.Commands
{
    public interface ISlashCommandManager
    {
        /// <summary>
        /// Create and register commands.
        /// </summary>
        Task CreateCommands();

        /// <summary>
        /// Handles delegating commands based on command name.
        /// </summary>
        /// <param name="command">The command information.</param>
        Task HandleCommands(SocketSlashCommand command);
    }
}
