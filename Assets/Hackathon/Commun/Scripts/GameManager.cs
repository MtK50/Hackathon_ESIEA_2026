using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public Transform playerTransform;
    
    public List<GameObject> objectToDisableAtEnd = new List<GameObject>();
    public List<GameObject> objectToEnableAtEnd = new List<GameObject>();
    public TextMeshProUGUI endGameText;
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }
    
    
    public void GameIsCompleted(bool withSuccess)
    {
        foreach (var obj in objectToDisableAtEnd)
        {
            obj.SetActive(false);
        }
        foreach (var obj in objectToEnableAtEnd)
        {
            obj.SetActive(true);
        }
        if (withSuccess)
        {
            endGameText.text = "Félicitations!\nVous avez réussi à vous échapper!";
        }
        else
        {
            endGameText.text = "Temps écoulé!\nVous n'avez pas réussi à vous échapper...";
        }
    }
}
