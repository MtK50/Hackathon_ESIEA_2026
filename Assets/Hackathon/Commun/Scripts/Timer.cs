using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    public float startTimeInSeconds = 30f;
    public float currentTime;
    public int seconds;
    public int minutes;
    private bool timerRunning = true;
    
    public static event Action<int, int> OnTimeUpdated;

    private void Start()
    {
        currentTime = startTimeInSeconds;
        UpdateTimerUI();
        StartTimer();
    }

    


    public void StartTimer()
    {
        timerRunning = true;
        StartCoroutine(UpdateTimerUICoroutine());
    }

    public void StopTimer()
    {
        timerRunning = false;
        StopCoroutine(UpdateTimerUICoroutine());
    }

    private IEnumerator UpdateTimerUICoroutine()
    {
        while (timerRunning)
        {
            yield return new WaitForSeconds(1f);
            currentTime -= 1f;
            UpdateTimerUI();

            OnTimeUpdated?.Invoke(minutes, seconds);

            if (currentTime <= 0f)
            {
                currentTime = 0f;
                StopTimer();
            }
        }
    }


    private void UpdateTimerUI()
    {
        Color targetColor = Color.white;
        if (currentTime < 120f)
        {
            targetColor = Color.yellow;
            if (currentTime < 60f)
            {
                targetColor = Color.red;
            }
        }
        timerText.color = targetColor;

        minutes = Mathf.FloorToInt(currentTime / 60f);
        seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
        
        if(minutes <= 0 && seconds <= 0)
        {
            GameManager.Instance.GameIsCompleted(false);

        }
    }
}
