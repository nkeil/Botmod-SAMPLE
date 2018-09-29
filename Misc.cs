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

// GENERAL COMMANDS

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
        //Give user a role
        [Command("role")]
        public async Task Role(string rolename)
        {
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
            if (Context.Guild.Id == 473217646884552734)
            {
                SocketRole role;
                switch (rolename)
                {
                    case "1":
                    case "Cowell":
                        role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Cowell");
                        break;
                    case "2":
                    case "Stevenson":
                        role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Stevenson");
                        break;
                    case "3":
                    case "Crown":
                        role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Crown");
                        break;
                    case "4":
                    case "Merrill":
                        role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Merrill");
                        break;
                    case "5":
                    case "Porter":
                        role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Porter");
                        break;
                    case "6":
                    case "Kresge":
                        role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Kresge");
                        break;
                    case "7":
                    case "Oakes":
                        role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Oakes");
                        break;
                    case "8":
                    case "Rachel-Carson":
                        role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Rachel-Carson");
                        break;
                    case "9":
                    case "C9":
                        role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "C9");
                        break;
                    case "10":
                    case "C10":
                        role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "C10");
                        break;
                    default:
                        role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "member");
                        break;
                }
                await (Context.User as IGuildUser).AddRoleAsync(role);
            }
        }
        //Give user a year role
        [Command("year")]
        public async Task Year(string yearname)
        {
            if (Context.Guild.Id == 473217646884552734)
            {
                SocketRole year;
                switch (yearname)
                {
                    case "Frosh":
                    case "frosh":
                    case "Freshman":
                        year = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Frosh");
                        break;
                    case "Soph":
                    case "soph":
                    case "Sophmore":
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
                        year = Context.Guild.Roles.FirstOrDefault(x => x.Name == "member");
                        break;
                }
                await (Context.User as IGuildUser).AddRoleAsync(year);
            }
        }
        // add a series of values
        [Command("add")]
        public async Task Add([Remainder]string message)
        {
            string[] options = message.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            int answer = 0;

            for (int i = 0; i < options.Length; i++)
                answer += Int32.Parse(options[i]);

            await Context.Channel.SendMessageAsync(answer.ToString());
        }
        // do math
        [Command("solve")]
        public async Task Solve_init([Remainder]string message)
        {
            string[] expression = message.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            
            await Context.Channel.SendMessageAsync("The answer is: " + Solve(expression));
        }
        public double Solve(string[] expression)
        {
            //check for unequal parenthesis
            int open_paren_count = 0;
            int closed_paren_count = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == "(") open_paren_count++;
                if (expression[i] == ")") closed_paren_count++;
            }
            if (open_paren_count != closed_paren_count) return 0;

            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == "(")
                {
                    //finds endIndex of first paren
                    int endIndex = i;
                    int count = 1;
                    while (count > 0)
                    {
                        endIndex++;
                        if (expression[endIndex] == "(") count += 1;
                        else if (expression[endIndex] == ")") count += -1;
                    }

                    //create sub-expression of paren expression not including parens
                    string[] sub_expression = new string[endIndex - i - 1];
                    for (int j = i + 1, n = 0; j < endIndex; j++)
                    {
                        sub_expression[n++] = expression[j];
                        expression[j] = null;
                    }
                    expression[i] = null;
                    expression[endIndex] = null;

                    //overrite paren expression in list with result of paren expression
                    bool temp = true;
                    string[] overrite_expression = new string[expression.Length - sub_expression.Length - 1];
                    for (int j = 0, n = 0; j < expression.Length; j++)
                    {
                        if (expression[j] != null)
                            overrite_expression[n++] = expression[j];
                        else if (temp == true)
                        {
                            overrite_expression[n++] = "" + Solve(sub_expression);
                            temp = false;
                        }
                    }

                    return Solve(overrite_expression);
                }
            }

            return Solve_no_paren(expression);
        }

        // helper function for solve, needs an array with no parens
        public double Solve_no_paren(string[] expression)
        {
            double answer = double.Parse(expression[0]);

            for (int i = 1; i < expression.Length; i += 2)
            {
                switch (expression[i])
                {
                    case "+":
                        answer += double.Parse(expression[i + 1]);
                        break;
                    case "-":
                        answer -= double.Parse(expression[i + 1]);
                        break;
                    case "*":
                        answer *= double.Parse(expression[i + 1]);
                        break;
                    case "/":
                        answer /= double.Parse(expression[i + 1]);
                        break;
                    case "%":
                        answer %= double.Parse(expression[i + 1]);
                        break;
                    case "^":
                        answer = Math.Pow(answer, double.Parse(expression[i + 1]));
                        break;
                }
            }

            return answer;
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
