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

            var inventory = Game.instance.player.inventory;
            
            if (Terminal.IssuedError) return;
            
            Debug.Log($"giving item {ResourceSystem.instance.GetItem(subCommand).name}");

            var itemCount1 = 2369;
            /*var itemCount2 = 1;*/
            
            inventory.AddInventory(subCommand, itemCount1);
        }
        
        [RegisterCommand(Help = "", MinArgCount = 1, MaxArgCount = 2)]
        private static void CommandUse(CommandArg[] args)
        {
            var subCommand = args[0].String;
            
            if (Terminal.IssuedError) return;
            
            ResourceSystem.instance.GetItem(subCommand).DisplayUseEffect();
        }
    }
}
