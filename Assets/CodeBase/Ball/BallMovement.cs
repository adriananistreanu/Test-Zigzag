using CodeBase.Common;
using CodeBase.Helpers;
using CodeBase.LevelGeneration;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Ball
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private BallControl ballControl;
        [SerializeField] private SettingsWindow settingsWindow;
        [SerializeField] private Transform platformDetectorLeft;
        [SerializeField] private Transform platformDetectorForward;

        private Rigidbody _rigidbody;
        private Vector3 _moveDir = Vector3.forward;
        private bool _enabled;
        private bool _autoControlOn;
        private Platform _currentPlatform;
        private static EventsHolder EventsHolder => EventsHolder.Instance;
        private static SoundsHolder SoundsHolder => SoundsHolder.Instance;

        private void Awake()
        {
            LoadSaves();
        }

        private void Start()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!_autoControlOn)
            {
                if (Input.GetMouseButtonDown(0) && _enabled && !UIHelper.IsPointerOverUIObject())
                    ChangeDirection();
            }
            else
            {
                RaycastHit hit;
                if (Grounded(out hit))
                    AutoChangeDirection(hit);
            }
        }

        private void FixedUpdate()
        {
            if (_enabled)
                Move();
        }

        private void LoadSaves()
        {
            _autoControlOn = PlayerPrefs.GetInt(SaveKeys.AutoControlSaveKey, 0) == 1  ? true : false;
        }

        private void Move()
        {
            Vector3 newPos = transform.position + _moveDir * speed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(newPos);
        }

        private void ChangeDirection()
        {
            _moveDir = _moveDir == Vector3.forward ? Vector3.left : Vector3.forward;
            ballControl.AddScore(1);
            SoundsHolder.tapSound.Play();
        }

        private void SwitchMoveState(bool enable) => _enabled = enable;

        private void OnAutoControlSwitch(bool on)
        {
            _autoControlOn = on;
            SaveAutoControlState();
        }

        private bool Grounded(out RaycastHit hit) =>
            Physics.Raycast(transform.position, Vector3.down, out hit, 1f,
                1 << LayerMask.NameToLayer("Ground"));

        private bool DetectorIsGrounded(Transform detector, out RaycastHit hit) =>
            Physics.Raycast(detector.position, Vector3.down, out hit, 1f,
                1 << LayerMask.NameToLayer("Ground"));

        private void AutoChangeDirection(RaycastHit hit)
        {
            if (_currentPlatform == null)
                _currentPlatform = hit.transform.GetComponent<Platform>();

            if (_currentPlatform.transform != hit.transform)
            {
                Platform newPlatform = hit.transform.GetComponent<Platform>();
                if (_currentPlatform.direction != newPlatform.direction)
                {
                    _currentPlatform = newPlatform;
                }
            }

            if (_currentPlatform)
                AutoChangeDirIfOnCenter();

            AutoChangeDirIfReachMargin();
        }

        private void AutoChangeDirIfOnCenter()
        {
            if (_currentPlatform.direction == PlatformDirection.Forward &&
                transform.position.x >= _currentPlatform.transform.position.x)
                _moveDir = Vector3.forward;
            if (_currentPlatform.direction == PlatformDirection.Left &&
                transform.position.z >= _currentPlatform.transform.position.z)
                _moveDir = Vector3.left;
        }

        private void AutoChangeDirIfReachMargin()
        {
            RaycastHit detectorHit;

            if (_moveDir == Vector3.left && !DetectorIsGrounded(platformDetectorLeft, out detectorHit))
                _moveDir = Vector3.forward;
            else if (_moveDir == Vector3.forward && !DetectorIsGrounded(platformDetectorForward, out detectorHit))
                _moveDir = Vector3.left;
        }

        private void SaveAutoControlState()
        {
            PlayerPrefs.SetInt(SaveKeys.AutoControlSaveKey, _autoControlOn ? 1 : 0);
        }

        private void OnEnable()
        {
            EventsHolder.startEvent.AddListener(() => SwitchMoveState(true));
            EventsHolder.gamePausedEvent.AddListener(() => SwitchMoveState(false));
            EventsHolder.gameUnPauseEvent.AddListener(() => SwitchMoveState(true));
            EventsHolder.dieEvent.AddListener(() => SwitchMoveState(false));

            settingsWindow.AutoControlSwitch += OnAutoControlSwitch;
        }

        private void OnDisable()
        {
            EventsHolder?.startEvent.RemoveListener(() => SwitchMoveState(true));
            EventsHolder?.gamePausedEvent.RemoveListener(() => SwitchMoveState(false));
            EventsHolder?.gameUnPauseEvent.RemoveListener(() => SwitchMoveState(true));
            EventsHolder?.dieEvent.RemoveListener(() => SwitchMoveState(false));
        }

        private void OnDestroy()
        {
            settingsWindow.AutoControlSwitch -= OnAutoControlSwitch;
        }
    }
}