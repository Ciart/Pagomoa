using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        public InputActions.PlayerActions Actions;

        public Vector2 Move { get; private set; }
        
        public bool IsDig { get; private set; }
        
        public bool IsJump { get; private set; }
        
        public bool IsClimb { get; private set; }
        
        public bool IsInteraction { get; private set; }

        private void Awake()
        {
            Actions = new InputActions().Player;
        }
        
        private void Update()
        {
            Move = Actions.Move.ReadValue<Vector2>();
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