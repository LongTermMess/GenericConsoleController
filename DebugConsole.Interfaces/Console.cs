using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace DebugConsole.Interfaces
{
    public class ConsoleController
    {
        public List<object> Commands = new List<object>();
        public List<ConsoleAddon> Addons = new List<ConsoleAddon>();

        public List<ConsoleMessage> Messages { get; set; } = new List<ConsoleMessage>();
        public string Input { get; set; } = "";
        public string CommandPrefix { get; set; } = "";
        public bool EchoToReloaded { get; set; } = false;
        public ConsoleController()
        {

        }

        public void WriteLine(string message, string sender)
        {
            Messages.Add(new ConsoleMessage(message, sender));
            if (EchoToReloaded && sender != "Reloaded") { OnMessageWritten.Invoke("[DebugConsole] " + message); }
        }
        public event MessageWriteEvent OnMessageWritten;
        public delegate void MessageWriteEvent(string Message);

        public void ClearMessages()
        {
            Messages = new List<ConsoleMessage>();
        }

        
        public void RegisterAddon(ConsoleAddon addon)
        {
            for(int i = 0; i < addon.Commands.Count; i++)
            {
                Object tempCommand = addon.Commands[i];
                (tempCommand as CommandBase).Catagory = addon.Name;
                RegisterCommand(tempCommand);
            }
            Addons.Add(addon);
        }

        //Catagory is only used if registered manually like this, its replaced by addon name normally
        public void RegisterCommand(Object command)
        {
            Commands.Add(command);
        }

        public void ReceiveCommand(string Input, string Sender, bool EchoPrint = true)
        {
            if(CommandPrefix != "")
            {
                if(!Input.StartsWith(CommandPrefix)) { if (EchoPrint) { WriteLine(Input, Sender); return; } }
                else
                {
                    Input = Input.Substring(CommandPrefix.Length);

                }
            }

            if (EchoPrint){ WriteLine(Input, Sender);}

            if(Input.EndsWith(" ")) { Input = Input.Remove(Input.Length - 1);}
            List<string> FullMessage = Input.Split(" ").ToList<string>();
            string Command = FullMessage[0];
            List<string> Args = FullMessage; Args.RemoveAt(0);
            int FoundArgs = Args.Count();

            //I HAVE NO IDEA WHY I WROTE THIS LINE BUT IT BROKE EVERYTHING FOR AGES IT STAYS HERE AS A REMINDER TO NOT BE A MORON
            //if (FullMessage.Count() > 1) { FoundArgs = FullMessage.Count() - 1; }

            for (int i = 0; i < Commands.Count; i++)
            {
                CommandBase baseCommand = Commands[i] as CommandBase;

                //WriteLine(baseCommand.Name + "." + baseCommand.Arguments + "." + baseCommand + "|" + Input + "." + Sender + "." + FoundArgs, "Debug");

                if (string.Equals(Command, baseCommand.Name, StringComparison.CurrentCultureIgnoreCase) && baseCommand.Arguments == FoundArgs) 
                {

                    if (Commands[i] as Command != null)
                    {
                        (Commands[i] as Command).Invoke();
                        return;
                    }
                    else if (Commands[i] as Command<int> != null)
                    {
                        (Commands[i] as Command<int>).Invoke(Int32.Parse(Args[0]));
                        return;
                    }
                    else if (Commands[i] as Command<string> != null)
                    {
                        (Commands[i] as Command<string>).Invoke(Args[0]);
                        return;
                    }
                    else if (Commands[i] as Command<int, string> != null)
                    {
                        (Commands[i] as Command<int, string>).Invoke((int)Convert.ToInt32(Args[0]), Args[1]);
                        return;
                    }
                    else if (Commands[i] as Command<int, int> != null)
                    {
                        (Commands[i] as Command<int, int>).Invoke((int)Convert.ToInt32(Args[0]), (int)Convert.ToInt32(Args[1]));
                        return;
                    }
                    else if (Commands[i] as Command<string, string> != null)
                    {
                        (Commands[i] as Command<string, string>).Invoke(Args[0], Args[1]);
                        return;
                    }
                    else if (Commands[i] as Command<nuint> != null)
                    {
                        (Commands[i] as Command<nuint>).Invoke((nuint)Convert.ToInt64(Args[0], 16));
                        return;
                    }
                    else if (Commands[i] as Command<nuint, string> != null)
                    {
                        (Commands[i] as Command<nuint, string>).Invoke((nuint)Convert.ToInt64(Args[0], 16), Args[1]);
                        return;
                    }
                    else if (Commands[i] as Command<nuint, string, long> != null)
                    {
                        (Commands[i] as Command<nuint, string, long>).Invoke((nuint)Convert.ToInt64(Args[0], 16), Args[1], Convert.ToInt64(Args[2]));
                        return;
                    }


                }


            }
            if (EchoPrint){ WriteLine("Unknown Command or incorrect arguments", "ConsoleController"); }


        }


        public ConsoleMessage GetLastMessageFrom(string Sender)
        {
            for (int i = Messages.Count() - 1; i >= 0; i--)
            {
                if (Messages[i].Sender == Sender)
                {
                    return Messages[i];
                }
            }
            return new ConsoleMessage("Null", "Null");
        }








    }

    public class ConsoleMessage
    {
        public string MessageBody { get; }
        public DateTime TimeStamp { get; }
        public string Sender { get; }
        public ConsoleMessage(string Msg, string Source)
        {
            MessageBody = Msg;
            Sender = Source;
            TimeStamp = DateTime.Now;
        }
    }


    public class ConsoleAddon
    {
        public string Name { get; set; }
        public string Author { get; set; } = "";
        public List<object> Commands { get; set; } = new List<object>();
    }



    public class CommandBase
    {
        public string Name;
        public string ArgumentFormat = "";
        public string Description = "";
        public int Arguments;
        public bool HideFromHelp = false;
        public string Catagory;
    }

    public class Command : CommandBase
    {
        public Action CommandAction;
        public void Invoke()
        {
            CommandAction.Invoke();
        }
    }

    public class Command<T1> : CommandBase
    {

        public Action<T1> CommandAction;
        public void Invoke(T1 Value)
        {
            CommandAction.Invoke(Value);
        }
    }
    public class Command<T1, T2> : CommandBase
    {

        public Action<T1, T2> CommandAction;
        public void Invoke(T1 Value, T2 Value2)
        {
            CommandAction.Invoke(Value, Value2);
        }
    }

    public class Command<T1, T2, T3> : CommandBase
    {

        public Action<T1, T2, T3> CommandAction;
        public void Invoke(T1 Value, T2 Value2, T3 Value3)
        {
            CommandAction.Invoke(Value, Value2, Value3);
        }
    }

    public class Command<T1, T2, T3, T4> : CommandBase
    {

        public Action<T1, T2, T3, T4> CommandAction;
        public void Invoke(T1 Value, T2 Value2, T3 Value3, T4 Value4)
        {
            CommandAction.Invoke(Value, Value2, Value3, Value4);
        }
    }

}
