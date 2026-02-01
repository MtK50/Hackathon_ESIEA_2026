

using UnityEngine;

public class Level_height : Level
{

    public Transform spawnPoint;
    public AudioSource soun;
    private void OnEnable()
    {
        LevelManager.Instance.currentLevel = this;


        IntializeLevel();
    }

    public override void IntializeLevel()
    {
        soun.Play();
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
