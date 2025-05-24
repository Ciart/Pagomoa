using System;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Terminal.Commands
{
    public static class WorldCommand
    {
        [RegisterCommand(Help = "Move to specified level", MinArgCount = 1, MaxArgCount = 1)]
        private static void CommandLevel(CommandArg[] args)
        {
            var levelId = args[0].String;

            if (Terminal.IssuedError) return;

            WorldManager.world.ChangeLevel(levelId);
        }
    }
}
