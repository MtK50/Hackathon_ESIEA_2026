using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicL10 : MonoBehaviour
{
    public Transform guillotine;
    
    public float minYPosition = 3.343344f;
    public float maxYPosition = -0.400f;
    
    public float duration = 3f;
    [SerializeField] private float nextLevelDelay = 1f; // Configurable delay before proceeding to the next level

    private void RestartGuillotinePosition()
    {
        guillotine.localPosition = new Vector3(guillotine.localPosition.x, minYPosition, guillotine.localPosition.z);
    }
    
    public void AllenIsCollected()
    {
        RestartGuillotinePosition();
        Debug.Log("Allen is collected");
        DropGuillotine();
    }

    // Drop the guillotine over time
    public void DropGuillotine()
    {
        StartCoroutine(DropGuillotineCoroutine());
    }
    
    private IEnumerator DropGuillotineCoroutine()
    {
        float elapsed = 0f;
        Vector3 startPosition = guillotine.localPosition;
        Vector3 endPosition = new Vector3(guillotine.localPosition.x, maxYPosition, guillotine.localPosition.z);

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration); // Clamp t to ensure it doesn't exceed 1.0
            guillotine.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        guillotine.localPosition = endPosition; // Ensure it reaches the final localPosition
        Invoke(nameof(Next), nextLevelDelay); // Use the configurable delay
    }
    
    private void Next()
    {
        Debug.Log("Next");
        LevelManager.Instance.currentLevel.OnCompleteLevel();
    }
}