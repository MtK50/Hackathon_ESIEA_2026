

using UnityEngine;

public class Level_One : Level
{
    private void OnEnable()
    {
        LevelManager.Instance.currentLevel = this;
        
        
        IntializeLevel();
    }

    public override void IntializeLevel()
    {
        if (isInitialized) return;
        
        Debug.Log("Intialize Level One");
        
        isInitialized = true;
        
    }
    
    private void EntrerVotreCodeIci()
    {
     
        // Bla-bla-bla

        OnCompleteLevel();

    }
}
