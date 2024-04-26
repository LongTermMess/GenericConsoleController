using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebugConsole.Interfaces;
using Reloaded.Mod.Interfaces;

namespace DebugConsole.BaseCommands
{
    internal class ModCommands
    {
        private ConsoleController DebugConsole;
        private IModLoader _modLoader;

        public Command ListModsCommand;
        public Command ListAddonsCommand;


        public ModCommands(IModLoader modLoader, ConsoleController console) 
        {
            _modLoader = modLoader;
            DebugConsole = console;

            

            void ListMods()
            {

                for (int i = 0; i < _modLoader.GetActiveMods().Count(); i++)
                {
                    DebugConsole.WriteLine(_modLoader.GetActiveMods()[i].Generic.ModName + " | " + _modLoader.GetActiveMods()[i].Generic.ModId + " | By " + _modLoader.GetActiveMods()[i].Generic.ModAuthor, "ConsoleController");
                }
            }

            ListModsCommand = new Command()
            {
                Name = "ListMods",
                Description = "Prints all loaded reloaded mods and their authors.",
                Arguments = 0,
                CommandAction = ListMods,
                Catagory = "Default Commands"
            };

            void ListAddons()
            {
                for (int i = 0; i < DebugConsole.Addons.Count(); i++)
                {
                    DebugConsole.WriteLine(DebugConsole.Addons[i].Name + " | By " + DebugConsole.Addons[i].Author, "ConsoleController");
                }
            }
            ListAddonsCommand = new Command()
            {
                Name = "ListAddons",
                Description = "Prints all registered addons.",
                Arguments = 0,
                CommandAction = ListAddons,
                Catagory = "Default Commands"
            };

        }
        




    }
}
