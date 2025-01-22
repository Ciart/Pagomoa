using Ciart.Pagomoa.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ciart.Pagomoa.Entities.Players
{
    public class PlayerInput : MonoBehaviour
    {
        public InputActions.PlayerActions Actions;
        public InputActions.PlayerUIActions UIActions;

        public Vector2 Move { get; private set; }
        
        public Vector2 Look { get; private set; }
        
        public Vector2 DigDirection { get; private set; }
        
        public bool IsDig { get; private set; }
        
        public bool IsJump { get; private set; }
        
        public bool IsClimb { get; private set; }
        
        public bool IsInteraction { get; private set; }

        public bool Actable;

        private void Awake()
        {
            Actions = new InputActions().Player;
            UIActions = new InputActions().PlayerUI;
            Actable = true;
        }
        
        private void Update()
        {
            if (!Actable) return;

            Move = Actions.Move.ReadValue<Vector2>();
            Look = Actions.Look.ReadValue<Vector2>();
            DigDirection = Actions.DigDirection.ReadValue<Vector2>();
            IsDig = Actions.Dig.IsPressed();
            IsJump = Actions.Jump.IsPressed();
            IsClimb = Actions.Climb.IsPressed();
            IsInteraction = Actions.Interaction.IsPressed();
        }

        private void OnEnable()
        {
            Actions.Enable();
            EventSystem.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        private void OnDisable()
        {
            Actions.Disable();
            EventSystem.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
        }

        private void OnPlayerSpawned(PlayerSpawnedEvent e)
        {
            var player = e.player;

            var playerInput = player.GetComponent<PlayerInput>();

            playerInput.Actions.Inventory.performed += context => { Actable = !Actable; StopAct(); };
        }

        private void StopAct()
        {
            Move = Vector2.zero;
            Look = Vector2.zero;
            DigDirection = Vector2.zero;
            IsDig = false;
            IsJump = false;
            //IsClimb = false;
            IsInteraction = false;
        }
    }
}