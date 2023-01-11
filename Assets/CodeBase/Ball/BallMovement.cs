using System;
using CodeBase.Common;
using UnityEngine;

namespace CodeBase.Ball
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallMovement : MonoBehaviour
    {
        public event Action<int> DirectionChange;
        
        [SerializeField] private float speed;

        private Rigidbody _rigidbody;
        private Vector3 _moveDir = Vector3.forward;
        private bool _enabled;
        private int _dirChangeCount;

        private EventsHolder EventsHolder => EventsHolder.Instance;

        private void Start()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _enabled)
            {
                ChangeDirection();
            }
        }

        private void FixedUpdate()
        {
            if(_enabled)
                Move();
        }

        private void Move()
        {
            Vector3 newPos = transform.position + _moveDir * speed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(newPos);
        }

        private void ChangeDirection()
        {
            _moveDir = GetMoveDirection();
            _dirChangeCount++;
            DirectionChange?.Invoke(_dirChangeCount);
        }

        private Vector3 GetMoveDirection() => 
            _moveDir == Vector3.forward ? Vector3.left : Vector3.forward;

        private void SwitchMoveState(bool enable) => _enabled = enable;

        private void OnEnable()
        {
            EventsHolder.startEvent.AddListener(() => SwitchMoveState(true));
            EventsHolder.dieEvent.AddListener(() => SwitchMoveState(false));
        }

        private void OnDisable()
        {
            EventsHolder?.startEvent.RemoveListener(() => SwitchMoveState(true));
            EventsHolder?.dieEvent.RemoveListener(() => SwitchMoveState(false));
        }
    }
}
