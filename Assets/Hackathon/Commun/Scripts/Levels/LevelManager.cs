using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct LevelData
{
    public int id;
    public string sceneName;
}


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    public Scene mainScene;
    public List<LevelData> levelsList = new List<LevelData>();

    [Header("Current Level Info")]
    public int currentLevelId = 0;
    public Level currentLevel = null;

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
    }

    public void CloseLevel()
    {
        foreach (LevelData level in levelsList)
        {
            if (level.id == currentLevelId)
            {
                SceneManager.UnloadSceneAsync(level.sceneName);
                break;
            }
        }
    }

    public void LoadPreviousLevel()
    {
        if (currentLevelId > 0)
        {
            int previousLevelId = currentLevelId - 1;
            foreach (LevelData level in levelsList)
            {
                if (level.id == previousLevelId)
                {
                    CloseLevel();
                    SceneManager.LoadScene(level.sceneName, LoadSceneMode.Additive);
                    currentLevelId = previousLevelId;
                }
            }
        }
    }

    public void LoadNextLevel()
    {
        int nextLevelId = currentLevelId + 1;
        foreach (LevelData level in levelsList)
        {
            if (level.id == nextLevelId)
            {
                CloseLevel();
                SceneManager.LoadScene(level.sceneName, LoadSceneMode.Additive);
                currentLevelId = nextLevelId;
            }
        }
    }
}