using Indicator;
using UnityEngine;

namespace GameSystem
{
    public class GameStatusManager : MonoBehaviour
    {
        public AchievementSystem achievementSystem;
        public IndicatorManager indicatorManager;

        private void Update()
        {
            if (achievementSystem.isGameFinished) { return; }

            if (indicatorManager.isGameOver) { return; }
        }
    }
}