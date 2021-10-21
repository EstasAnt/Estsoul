namespace Character.Control
{
    public class DeviceInputController : InputController
    {

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Move()
        {
            var axis = SimpleJoystick.Instance.Axis;
            _PlayerController.SetHorizontal(axis.x);
            _PlayerController.SetVertical(axis.y);
        }

        protected override void Jump()
        {

        }

        protected override void Roll()
        {

        }

        protected override void Attack()
        {

        }

        protected override void Action()
        {

        }

        protected override void Pause()
        {

        }
    }
}