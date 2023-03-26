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
        private Coroutine _penaltyCoroutine;
        private readonly WaitForSecondsRealtime _countDownSecond = new (1f);
        #endregion

        private void Start()
        {
            timer = 60;
            score = 0;
            StartCoroutine(CountDownCoroutine());
        }

        private IEnumerator CountDownCoroutine()
        {
            while (true)
            {
                EditTimer(-1);
                yield return _countDownSecond;
            }
        }

        public void EditTimer(int newTime)
        {
            timer += newTime;
            timerText.text= ConvertToClockFormat(timer);
        }
        private string ConvertToClockFormat(int time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = time % 60;
            return $"{minutes:00}:{seconds:00}";
        }

        public void EditScore(int newScore)
        {
            score += newScore;
            scoreText.text = score.ToString();
        }

        private IEnumerator PenaltyCoroutine()
        {
            while (true)
            {
                EditScore(-1);
                yield return _countDownSecond;
            }
        }
        public void StartPenalty() => _penaltyCoroutine = StartCoroutine(PenaltyCoroutine());

        public void StopPenalty() => StopCoroutine(_penaltyCoroutine);

      
    }
}