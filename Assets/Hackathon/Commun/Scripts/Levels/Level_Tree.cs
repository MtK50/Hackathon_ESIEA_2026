

using UnityEngine;

public class Level_Tree : Level
{
    private void OnEnable()
    {
        LevelManager.Instance.currentLevel = this;
        
        
        IntializeLevel();
    }

    public override void IntializeLevel()
    {
        if (isInitialized) return;
        
        Debug.Log("Intialize Level Tree");
        GameManager.Instance.cabane.SetActive(false);
        isInitialized = true;
        
    }
    
    private void EntrerVotreCodeIci()
    {
     
        // Bla-bla-bla

        OnCompleteLevel();

    }
}
