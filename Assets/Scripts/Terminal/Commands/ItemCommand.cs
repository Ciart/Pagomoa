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

            var inventory = GameManager.player.inventory;
            
            if (Terminal.IssuedError) return;
            
            Debug.Log($"giving item {GameManager.instance.items[subCommand].name}");
            
            inventory.Add(GameManager.instance.items[subCommand]);
        }
        
        [RegisterCommand(Help = "", MinArgCount = 1, MaxArgCount = 2)]
        private static void CommandUse(CommandArg[] args)
        {
            var subCommand = args[0].String;
            
            if (Terminal.IssuedError) return;
            
            GameManager.UseItem(GameManager.instance.items[subCommand]);
        }
    }
}