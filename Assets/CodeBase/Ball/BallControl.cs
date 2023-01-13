using System;
using CodeBase.Common;
using CodeBase.Helpers;
using CodeBase.LevelGeneration;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase.Ball
{
    public class BallControl : MonoBehaviour
    {
        public event Action<int> ScoreChange;

        [SerializeField] private EndlessGeneration endlessGeneration;
        [SerializeField] private ParticleSystem collectGemFX;
        [SerializeField] private TextMeshPro collectScoreText;

        private Collider _currentPlatform;
        private int _platformsPassed;
        private int _score;
        private int _bestScore;
        private int _collectedGems;
        private bool _dead;
        private const int CollectScoreCount = 2;

        private static EventsHolder EventsHolder => EventsHolder.Instance;
        private static SoundsHolder SoundsHolder => SoundsHolder.Instance;

        public int CollectedGems => _collectedGems;
        public int BestScore => _bestScore;
        public int Score => _score;

        private void Awake()
        {
            LoadSaves();
        }

        private void Update()
        {
            RaycastHit hit;

            if (Grounded(out hit))
                DestroyPreviousPlatform(hit);
            else if (!_dead)
                Die();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Gem"))
            {
                CollectGem(other.gameObject);
            }
        }

        private bool Grounded(out RaycastHit hit)
        {
            return Physics.Raycast(transform.position, Vector3.down, out hit, 1f,
                1 << LayerMask.NameToLayer("Ground"));
        }

        private void DestroyPreviousPlatform(RaycastHit hit)
        {
            if (_currentPlatform == null)
                _currentPlatform = hit.collider;

            if (_currentPlatform != hit.collider)
            {
                _currentPlatform.attachedRigidbody.isKinematic = false;
                Destroy(_currentPlatform.gameObject, 5f);
                _currentPlatform = null;
                IncrementPlatformPassed();
            }
        }

        private void IncrementPlatformPassed()
        {
            _platformsPassed++;
            if (_platformsPassed >= endlessGeneration.RateToSpawn)
                endlessGeneration.SpawnNextPlatforms();
        }

        public void AddScore(int count)
        {
            _score += count;
            ScoreChange?.Invoke(_score);
            SaveScore();
        }

        private void CollectGem(GameObject gem)
        {
            _collectedGems++;
            AddScore(CollectScoreCount);
            CollectGemEffect(gem);
            SaveGems();
            Destroy(gem);
        }

        private void CollectGemEffect(GameObject gem)
        {
            Instantiate(collectGemFX, gem.transform.position, collectGemFX.transform.rotation);
            DisplayScoreGemText(gem.transform);
            SoundsHolder.collectSound.Play();
        }

        private void DisplayScoreGemText(Transform target)
        {
            var textInstance = Instantiate(collectScoreText,
                target.position + collectScoreText.transform.position,
                collectScoreText.transform.rotation);
            textInstance.text = "+" + CollectScoreCount;
            textInstance.transform.LookAt(2 * textInstance.transform.position - Camera.main.transform.position);
            textInstance.transform.DOMoveY(textInstance.transform.position.y + 5f, 1f)
                .OnComplete(() => { Destroy(textInstance.gameObject); });
            textInstance.DOFade(0f, 1f);
        }

        private void Die()
        {
            _dead = true;
            EventsHolder.dieEvent?.Invoke();
            Destroy(gameObject, 5f);
            SoundsHolder.loseSound.Play();
        }

        private void SaveGems()
        {
            PlayerPrefs.SetInt(SaveKeys.GemsCountSaveKey, _collectedGems);
        }

        private void SaveScore()
        {
            if (_score > _bestScore)
            {
                PlayerPrefs.SetInt(SaveKeys.BestScoreSaveKey, _score);
                _bestScore = _score;
            }
        }

        private void LoadSaves()
        {
            _collectedGems = PlayerPrefs.GetInt(SaveKeys.GemsCountSaveKey, _collectedGems);
            _bestScore = PlayerPrefs.GetInt(SaveKeys.BestScoreSaveKey, _bestScore);
        }
    }
}