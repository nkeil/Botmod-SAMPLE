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

// ADMINISTRATION COMMANDS

namespace Discord_BodmodBot.Modules
{
    public class Admin: ModuleBase<SocketCommandContext>
    {
        Random _ran = new Random();

        //Kick a user
        [Command("kick"), RequireUserPermission(GuildPermission.KickMembers)]
        public async Task KickUser(IGuildUser user, [Remainder] string reason = "none")
        {
            await user.KickAsync();
            var dmChannel = await user.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync("kicked for: " + reason);
            await Context.Channel.SendMessageAsync($"kicked {user}");
        }

        //Mass remove messages, optional for a certain user
        [Command("prune"), RequireUserPermissionAttribute(GuildPermission.ManageMessages)]
        public async Task Prune(SocketGuildUser name = null)
        {
            IEnumerable<IMessage> msgs = null;

            if (name == null)
                msgs = await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, 100).Flatten();
            else
                msgs = (await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, 250).Flatten()).Where(x => x.Author.Id == name.Id).Take(250);

            await (Context.Channel as ITextChannel).DeleteMessagesAsync(msgs as IEnumerable<IMessage>);
            await Context.Message.DeleteAsync();

            EmbedBuilder builder = new EmbedBuilder()
            {
                Title = $"**{msgs.Count()} messages deleted!**",
                Color = new Color((byte)(_ran.Next(255)), (byte)(_ran.Next(255)), (byte)(_ran.Next(255)))
            }.WithCurrentTimestamp();

            await ReplyAsync(string.Empty, false, embed: builder.WithFooter(y => y.WithText($"{Context.User}")).Build());
        }
    }
}