using DebugConsole.Interfaces;
using Reloaded.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DebugConsole.BaseCommands
{
    internal class ProgramCommands
    {
        ConsoleController DebugConsole;

        public Command<nuint, string> ReadMemoryCommand;
        public Command<nuint, string, long> WriteMemoryCommand;
        public Command ExitCommand;
        public Command TimeCommand;
        public Command ClearCommand;


        public ProgramCommands(ConsoleController console)
        {
            DebugConsole = console;

            void ReadMemory(nuint Address, string Type)
            {
                Memory memory = Memory.Instance;
                long Value = 0;

                if (Type == "BYTE")
                {
                    Value = memory.Read<byte>(Address);
                }
                else if (Type == "SHORT")
                {
                    Value = memory.Read<short>(Address);
                }
                else if (Type == "INT")
                {
                    Value = memory.Read<int>(Address);
                }
                else if (Type == "LONG")
                {
                    Value = memory.Read<long>(Address);
                }
                else
                {
                    DebugConsole.WriteLine("Incorrect type", "ConsoleController"); return;
                }


                DebugConsole.WriteLine("Value at " + Address.ToString("X8") + " is " + Value, "ConsoleController");
            }
            void WriteMemory(nuint Address, string Type, long Value)
            {
                Memory memory = Memory.Instance;

                if (Type == "BYTE")
                {
                    memory.Write<byte>(Address, (byte)Value);
                }
                else if (Type == "SHORT")
                {
                    memory.Write<short>(Address, (short)Value);
                }
                else if (Type == "INT")
                {
                    memory.Write<int>(Address, (int)Value);
                }
                else if (Type == "LONG")
                {
                    memory.Write<long>(Address, Value);
                }
                else
                {
                    DebugConsole.WriteLine("Incorrect type", "ConsoleController"); return;
                }


                DebugConsole.WriteLine("Wrote value " + Value + " at " + Address.ToString("X8"), "ConsoleController");
            }
            ReadMemoryCommand = new Command<nuint, string>()
            {
                Name = "ReadMemory",
                ArgumentFormat = "(Address, Type)",
                Description = "Reads memory at specified address. Type accepts BYTE, SHORT, INT and LONG.",
                Arguments = 2,
                CommandAction = ReadMemory,
                Catagory = "Default Commands"
            };
            WriteMemoryCommand = new Command<nuint, string, long>()
            {
                Name = "WriteMemory",
                ArgumentFormat = "(Address, Type, Value)",
                Description = "Writes memory at specified address. Type accepts BYTE, SHORT, INT and LONG.",
                Arguments = 3,
                CommandAction = WriteMemory,
                Catagory = "Default Commands"
            };

            void Exit() { Environment.Exit(0); }
            ExitCommand = new Command()
            {
                Name = "Quit",
                Description = "Closes the game.",
                Arguments = 0,
                CommandAction = Exit,
                Catagory = "Default Commands"
            };

            TimeCommand = new Command()
            {
                Name = "PrintTime",
                Description = "",
                Arguments = 0,
                HideFromHelp = true,
                CommandAction = PrintTime,
                Catagory = "Default Commands"
            };
            void PrintTime(){DebugConsole.WriteLine(DateTime.UtcNow.Ticks.ToString(), "ConsoleController");}
            ClearCommand = new Command()
            {
                Name = "Clear",
                Description = "Clears all messages from console",
                Arguments = 0,
                CommandAction = DebugConsole.ClearMessages,
                Catagory = "Default Commands"
            };




        }



    }
}
