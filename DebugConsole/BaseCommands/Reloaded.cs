using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebugConsole.Interfaces;
using Reloaded.Mod.Interfaces;

namespace DebugConsole.BaseCommands
{
    internal class ReloadedCommands
    {
        private ConsoleController DebugConsole;
        private IModLoader _modLoader;

        public Command ListModsCommand;


        public ReloadedCommands(IModLoader modLoader, ConsoleController console) 
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


        }
        




    }
}
