using System;
using UnityEngine;

namespace UniversalAssetsProject.Utilities.Timer
{
    public class Timer : MonoBehaviour
    {
        public Action TimerCompleteAction;
        public Action OnTimerStart;

        public float TimeLimit;
        private float currentTime;
        private bool isRunning;
        private bool isPaused;
        private bool isComplete;

        public bool IsComplete { get { return isComplete; } }
        public bool IsRunning {  get { return isRunning && !isPaused; } }
        public float TimeRemaining { get { return Mathf.Max(currentTime, 0.0f); } }

        public Timer(float timeLimit)
        {
            TimeLimit = timeLimit;
            currentTime = timeLimit;
        }

        public void Update()
        {
            if(isRunning && !isPaused)
            {
                currentTime -= Time.deltaTime;
                if(currentTime <= 0f)
                {
                    OnTimerComplete();
                }
            }
        }

        public void StartTimer()
        {
            isRunning = true;
            isPaused = false;
            isComplete = false;
            currentTime = TimeLimit;
            OnTimerStart?.Invoke();
        }

        public void PauseTimer() //Toggle pause maybe?
        {
            isPaused = true;
        }

        public void ResumeTimer()
        {
            isPaused = false;
        }

        public void ResetTimer()
        {
            isRunning = false;
            isPaused = false;
            isComplete = false;
            currentTime = TimeLimit;
        }

        public void SetTimeLimit(float timeLimit)
        {
            this.TimeLimit = timeLimit;
            currentTime = timeLimit;
        }

        private void OnTimerComplete()
        {
            isRunning = false;
            isPaused = false;
            isComplete = true;
            TimerCompleteAction?.Invoke();
        }
    }
}
