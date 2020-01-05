using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class ButtonFunctionMenu : MonoBehaviour
{
    public string virtualScene;

    public string EnvironmentScene;

    public void changeVirtualScene() {
        SceneManager.LoadScene(virtualScene);
    }

    public void changeEnvironmentScene()
    {
        SceneManager.LoadScene(EnvironmentScene);
    }
}
