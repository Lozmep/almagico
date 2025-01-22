using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSystem
{
    public class GameStatus : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene(1);
        }

        public void QuitGame()
        {
            Application.Quit(); //PROD
            //UnityEditor.EditorApplication.isPlaying = false;
        }

        public void Menu()
        {
            SceneManager.LoadScene(0);
        }

        public void PauseGame()
        {
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            Time.timeScale = 1f;
        }
    }
}