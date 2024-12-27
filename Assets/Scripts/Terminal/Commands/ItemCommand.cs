using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Terminal.Commands
{
    public static class ItemCommand
    {
        [RegisterCommand(Help = "", MinArgCount = 1, MaxArgCount = 2)]
        private static void CommandGive(CommandArg[] args)
        {
            var subCommand = args[0].String;

            var inventory = GameManager.instance.player.inventory;
            
            if (Terminal.IssuedError) return;
            
            Debug.Log($"giving item {ResourceSystem.instance.GetItem(subCommand).name}");
            
            inventory.Add(ResourceSystem.instance.GetItem(subCommand));
        }
        
        [RegisterCommand(Help = "", MinArgCount = 1, MaxArgCount = 2)]
        private static void CommandUse(CommandArg[] args)
        {
            var subCommand = args[0].String;
            
            if (Terminal.IssuedError) return;
            
            ResourceSystem.instance.GetItem(subCommand).Use();
        }
    }
}
