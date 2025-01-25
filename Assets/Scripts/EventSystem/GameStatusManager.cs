using Indicator;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSystem
{
    public class GameStatusManager : MonoBehaviour
    {
        public AchievementSystem achievementSystem;
        public IndicatorManager indicatorManager;
        public TextMeshProUGUI title;
        public GameObject panel;
        public GameObject pausePanel;
        public bool isPaused;
        public bool isOver;

        private void Start()
        {
            Time.timeScale = 1f;
            panel.SetActive(false);
            isPaused = true;
        }

        private void Update()
        {
            if (achievementSystem.isGameFinished) {
                isOver = true;
                PauseGame();
                panel.SetActive(true);
                title.text = "VICTORIA!";
                return; 
            }

            if (indicatorManager.isGameOver) {
                isOver = true;
                PauseGame();
                panel.SetActive(true);
                title.text = "GAME OVER";
                return; 
            }
        }

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
            isPaused = true;
        }

        public void PausePanel()
        {
            isPaused = !isPaused;

            if (isPaused) { 
                pausePanel.SetActive(false);
                ResumeGame();
            } else
            {
                pausePanel.SetActive(true);
                PauseGame();
            }
            
        }
    }
}