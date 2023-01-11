using Cinemachine;
using UnityEngine;

namespace CodeBase.Camera
{ 
    [SaveDuringPlay] [AddComponentMenu("")]
    public class LockCameraY : CinemachineExtension
    {
        public float yPosition = 50f;

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, 
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Body)
            {
                var pos = state.RawPosition;
                pos.y = yPosition;
                state.RawPosition = pos;
            }
        }
    }
}