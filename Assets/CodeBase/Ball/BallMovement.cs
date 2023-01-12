using System;
using CodeBase.Common;
using UnityEngine;

namespace CodeBase.Ball
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private BallControl ballControl;

        private Rigidbody _rigidbody;
        private Vector3 _moveDir = Vector3.forward;
        private bool _enabled;

        private static  EventsHolder EventsHolder => EventsHolder.Instance;
        private static  SoundsHolder SoundsHolder => SoundsHolder.Instance;

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
            ballControl.AddScore(1);
            SoundsHolder.tapSound.Play();
        }

        private Vector3 GetMoveDirection() => 
            _moveDir == Vector3.forward ? Vector3.left : Vector3.forward;

        private void SwitchMoveState(bool enable) => _enabled = enable;

        private void OnEnable()
        {
            EventsHolder.startEvent.AddListener(() => SwitchMoveState(true));
            EventsHolder.gamePausedEvent.AddListener(() => SwitchMoveState(false));
            EventsHolder.gameUnPauseEvent.AddListener(() => SwitchMoveState(true));
            EventsHolder.dieEvent.AddListener(() => SwitchMoveState(false));
        }

        private void OnDisable()
        {
            EventsHolder?.startEvent.RemoveListener(() => SwitchMoveState(true));
            EventsHolder?.gamePausedEvent.RemoveListener(() => SwitchMoveState(false));
            EventsHolder?.gameUnPauseEvent.RemoveListener(() => SwitchMoveState(true));
            EventsHolder?.dieEvent.RemoveListener(() => SwitchMoveState(false));
        }
    }
}
