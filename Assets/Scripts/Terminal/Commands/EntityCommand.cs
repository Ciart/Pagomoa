using System;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Time;
using UnityEngine;

namespace Ciart.Pagomoa.Terminal.Commands
{
    public static class EntityCommand
    {
        [RegisterCommand(Help = "Spawn Entity", MinArgCount = 1, MaxArgCount = 1)]
        private static void CommandSpawn(CommandArg[] args)
        {
            var entityId = args[0].String;
            
            if (Terminal.IssuedError) return;

            var entityManager = Game.Instance.Entity;
            var player = Game.Instance.player;

            entityManager.Spawn(entityId, player?.transform.position ?? Vector3.zero);
        }
    }
}