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
            
            Debug.Log($"giving item {ResourceManager.instance.items[subCommand].name}");
            
            inventory.Add(ResourceManager.instance.items[subCommand]);
        }
        
        [RegisterCommand(Help = "", MinArgCount = 1, MaxArgCount = 2)]
        private static void CommandUse(CommandArg[] args)
        {
            var subCommand = args[0].String;
            
            if (Terminal.IssuedError) return;
            
            ResourceManager.instance.UseItem(ResourceManager.instance.items[subCommand]);
        }
    }
}