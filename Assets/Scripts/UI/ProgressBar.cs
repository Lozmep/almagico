using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressBar : MonoBehaviour
    {
        private Slider progressBar;
        public float initialValue;

        private void Awake()
        {
            progressBar = GetComponent<Slider>();
            progressBar.value = initialValue;
        }

        public void SetProgress(float targetProgress)
        {
            progressBar.value = targetProgress;
        }
     
    }
}
