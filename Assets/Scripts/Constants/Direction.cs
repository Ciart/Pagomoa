using UnityEngine;

namespace Constants
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public static class DirectionUtility
    {
        private static readonly Vector2 BaseVector = new Vector2(1f, 1f);
        
        public static Direction ToDirection(Vector2 vector)
        {
            var signedAngle = Vector2.SignedAngle(BaseVector, vector);
            var angle = signedAngle < 0 ? 360 + signedAngle : signedAngle;

            return angle switch
            {  
                >= 0 and < 90 => Direction.Up,
                >= 90 and < 180 => Direction.Left,
                >= 180 and < 270 => Direction.Down,
                >= 270 and < 360 => Direction.Right,
                _ => Direction.Down
            };
        }
    }
}
