using System;
using Ciart.Pagomoa.Sounds;

namespace Ciart.Pagomoa.Entities.Players
{
    public partial class PlayerController
    {
        private void UpdateSound()
        {
            if (state == PlayerState.Walk && isGrounded && MathF.Abs(_rigidbody.velocity.x) > 0.1f)
            {
                if (!SoundManager.instance.FindAudioSource("TeamEffect").isPlaying)
                {
                    SoundManager.instance.PlaySfx("FootSteps", true);
                }
            }
        }
    }
}