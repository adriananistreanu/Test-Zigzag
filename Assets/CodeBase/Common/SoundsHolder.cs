using UnityEngine;

namespace CodeBase.Common
{
    public class SoundsHolder : Singleton<SoundsHolder>
    {
        public AudioSource clickSound;
        public AudioSource tapSound;
        public AudioSource loseSound;
        public AudioSource collectSound;
    }
}