

public class Level_One : Level
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
        
        
        isInitialized = true;
        
    }
    
    private void EntrerVotreCodeIci()
    {
     
        // Bla-bla-bla

        OnCompleteLevel();

    }
}
