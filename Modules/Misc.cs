using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Discord_BodmodBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        //Greets user
        [Command("hi")]
        public async Task Hi()
        {
            string response = "stop annoying me " + Context.User.Username;
            await Context.Channel.SendMessageAsync(response);
        }
        //Picks randomly from an assortment of choices seperated by 'or'
        [Command("pick")]
        public async Task PickOne([Remainder]string message)
        {
            string[] options = message.Split(new string[] { " or " }, StringSplitOptions.RemoveEmptyEntries);

            Random r = new Random();
            string selection = options[r.Next(0, options.Length)];

            string responce = "i choose " + selection;
            await Context.Channel.SendMessageAsync(responce);
        }
        //Gets the url of the avatar of selected user
        [Command("avatar")]
        public async Task Avatar(IGuildUser user)
        {
            string pic = user.GetAvatarUrl();
            await Context.Channel.SendMessageAsync(pic);
        }
        //DM's user help menu
        [Command("help")]
        public async Task Help ()
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            string message = "to make me do things @ me or use `*` \ncommands: ```*help \n*hi \n*pick [choice1] or [choice2] or... \n*avatar [user name] ```";
            await dmChannel.SendMessageAsync(message);
        }
        // encode w/ caesar and key
        [Command("caesar-e")]
        public async Task CaesarEncode(string message, string key_s)
        {
            string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int key_i = Int32.Parse(key_s);
            string encoded_string = "";

            for (int i = 0; i < message.Length; i++)
            {
                if (upperCase.Contains(message[i]))
                    encoded_string += (char)(((message[i] + key_i - 65) % 26) + 65);
                else if (lowerCase.Contains(message[i]))
                    encoded_string += (char)(((message[i] + key_i - 97) % 26) + 97);
                else
                    encoded_string += message[i];
            }
           
            await Context.Channel.SendMessageAsync(encoded_string);
        }
    }
}
