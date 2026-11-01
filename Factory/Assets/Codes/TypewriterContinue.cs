
using UnityEngine;
using UnityEngine.SceneManagement;

public class TypewriterContinue : MonoBehaviour
{
    [Header("Next Scene")]
    public string nextSceneName = "Game3Scene";

    public void GoToNextGame()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
