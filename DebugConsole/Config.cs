using p5rDebugConsole.Template.Configuration;
using Reloaded.Mod.Interfaces.Structs;
using System.ComponentModel;

namespace p5rDebugConsole.Configuration
{
    public class Config : Configurable<Config>
    {

        [DisplayName("Show Hidden commands in help")]
        [DefaultValue(false)]
        public bool ShowHiddenHelp { get; set; } = false;

        [DisplayName("Echo reloaded console")]
        [DefaultValue(false)]
        public bool ReloadedConsole { get; set; } = false;

    }

    /// <summary>
    /// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
    /// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
    /// </summary>
    public class ConfiguratorMixin : ConfiguratorMixinBase
    {
        // 
    }
}
