using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        private InputActions _actions;

        public Vector2 Move { get; private set; }
        
        public bool IsDig { get; private set; }
        
        public bool IsJump { get; private set; }
        
        public bool IsClimb { get; private set; }
        
        public bool IsInteraction { get; private set; }

        private void Awake()
        {
            _actions = new InputActions();
        }
        
        private void Update()
        {
            Move = _actions.Player.Move.ReadValue<Vector2>();
            IsDig = _actions.Player.Dig.IsPressed();
            IsJump = _actions.Player.Jump.IsPressed();
            IsClimb = _actions.Player.Climb.IsPressed();
            IsInteraction = _actions.Player.Interaction.IsPressed();
        }
        
        private void OnEnable()
        {
            _actions.Player.Enable();
        }

        private void OnDisable()
        {
            _actions.Player.Disable();
        }
    }
}