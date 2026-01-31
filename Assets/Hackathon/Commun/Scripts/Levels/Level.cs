using UnityEngine;


public class Level : MonoBehaviour
{
    public bool isLevelCompleted = false;
    public bool isInitialized = false;



    public virtual void IntializeLevel()
    {
        Debug.Log("Intialize Level");
    }
    
    public void OnCompleteLevel()
    {
        GameManager.Instance.cabane.SetActive(true);
        isLevelCompleted = true;
        LevelManager.Instance.LoadNextLevel();
    }
}
