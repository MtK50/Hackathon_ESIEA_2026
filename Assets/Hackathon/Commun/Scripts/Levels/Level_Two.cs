

using UnityEngine;

public class Level_Two : Level
{
    private bool isInitialized = false;
    
    private void OnEnable()
    {
        LevelManager.Instance.currentLevel = this;
        
        
        IntializeLevel();
    }

    public override void IntializeLevel()
    {
        if (isInitialized) return;
        
        Debug.Log("Intialize Level Two");
        
        isInitialized = true;
        
    }
    
    private void EntrerVotreCodeIci()
    {
     
        // Bla-bla-bla

        OnCompleteLevel();

    }
}
