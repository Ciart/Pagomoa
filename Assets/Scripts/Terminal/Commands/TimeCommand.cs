using Ciart.Pagomoa.Systems.Time;

namespace Ciart.Pagomoa.Terminal.Commands
{
    public static class TimeCommand
    {
        [RegisterCommand(Help = "Change time tick", MinArgCount = 1, MaxArgCount = 2)]
        private static void CommandTick(CommandArg[] args)
        {
            var subCommand = args[0].String;
            
            var timeManager = Game.Get<TimeManager>();
            
            if (Terminal.IssuedError) return;
            
            switch (subCommand)
            {
                case "morning":
                    timeManager.tick = TimeManager.Morning;
                    break;
                case "set":
                    timeManager.tick = args[1].Int;
                    break;
                case "hour":
                    timeManager.SetTime(args[1].Int, 0);
                    break;
                case "minute":
                    timeManager.SetTime(timeManager.hour, args[1].Int);
                    break;
                default:
                    Terminal.Log(TerminalLogType.Error, "time {0} is not defined", subCommand);
                    break;
            }
        }
    }
}