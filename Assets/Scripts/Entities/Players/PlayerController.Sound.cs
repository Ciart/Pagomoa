using System;
using Ciart.Pagomoa.Sounds;
using Ciart.Pagomoa.Systems;

namespace Ciart.Pagomoa.Entities.Players
{
    public partial class PlayerController
    {
        private void UpdateSound()
        {
            SoundManager sound = Game.Instance.Sound;
            if (state == PlayerState.Walk && isGrounded && MathF.Abs(_rigidbody.linearVelocity.x) > 0.1f)
            {
                if (!sound.controller.GetPlayerSource().isPlaying)
                {
                    sound.PlaySfx("FootSteps", true);
                }
            }
        }
    }
}