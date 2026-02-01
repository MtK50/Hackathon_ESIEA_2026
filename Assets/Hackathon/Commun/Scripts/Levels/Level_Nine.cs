using UnityEngine;

public class Level_Nine : Level
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
        GameManager.Instance.cabane.SetActive(false);
        GameManager.Instance.playerTransform.transform.localPosition = spawnPoint.localPosition;
        isInitialized = true;

    }

    private void EntrerVotreCodeIci()
    {

        // Bla-bla-bla

        OnCompleteLevel();

    }
}
