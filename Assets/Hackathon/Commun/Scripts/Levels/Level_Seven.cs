

using UnityEngine;

public class Level_Seven : Level
{

    public Transform spawnPoint;
    private void OnEnable()
    {
        LevelManager.Instance.currentLevel = this;


        IntializeLevel();
    }

    public override void IntializeLevel()
    {
        if (isInitialized) return;

        Debug.Log("Intialize Level Five");
        GameManager.Instance.cabane.SetActive(false);
        GameManager.Instance.playerTransform.transform.position = spawnPoint.position;
        isInitialized = true;

    }

    private void EntrerVotreCodeIci()
    {

        // Bla-bla-bla

        OnCompleteLevel();

    }
}
