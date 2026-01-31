using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    public Scene mainScene;
    public List<Level> levelsList = new List<Level>();

    [Header("Current Level Info")]
    public int currentLevelId = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        
        levelsList = new List<Level>(this.transform.GetComponentsInChildren<Level>());
    }

    public void CloseLevel()
    {
        foreach (Level level in levelsList)
        {
            if (level.id == currentLevelId)
            {
                SceneManager.UnloadSceneAsync(level.sceneName);
                break; // Stop iterating after unloading the level
            }
        }
    }

    public void LoadPreviousLevel()
    {
        if (currentLevelId > 0)
        {
            int previousLevelId = currentLevelId - 1;
            foreach (Level level in levelsList)
            {
                if (level.id == previousLevelId)
                {
                    CloseLevel();
                    SceneManager.LoadScene(level.sceneName, LoadSceneMode.Additive);
                    currentLevelId = previousLevelId; // Update currentLevelId
                }
            }
        }
    }

    public void LoadNextLevel()
    {
        int nextLevelId = currentLevelId + 1;
        foreach (Level level in levelsList)
        {
            if (level.id == nextLevelId)
            {
                CloseLevel();
                SceneManager.LoadScene(level.sceneName, LoadSceneMode.Additive);
                currentLevelId = nextLevelId; // Update currentLevelId
            }
        }
    }
}