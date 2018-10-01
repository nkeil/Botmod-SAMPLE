using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using System.Diagnostics;
using System.Threading;

namespace Discord_BodmodBot.Modules
{
    public class IAM : ModuleBase<SocketCommandContext>
    {
        Random _ran = new Random();

        // Gives user Tan-Ultra (mute role)
        [Command("tanUltra"), RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task TanUltra(SocketGuildUser arg, [Remainder]string reason = null)
        {
            await Context.Message.DeleteAsync();

            var tanUltraRole = Context.Guild.Roles.FirstOrDefault(y => y.Name == "Tan-Ultra (Muted)");
            await (arg as IGuildUser).AddRoleAsync(tanUltraRole);

            EmbedBuilder builder = new EmbedBuilder()
            {
                Title = $"**{arg.Username} has been placed under Tan-Ultra**",
                Description = $"{(string.IsNullOrEmpty(reason) ? string.Empty : $"**Reason: {reason}**")}",
                Color = new Color((byte)(_ran.Next(255)), (byte)(_ran.Next(255)), (byte)(_ran.Next(255)))
            }.WithCurrentTimestamp();

            await ReplyAsync(string.Empty, false, embed: builder.WithFooter(y => y.WithText($"{Context.User}")).Build());

            EmbedBuilder builder2 = new EmbedBuilder()
            {
                Description = $"**You've been muted in {Context.Guild.Name}**{(string.IsNullOrEmpty(reason) ? string.Empty : $"\n**Reason: {reason}**")}",
                Color = new Color((byte)(_ran.Next(255)), (byte)(_ran.Next(255)), (byte)(_ran.Next(255)))
            }.WithCurrentTimestamp();

            IDMChannel x = await arg.GetOrCreateDMChannelAsync();
            builder.WithCurrentTimestamp();
            await x.SendMessageAsync(string.Empty, false, embed: builder2.Build());
        }
    }
}