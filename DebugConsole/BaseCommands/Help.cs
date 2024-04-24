using DebugConsole.Interfaces;
using Reloaded.Mod.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using p5rDebugConsole.Configuration;

namespace DebugConsole.BaseCommands
{
    internal class HelpCommands
    {
        ConsoleController DebugConsole;
        Config _configuration;

        public Command HelpCommand;
        public Command<string> HelpCommandWithFind;
        public Command<string, string> HelpCommandWithTwoFinds;
        public HelpCommands(ConsoleController console, Config config)
        {
            DebugConsole = console;
            _configuration = config;

            void ListCommands()
            {
                for (int i = 0; i < DebugConsole.Commands.Count(); i++)
                {
                    CommandBase Command = DebugConsole.Commands[i] as CommandBase;
                    if (Command.HideFromHelp && !_configuration.ShowHiddenHelp) { continue; }

                    HelpPrint(Command);
                }
            }

            void ListCommandsCatagory(string Find)
            {
                for (int i = 0; i < DebugConsole.Commands.Count(); i++)
                {
                    CommandBase Command = DebugConsole.Commands[i] as CommandBase;
                    if (Command.HideFromHelp && !_configuration.ShowHiddenHelp) { continue; }

                    string TempFind = Find;

                    if(TempFind.StartsWith("#"))
                    {
                        TempFind = TempFind.Substring(1);
                        if (!Command.Catagory.Contains(TempFind, StringComparison.CurrentCultureIgnoreCase)) { continue; }
                    }
                    else
                    {
                        if (!Command.Name.Contains(TempFind, StringComparison.CurrentCultureIgnoreCase)) { continue; }
                    }

                    HelpPrint(Command);
                }
            }

            void ListCommandsCatagoryCommand(string Catagory, string Search)
            {
                if (!Catagory.StartsWith("#")) { DebugConsole.WriteLine("Must search for catagory then command name", "ConsoleController"); }

                for (int i = 0; i < DebugConsole.Commands.Count(); i++)
                {
                    CommandBase Command = DebugConsole.Commands[i] as CommandBase;
                    if (Command.HideFromHelp && !_configuration.ShowHiddenHelp) { continue; }

                    string SearchCatagory = Catagory.Substring(1);

                    if (!Command.Catagory.Contains(SearchCatagory, StringComparison.CurrentCultureIgnoreCase)) { continue; }

                    if (!Command.Name.Contains(Search, StringComparison.CurrentCultureIgnoreCase)) { continue; }


                    HelpPrint(Command);
                }
            }

            void HelpPrint(CommandBase Command)
            {
                string Temp = Command.Catagory + " | " + Command.Name;
                if (Command.ArgumentFormat != "") { Temp = Temp + " | " + Command.ArgumentFormat; }
                if (Command.Description != "") { Temp = Temp + " | " + Command.Description; }
                DebugConsole.WriteLine(Temp, "HelpCommandOutput");
            }




            HelpCommand = new Command()
            {
                Name = "Help",
                Description = "",
                Arguments = 0,
                Catagory = "Default Commands",
                HideFromHelp = true,
                CommandAction = ListCommands
            };

            HelpCommandWithFind = new Command<string>()
            {
                Name = "Help",
                ArgumentFormat = "(Search Term)",
                Catagory = "Default Commands",
                HideFromHelp = true,
                Description = "Prints all registeed commands that match search, start search term with a \"#\" to search catagory, leave empty to list all.",
                Arguments = 1,
                CommandAction = ListCommandsCatagory
            };
            HelpCommandWithTwoFinds = new Command<string, string>()
            {
                Name = "Help",
                ArgumentFormat = "(Command Name/Addon Name, Command name in addon)",
                Catagory = "Default Commands",
                Description = "Print details of commands. Start search with \"#\" to search addon, then you can add another word to then filter by name, leave empty to list all.",
                Arguments = 2,
                CommandAction = ListCommandsCatagoryCommand
            };

        }
    }
}
