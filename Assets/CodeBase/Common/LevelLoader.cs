using System.Collections;
using CodeBase.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Common
{
    public class LevelLoader : Singleton<LevelLoader>
    {
        private static EventsHolder EventsHolder => EventsHolder.Instance;
        
        public static IEnumerator PauseGame(float delay)
        {
            EventsHolder.gamePausedEvent.Invoke();
            yield return new WaitForSeconds(delay);
            Time.timeScale = 0f;
        }
        
        public static void UnpauseGame()
        {
            EventsHolder.gameUnPauseEvent.Invoke();
            Time.timeScale = 1f;
        }
        
        public static void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}