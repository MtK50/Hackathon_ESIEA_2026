using UnityEngine;

public class UILauncher : MonoBehaviour
{

    public string sceneToLaunch = "SceneCommun";
    public void LaunchGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLaunch);
    }
}
