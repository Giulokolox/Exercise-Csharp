using Aiv.Fast2D;
using OpenTK;

namespace Tankz_2021
{
    class MoveToPointBehaviour : CameraBehaviour
    {
        protected float counter;
        protected float duration;
        protected Vector2 cameraStartPosition;

        public MoveToPointBehaviour(Camera camera) : base(camera)
        {
        }

        public virtual void MoveTo(Vector2 point, float movementDuration)
        {
            cameraStartPosition = camera.position;
            pointToFollow = point;
            duration = movementDuration;
            counter = 0;
            blendFactor = 0;
        }

        public override void Update()
        {
            counter += Game.DeltaTime;

            if(counter >= duration)
            {//animation ended
                counter = duration;
                CameraMngr.OnMovementEnd();//go to follow target behaviour
            }

            blendFactor = counter / duration;

            camera.position = Vector2.Lerp(cameraStartPosition, pointToFollow, blendFactor);

        }

    }
}
