using UnityEngine;

public class Level_Six : Level
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

        Debug.Log("Intialize Level Six");
        GameManager.Instance.cabane.SetActive(true);
        GameManager.Instance.playerTransform.transform.position = spawnPoint.position;
        isInitialized = true;

    }

    private void EntrerVotreCodeIci()
    {

        // Bla-bla-bla

        OnCompleteLevel();

    }
}
