
using UnityEngine;
using TMPro;

public class TypewriterGame : MonoBehaviour
{
    [Header("Text References")]
    public TMP_Text messageText;
    public TMP_Text typedText;
    public TMP_Text timerText;

    [Header("Timer Settings")]
    public float timeLimit = 90f;
    public float tickingStartsAt = 10f;

    [Header("Success UI")]
    public GameObject successPanel;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource tickingSource;
    public AudioSource musicSource;

    [Header("Sound Effects")]
    public AudioClip typingSound;
    public AudioClip tickingSound;
    public AudioClip successSound;
    public AudioClip errorSound;

    [Header("Audio Volumes")]
    [Range(0f, 1f)] public float typingVolume = 0.6f;
    [Range(0f, 1f)] public float musicVolume = 0.25f;
    [Range(0f, 1f)] public float tickingVolume = 0.7f;

    private float timeRemaining;
    private string targetMessage;
    private string currentTyping = "";

    private bool gameEnded = false;
    private bool tickingStarted = false;

    void Start()
    {
        targetMessage = messageText.text;
        currentTyping = "";
        typedText.text = "";

        timeRemaining = timeLimit;
        gameEnded = false;
        tickingStarted = false;

        if (successPanel != null)
            successPanel.SetActive(false);

        if (musicSource != null && musicSource.clip != null)
        {
            musicSource.volume = musicVolume;
            musicSource.loop = true;
            musicSource.Play();
        }

        if (tickingSource != null)
        {
            tickingSource.Stop();
            tickingSource.loop = true;
        }

        UpdateTimerText();
    }

    void Update()
    {
        if (gameEnded) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            UpdateTimerText();
            GameOver();
            return;
        }

        UpdateTimerText();

        // Start ticking at the final 10 seconds
        if (!tickingStarted &&
            timeRemaining <= tickingStartsAt)
        {
            tickingStarted = true;

            if (tickingSource != null && tickingSound != null)
            {
                tickingSource.clip = tickingSound;
                tickingSource.volume = tickingVolume;
                tickingSource.loop = true;
                tickingSource.Play();
            }

            if (musicSource != null)
                musicSource.volume = musicVolume * 0.5f;
        }

        // Keyboard input
        foreach (char key in Input.inputString)
        {
            if (key == '\b' || key == '\n' || key == '\r')
                continue;

            if (currentTyping.Length >= targetMessage.Length)
                break;

            char expected = targetMessage[currentTyping.Length];

            if (char.ToUpperInvariant(key) ==
                char.ToUpperInvariant(expected))
            {
                currentTyping += expected;
                typedText.text = currentTyping;

                PlaySFX(typingSound, typingVolume);
            }
            else
            {
                currentTyping = "";
                typedText.text = "";

                PlaySFX(errorSound, 0.7f);
            }

            if (currentTyping == targetMessage)
            {
                WinGame();
                break;
            }
        }
    }

    void PlaySFX(AudioClip clip, float volume)
    {
        if (sfxSource != null && clip != null)
            sfxSource.PlayOneShot(clip, volume);
    }

    void UpdateTimerText()
    {
        if (timerText == null) return;

        int seconds = Mathf.CeilToInt(timeRemaining);

        timerText.text = string.Format(
            "{0:00}:{1:00}",
            seconds / 60,
            seconds % 60
        );
    }

    void WinGame()
    {
        gameEnded = true;

        StopTicking();
        PlaySFX(successSound, 1f);

        if (musicSource != null)
            musicSource.volume = musicVolume;

        if (successPanel != null)
            successPanel.SetActive(true);

        Debug.Log("MESSAGE SENT!");
    }

    void GameOver()
    {
        gameEnded = true;

        StopTicking();

        if (musicSource != null)
            musicSource.Stop();

        Debug.Log("TIME'S UP!");
    }

    void StopTicking()
    {
        if (tickingSource != null)
            tickingSource.Stop();
    }

    public void RestartGame()
    {
        currentTyping = "";
        typedText.text = "";

        timeRemaining = timeLimit;
        gameEnded = false;
        tickingStarted = false;

        StopTicking();

        if (successPanel != null)
            successPanel.SetActive(false);

        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
            if (!musicSource.isPlaying)
                musicSource.Play();
        }

        UpdateTimerText();
    }
}
