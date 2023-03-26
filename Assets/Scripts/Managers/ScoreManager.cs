using System.Collections;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        #region Fields

        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI timerText;
        public int score;
        public int timer;
        
        private readonly WaitForSecondsRealtime _countDownSecond = new (1f);

        #endregion

        private void Start()
        {
            timer = 60;
            StartCoroutine(CountDownCoroutine());
        }

        private IEnumerator CountDownCoroutine()
        {
            while (true)
            {
                timer--;
                yield return _countDownSecond;
            }
        }

        public void EditTimer(int newTime)
        {
            timer += newTime;
        }

        public void EditScore(int newScore)
        {
            score += newScore;
        }


    }
}