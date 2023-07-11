namespace GunchBot.Modules
{
    using Discord.Commands;

    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("<:poggies:885310337602175037>");
        }

        [Command("currentweather")]
        public async Task CurrentWeather(string location, string unit)
        {
            await ReplyAsync($"It's like 45{unit} in {location} idk man");
        }
    }
}
