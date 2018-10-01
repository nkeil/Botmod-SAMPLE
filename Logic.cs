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
    public class Logic : ModuleBase<SocketCommandContext>
    {
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
        //solve expressions with no parethesis
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
    }
}
