using UnityEngine;

namespace CodeBase
{
    public class EndlessGeneration : MonoBehaviour
    {
        [Header("PLATFORM GENERATION")] [SerializeField]
        private Transform platformPrefab;

        [SerializeField] private Transform mainPlatform;
        [SerializeField] private Transform platformsParent;
        [SerializeField] private int maxSize;
        [SerializeField] private int countToSpawn;
        [SerializeField] private int rateToSpawn;

        private const int MinSize = 2;
        private Transform _lastPlatform;
        private SpawnDirection _spawnDirection;

        [Header("GEMS GENERATION")] [SerializeField]
        private Transform gemPrefab;

        [SerializeField] private Transform gemsParent;

        public int RateToSpawn => rateToSpawn;

        private void Start()
        {
            _lastPlatform = mainPlatform;
            _spawnDirection = (SpawnDirection)Random.Range(0, 2);
            Debug.Log(_spawnDirection.ToString());
            SpawnNextPlatforms();
        }

        public void SpawnNextPlatforms()
        {
            for (int i = 0; i < countToSpawn; i++)
            {
                InstantiateNewPlatform();
            }
        }

        private void InstantiateNewPlatform()
        {
            Vector3 scale = GetPlatformRandomScale();
            var position = GetPlatformNextPosition(scale);
            Transform newPlatform = Instantiate(platformPrefab, position, Quaternion.identity, platformsParent);
            newPlatform.localScale = scale;
            _lastPlatform = newPlatform;
            InstantiateGems(_lastPlatform);
            SwitchDirection();
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
                _lastPlatform.position.x - _lastPlatform.localScale.x / 2 - scale.x / 2 + MinSize,
                platformPrefab.position.y,
                _lastPlatform.position.z + _lastPlatform.localScale.z / 2 + scale.z / 2);
        }

        private Vector3 GetPlatformRandomScale()
        {
            var scale = Vector3.one;
            if (_spawnDirection == SpawnDirection.Forward)
                scale = new Vector3(MinSize, platformPrefab.localScale.y, GetEvenRandomNumber());
            else if (_spawnDirection == SpawnDirection.Left)
                scale = new Vector3(GetEvenRandomNumber(), platformPrefab.localScale.y, MinSize);

            return scale;
        }

        private Vector3 GetGemNextPosition(Transform platform)
        {
            Vector3 position = Vector3.one;
            if (_spawnDirection == SpawnDirection.Forward)
                position = new Vector3(platform.position.x, gemPrefab.position.y,
                    platform.position.z + GetGemRandomPos(platform.localScale.z));
            else if (_spawnDirection == SpawnDirection.Left)
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
            if (_spawnDirection == SpawnDirection.Forward)
                _spawnDirection = SpawnDirection.Left;
            else if (_spawnDirection == SpawnDirection.Left)
                _spawnDirection = SpawnDirection.Forward;
        }
    }

    public enum SpawnDirection
    {
        Forward = 0,
        Left = 1
    }
}