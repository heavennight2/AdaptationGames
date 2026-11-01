
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueToNextGame : MonoBehaviour
{
    [Header("References")]
    public PackManager packManager;
    public GameObject continueButton;

    [Header("Next Game")]
    public string nextSceneName = "TypewriterScene";

    private bool unlocked = false;

    void Start()
    {
        continueButton.SetActive(false);
    }

    void Update()
    {
        // Show Continue after completing one pack
        if (!unlocked && packManager.completedPacks >= 1)
        {
            unlocked = true;
            continueButton.SetActive(true);
        }
    }

    public void GoToNextGame()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
