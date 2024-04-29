using p5rDebugConsole.Configuration;
using p5rDebugConsole.Template;
using Reloaded.Hooks.Definitions.X64;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using System.Runtime.InteropServices;
using Reloaded.Memory.Pointers;
using Reloaded.Memory;
using Reloaded.Memory.Utilities;
using System.Text;
using System.Net.Sockets;
using DebugConsole.Interfaces;
using static p5rDebugConsole.Mod;
using System.Xml;
using System.Windows.Input;
using DebugConsole.BaseCommands;
using System.Drawing;



namespace p5rDebugConsole
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : ModBase // <= Do not Remove.
    {
        private readonly IModLoader _modLoader;
        private readonly IReloadedHooks? _hooks;
        private readonly ILogger _logger;
        private readonly IMod _owner;
        private Config _configuration;
        private readonly IModConfig _modConfig;

        public ConsoleController DebugConsole;
        HelpCommands _helpCommands;

        public Mod(ModContext context)
        {
            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _logger = context.Logger;
            _owner = context.Owner;
            _configuration = context.Configuration;
            _modConfig = context.ModConfig;

            _logger.OnWriteLine += _logger_OnWriteLine;

            DebugConsole = new ConsoleController();
            DebugConsole.EchoToReloaded = _configuration.EchoDebug;
            DebugConsole.OnMessageWritten += _logger.WriteLine;

            ProgramCommands _programCommands = new ProgramCommands(DebugConsole);
            _helpCommands = new HelpCommands(DebugConsole); _helpCommands.HelpShowHidden = _configuration.ShowHiddenHelp;
            ModCommands _reloadedCommands = new ModCommands(_modLoader, DebugConsole);

            List<Object> BaseCommands = new List<Object>
            {
                _programCommands.WriteMemoryCommand,
                _programCommands.ReadMemoryCommand,
                _programCommands.ExitCommand,
                _programCommands.TimeCommand,
                _programCommands.ClearCommand,
                _helpCommands.HelpCommand,
                _helpCommands.HelpCommandWithFind,
                _helpCommands.HelpCommandWithTwoFinds,
                _reloadedCommands.ListModsCommand,
                _reloadedCommands.ListAddonsCommand,
            };

            ConsoleAddon BaseAddon = new ConsoleAddon()
            {
                Name = "BaseCommands",
                Author = "Cornflakes",
                Commands = BaseCommands
            };

            DebugConsole.RegisterAddon(BaseAddon);

            _modLoader.AddOrReplaceController<IConsoleController>(_owner, new ConsoleControl() { ConsoleController = DebugConsole});
        }

        private void _logger_OnWriteLine(object? sender, (string text, Color color) e)
        {
            if (_configuration.EchoReloaded && !e.text.StartsWith("[DebugConsole]")) { DebugConsole.WriteLine(e.text, "Reloaded"); }
        }

     
        

        public class Exports : IExports
        {
            public Type[] GetTypes() => new[] { typeof(IConsoleController) };
        }

        internal class ConsoleControl : IConsoleController
        {
            public ConsoleController ConsoleController { get; set; }
        }













        #region Standard Overrides
        public override void ConfigurationUpdated(Config configuration)
        {
            _helpCommands.HelpShowHidden = _configuration.ShowHiddenHelp;
            DebugConsole.EchoToReloaded = _configuration.EchoDebug;

            _configuration = configuration;
            _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
        }
        #endregion

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
    

    





}