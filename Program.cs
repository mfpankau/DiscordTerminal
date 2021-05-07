using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

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
            int writeMode = 0;
            string fileDir = "";
            //token 
            //<Token>
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
                if (!e.Message.Author.IsBot)
                {
                    char[] args = FilterArgs(e.Message.Content).ToArray();
                    //writing to file
                    if (writeMode == 1)
                    {
                        Console.WriteLine(fileDir);
                        File.WriteAllText(fileDir, e.Message.Content);
                        writeMode = 0;
                        fileDir = "";
                    }
                    /*if(writeMode == 2)
                    {
                        File.WriteAllBytes(fileDir, e.Message.Content);
                        writeMode = 0;
                    }*/
                    //ls command
                    if (e.Message.Content.StartsWith("ls"))
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
                    if (e.Message.Content.StartsWith("cd"))
                    {
                        string address = e.Message.Content.Replace("cd ", "");
                        if (address == "..")
                        {
                            RemoveLastDir();
                        }
                        else
                        {
                            curDir += '\\' + address;
                        }
                        discord.SendMessageAsync(e.Message.Channel, "**Current Directory Set to: **" + curDir);
                    }
                    //pwd command
                    if (e.Message.Content.StartsWith("pwd"))
                    {
                        discord.SendMessageAsync(e.Message.Channel, "**Current Directory is: **" + curDir);
                    }
                    //cat command
                    if (e.Message.Content.StartsWith("cat"))
                    {
                        if(args.length < 1)
                        {
                            discord.SendMessageAsync(e.Message.Channel, 
                                "**Incorrect Usage!**\n" +
                                "**Please input the command in the form:**\n" +
                                "cat <mode(!t, !b)> <path to file>");
                        }
                        else
                        {
                            if(args[0] == 't')
                            {
                                data = File.ReadAllText(curDir + '\\' + e.Message.Content.Replace("cat !t ", ""));
                                discord.SendMessageAsync(e.Message.Channel, data);
                            }
                            else if(args[0] == 'b')
                            {
                                //read as bytes
                                data = ReadBytes(e.Message.Content);
                                discord.SendMessageAsync(e.Message.Channel, "**Length of file in bytes: **" + bytes.Length.ToString() + '\n' + "**First 500 bytes of data: **" + data);
                            }
                            
                            data = "";
                        }
                    }
                    //touch command
                    if (e.Message.Content.StartsWith("touch"))
                    {
                        //deal with issues
                        if (args.Length < 1)
                        {
                            discord.SendMessageAsync(e.Message.Channel,
                                "**Incorrect Usage!**\n" +
                                "**Please input the command in the form:**\n" +
                                "touch <write mode(!t, !b)> <access parameter(!w, !a)> <name of new file>");
                        }
                        else
                        {
                            if (args[0] == 'b')
                            {
                                writeMode = 2; //write as byte
                                fileDir = curDir + "\\" + e.Message.Content.Replace("touch !b ", "");
                            }
                            else if (args[0] == 't')
                            {
                                writeMode = 1; //write as plaintext
                                fileDir = curDir + "\\" + e.Message.Content.Replace("touch !t ", "");
                            }
                            discord.SendMessageAsync(e.Message.Channel, "**Next message will be written to file in selected mode.**");
                        }
                    }
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
                if (chars[i] == '\\')
                {
                    curDir = curDir.Substring(0, i);
                    return;
                }
            }
        }
        static List<char> FilterArgs(string input)
        {
            char[] chars = input.ToCharArray();
            List<char> args = new List<char>();
            for (int i = 0; i < chars.Length; i++)
            {
                if(chars[i] == '!')
                {
                    args.Add(chars[i + 1]);
                }
            }
            return args;
        }
        static string ReadBytes(string input)
        {
            string data = "";
            byte[] bytes = File.ReadAllBytes(curDir + '\\' + input.Replace("cat !b ", ""));
            for (int i = 0; i < 500; i++)
            {
                if (i >= bytes.Length)
                {
                    break;
                }
                data += bytes[i].ToString();
            }
            return data;
        }
    }
}
