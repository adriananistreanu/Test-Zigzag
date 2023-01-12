using Cinemachine;
using UnityEngine;

namespace CodeBase.CameraLogic
{ 
    [SaveDuringPlay] [AddComponentMenu("")]
    public class LockCameraX : CinemachineExtension
    {
        public float xPosition = 50f;

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, 
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Body)
            {
                var pos = state.RawPosition;
                pos.x = xPosition;
                state.RawPosition = pos;
            }
        }
    }
}