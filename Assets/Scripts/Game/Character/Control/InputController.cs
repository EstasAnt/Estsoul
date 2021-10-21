using UnityEngine;

namespace Character.Control
{
    public abstract class InputController : MonoBehaviour
    {
        protected PlayerController _PlayerController;
        
        protected virtual void Awake()
        {
            _PlayerController = GetComponent<PlayerController>();
        }
        
        protected virtual void Update()
        {
            Move();
            Jump();
            Roll();
            Attack();
            Action();
            Pause();
        }

        protected abstract void Move();
        protected abstract void Jump();
        protected abstract void Roll();
        protected abstract void Attack();
        protected abstract void Action();
        protected abstract void Pause(); 
    }
}