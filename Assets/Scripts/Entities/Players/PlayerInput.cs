using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities.Players
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

        private void Awake()
        {
            Actions = new InputActions().Player;
            UIActions = new InputActions().PlayerUI;
        }
        
        private void Update()
        {
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
        }
        private void OnDisable()
        {
            Actions.Disable();
        }
    }
}