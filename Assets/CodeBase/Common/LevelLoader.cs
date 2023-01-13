using UnityEngine.SceneManagement;

namespace CodeBase.Common
{
    public class LevelLoader : Singleton<LevelLoader>
    {
        private static EventsHolder EventsHolder => EventsHolder.Instance;
        
        public static void PauseGame() => EventsHolder.gamePausedEvent.Invoke();

        public static void UnpauseGame() => EventsHolder.gameUnPauseEvent.Invoke();

        public static void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}