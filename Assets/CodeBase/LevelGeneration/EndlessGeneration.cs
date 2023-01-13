using UnityEngine;

namespace CodeBase.LevelGeneration
{
    public class EndlessGeneration : MonoBehaviour
    {
        [Header("PLATFORM GENERATION")] 
        [SerializeField] private Platform platformPrefab;
        [SerializeField] private Platform mainPlatform;
        [SerializeField] private Transform platformsParent;
        [SerializeField] private int maxSize;
        [SerializeField] private int countToSpawn;
        [SerializeField] private int rateToSpawn;

        private const int MinSize = 2;
        private Platform _lastPlatform;
        private PlatformDirection _spawnDirection;

        [Header("GEMS GENERATION")] 
        [SerializeField] private Transform gemPrefab;
        [SerializeField] private Transform gemsParent;

        public int RateToSpawn => rateToSpawn;

        private void Start()
        {
            _lastPlatform = mainPlatform;
            _spawnDirection = (PlatformDirection)Random.Range(0, 2);
            
            SpawnNextPlatforms();
        }

        public void SpawnNextPlatforms()
        {
            for (int i = 0; i < countToSpawn; i++) 
                InstantiateNewPlatform();
        }

        private void InstantiateNewPlatform()
        {
            Vector3 scale = GetPlatformRandomScale();
            var position = GetPlatformNextPosition(scale);
            
            Platform newPlatform = Instantiate(platformPrefab, position, Quaternion.identity, platformsParent);
            
            SetNewPlatform(newPlatform, scale);
            SwitchDirection();
        }

        private void SetNewPlatform(Platform newPlatform, Vector3 scale)
        {
            newPlatform.transform.localScale = scale;
            newPlatform.direction = _spawnDirection;
            _lastPlatform = newPlatform;

            InstantiateGems(_lastPlatform.transform);
        }

        private void InstantiateGems(Transform platform)
        {
            var count = Random.Range(0, 2);
            for (int i = 0; i < count; i++)
                Instantiate(gemPrefab, GetGemNextPosition(platform), gemPrefab.rotation, gemsParent);
        }


        private Vector3 GetPlatformNextPosition(Vector3 scale)
        {
            return new Vector3(
                _lastPlatform.transform.position.x - _lastPlatform.transform.localScale.x / 2 - scale.x / 2 + MinSize,
                platformPrefab.transform.position.y,
                _lastPlatform.transform.position.z + _lastPlatform.transform.localScale.z / 2 + scale.z / 2);
        }

        private Vector3 GetPlatformRandomScale()
        {
            var scale = Vector3.one;
            if (_spawnDirection == PlatformDirection.Forward)
                scale = new Vector3(MinSize, platformPrefab.transform.localScale.y, GetEvenRandomNumber());
            else if (_spawnDirection == PlatformDirection.Left)
                scale = new Vector3(GetEvenRandomNumber(), platformPrefab.transform.localScale.y, MinSize);

            return scale;
        }

        private Vector3 GetGemNextPosition(Transform platform)
        {
            Vector3 position = Vector3.one;
            if (_spawnDirection == PlatformDirection.Forward)
                position = new Vector3(platform.position.x, gemPrefab.position.y,
                    platform.position.z + GetGemRandomPos(platform.localScale.z));
            else if (_spawnDirection == PlatformDirection.Left)
                position = new Vector3(platform.position.x + GetGemRandomPos(platform.localScale.x),
                    gemPrefab.position.y, platform.position.z);
            return position;
        }

        private int GetGemRandomPos(float scale)
        {
            return Random.Range(-(int)scale / 2 + 1, (int)scale / 2 - 1);
        }

        private int GetEvenRandomNumber()
        {
            var nr = Random.Range(MinSize, maxSize);
            if (nr % 2 != 0)
                nr++;
            return nr;
        }

        private void SwitchDirection()
        {
            if (_spawnDirection == PlatformDirection.Forward)
                _spawnDirection = PlatformDirection.Left;
            else if (_spawnDirection == PlatformDirection.Left)
                _spawnDirection = PlatformDirection.Forward;
        }
    }

    
}