using System;

namespace Ciart.Pagomoa.Entities.Players
{
    public partial class PlayerController
    {
        private void UpdateSound()
        {
            if (state == PlayerState.Walk && isGrounded && MathF.Abs(_rigidbody.velocity.x) > 0.1f)
            {
                if (!_audioSource.isPlaying)
                {
                    _audioSource.Play();
                }
            }
            else
            {
                if (_audioSource.isPlaying)
                {
                    _audioSource.Stop();
                }
            }
        }
    }
}