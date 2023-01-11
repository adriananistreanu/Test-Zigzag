using System;
using CodeBase.Common;
using UnityEngine;

namespace CodeBase.Ball
{
    public class BallControl : MonoBehaviour
    {
        [SerializeField] private ParticleSystem collectGemFX;
        
        private int _collectedGems;
        private bool _dead;
        private EventsHolder EventsHolder => EventsHolder.Instance;
        
        private void Update()
        {
            if (CheckIfDead()) 
                Die();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Gem"))
            {
                CollectGem(other.gameObject);
            }
        }

        private void CollectGem(GameObject gem)
        {
            _collectedGems++;
            Instantiate(collectGemFX, gem.transform.position, collectGemFX.transform.rotation);
            Destroy(gem);
        }

        private void Die()
        {
            _dead = true;
            EventsHolder.dieEvent?.Invoke();
        }

        private bool CheckIfDead() => transform.position.y < 0f && !_dead;
    }
}