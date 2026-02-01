using System;
using UnityEngine;

public class LogicL9 : MonoBehaviour
{
    public Transform key;
    
    public void KeyIsCollected()
    {
        Invoke(nameof(Next), 3f);
    }

    private void Next()
    {
        LevelManager.Instance.currentLevel.OnCompleteLevel();
    }
}
