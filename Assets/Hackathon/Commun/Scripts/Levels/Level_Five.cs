

using UnityEngine;

public class Level_Five : Level
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
        GameManager.Instance.cabane.SetActive(true);
        GameManager.Instance.playerTransform.transform.localPosition = spawnPoint.position;
        isInitialized = true;

    }

    private void EntrerVotreCodeIci()
    {

        // Bla-bla-bla

        OnCompleteLevel();

    }
}
