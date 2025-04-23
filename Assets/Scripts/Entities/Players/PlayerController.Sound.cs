using System;
using Ciart.Pagomoa.Sounds;
using Ciart.Pagomoa.Systems;

namespace Ciart.Pagomoa.Entities.Players
{
    public partial class PlayerController
    {
        private void UpdateSound()
        {
            if (state == PlayerState.Walk && isGrounded && MathF.Abs(_rigidbody.linearVelocity.x) > 0.1f)
            {
                if (!Game.Instance.Sound.FindAudioSource("PlayerEffect").isPlaying)
                {
                    Game.Instance.Sound.PlaySfx("FootSteps", true);
                }
            }
        }
    }
}