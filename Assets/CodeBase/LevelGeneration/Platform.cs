using UnityEngine;

namespace CodeBase.LevelGeneration
{
    public enum PlatformDirection
    {
        Forward = 0,
        Left = 1
    }
    
    public class Platform : MonoBehaviour
    {
        public PlatformDirection direction;
    }
}
