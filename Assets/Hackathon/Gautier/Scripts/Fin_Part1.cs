using System.Collections.Generic;
using UnityEngine;

public class Fin_Part_1 : MonoBehaviour
{
    private int caillouCount = 0;
    public List<GameObject> cailloux = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Caillou"))
        {
            caillouCount++;
            Debug.Log($"Caillou dans le panier : {caillouCount}/???");

            Destroy(other.gameObject); 

            if (caillouCount >= cailloux.Count)
            {
                Debug.Log("Fini ?");
                LevelManager.Instance.currentLevel.OnCompleteLevel();
            }
        }

    }
}
