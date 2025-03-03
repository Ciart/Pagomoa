using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;

namespace Ciart.Pagomoa.Terminal.Commands
{
    public static class SaveCommand
    {
        [RegisterCommand(Help = "")]
        private static void CommandSave(CommandArg[] args)
        {
            Game.Instance.Save.Save();
        }
        
        [RegisterCommand(Help = "")]
        private static void CommandLoad(CommandArg[] args)
        {
            Game.Instance.Save.Load();
        }
    }
}