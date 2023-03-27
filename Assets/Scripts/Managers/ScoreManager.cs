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
                //Clock
                AddToTimer(-1);
                yield return _countDownSecond;
            }
        }

        public void AddToTimer(int newTime)
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

        public void AddToScore(int newScore)
        {
            score += newScore;
            scoreText.text = score.ToString();
        }

        private IEnumerator PenaltyCoroutine()
        {
            while (true)
            {
                AddToScore(-1);
                yield return _countDownSecond;
            }
        }
        
        //Gets called if bus is off road 
        public void StartPenalty() => _penaltyCoroutine = StartCoroutine(PenaltyCoroutine());
        
        //Gets called if bus in back on road
        public void StopPenalty() => StopCoroutine(_penaltyCoroutine);

      
    }
}