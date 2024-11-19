using System;
using Ciart.Pagomoa.Sounds;

namespace Ciart.Pagomoa.Entities.Players
{
    public partial class PlayerController
    {
        private SoundManager soundManager => SoundManager.instance;
        
        private void UpdateSound()
        {
            if (state == PlayerState.Walk && isGrounded && MathF.Abs(_rigidbody.velocity.x) > 0.1f)
            {
                if (!soundManager.FindAudioSource("TeamEffect").isPlaying)
                {
                    soundManager.PlaySfx("FootSteps", true);
                }
            }
        }
    }
}