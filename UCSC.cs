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
    public class UCSC : ModuleBase<SocketCommandContext>
    {
        //Give joining users a role for their major
        [Command("major")]
        public async Task Major(string rolename)
        {
            //only works in welcome channel
            if (Context.Channel.Id == 473248037951766563)
            {
                var user = Context.User;
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == rolename);
                var memberRole = Context.Guild.Roles.FirstOrDefault(x => x.Name == "member");
                await (user as IGuildUser).AddRoleAsync(role);
                await (user as IGuildUser).AddRoleAsync(memberRole);

                var channel = Context.Client.GetChannel(473217646884552736) as SocketTextChannel;
                await channel.SendMessageAsync($"{user.Mention} has joined the server :)");
            }
        }
        //Role for college
        [Command("college")]
        public async Task College(string rolename)
        {
            SocketRole role;
            switch (rolename)
            {
                case "1":
                case "Cowell":
                case "cowell":
                    role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Cowell");
                    break;
                case "2":
                case "Stevenson":
                case "stevenson":
                    role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Stevenson");
                    break;
                case "3":
                case "Crown":
                case "crown":
                    role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Crown");
                    break;
                case "4":
                case "Merrill":
                case "merrill":
                    role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Merrill");
                    break;
                case "5":
                case "Porter":
                case "porter":
                    role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Porter");
                    break;
                case "6":
                case "Kresge":
                case "kresge":
                    role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Kresge");
                    break;
                case "7":
                case "Oakes":
                case "oakes":
                    role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Oakes");
                    break;
                case "8":
                case "Rachel-Carson":
                case "rachel-carson":
                case "rc":
                case "RC":
                    role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Rachel-Carson");
                    break;
                case "9":
                case "C9":
                case "College 9":
                case "college 9":
                    role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "C9");
                    break;
                case "10":
                case "C10":
                case "College 10":
                case "college 10":
                    role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "C10");
                    break;
                default:
                    role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "INVALID_ROLE");
                    break;
            }
            await (Context.User as IGuildUser).AddRoleAsync(role);
        }
        //Give joining users a role (either CS or CS:GD)
        [Command("year")]
        public async Task Year(string yearname)
        {
            //only works in specified server
            if (Context.Guild.Id == 473217646884552734)
            {
                SocketRole year;
                switch (yearname)
                {
                    case "Frosh":
                    case "frosh":
                    case "Freshman":
                    case "freshman":
                        year = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Frosh");
                        break;
                    case "Soph":
                    case "soph":
                    case "Sophmore":
                    case "sophmore":
                        year = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Soph");
                        break;
                    case "Junior":
                    case "junior":
                        year = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Junior");
                        break;
                    case "Senior":
                    case "senior":
                        year = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Senior");
                        break;
                    default:
                        year = Context.Guild.Roles.FirstOrDefault(x => x.Name == "INVALID_ROLE");
                        break;
                }
                await (Context.User as IGuildUser).AddRoleAsync(year);
            }
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
