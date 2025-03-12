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
            SaveSystem.Instance.Save();
        }
        
        [RegisterCommand(Help = "")]
        private static void CommandLoad(CommandArg[] args)
        {
            SaveSystem.Instance.Load();
        }
    }
}
