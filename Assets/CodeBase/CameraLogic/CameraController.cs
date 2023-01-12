using Cinemachine;
using CodeBase.Common;
using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera followCam;

        private static  EventsHolder EventsHolder => EventsHolder.Instance;

        private void LockFollowCam()
        {
            followCam.Follow = null;
            followCam.LookAt = null;
        }

        private void OnEnable()
        {
            EventsHolder.dieEvent.AddListener(LockFollowCam);
        }

        private void OnDisable()
        {
            EventsHolder?.dieEvent.RemoveListener(LockFollowCam);
        }
    }
}