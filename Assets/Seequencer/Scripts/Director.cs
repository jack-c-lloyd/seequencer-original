using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]

[RequireComponent(typeof(AudioSource))]

public class Director : Singleton<Director>
{
    [Range(0, 10)] public uint countdown = 3;

    [Range(float.Epsilon, 2.1f)] public float shortDuration = 0.7f;
    [Range(float.Epsilon, 2.1f)] public float mediumDuration = 1.4f;
    [Range(float.Epsilon, 2.1f)] public float longDuration = 2.1f;

    public string[] winMessages = new string[3] {
        "NEW HIGHSCORE!",
        "CONGRATULATIONS!",
        "PERSONAL BEST!"
    };

    public string[] loseMessages = new string[3] {
        "YOU LOSE!",
        "BAD LUCK!",
        "TRY AGAIN!"
    };

    public TextMesh countdownText;

    private bool isAttempting = false;
    private bool isCorrect = true;
    private uint attempts;
    private uint stage;

    private const string HIGHSCORE = "HIGHSCORE";

    private Sequencer sequencer;
    private AudioSource audioSource;

    private void Awake()
    {
        sequencer = FindObjectOfType<Sequencer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(Direct());
    }

    private IEnumerator Direct()
    {
        attempts = 3;
        stage = 1;

        sequencer.DisableAll();

        if (!PlayerPrefs.HasKey(HIGHSCORE))
        {
            PlayerPrefs.SetInt(HIGHSCORE, 0);
        }

        countdownText.text = "- HIGHSCORE -\nSTAGE " + PlayerPrefs.GetInt(HIGHSCORE) + "!";

        yield return FadeIn(1.0f);

        while (true)
        {
            yield return new WaitForSeconds(longDuration);

            isCorrect = true;

            countdownText.text = "STAGE " + stage + "!";
            yield return new WaitForSeconds(mediumDuration);

            yield return Countdown(countdown);
            
            sequencer.Generate(stage);
            sequencer.Play();

            while (sequencer.isPlaying) yield return null;

            sequencer.EnableAll();

            isAttempting = true;

            while (isAttempting)
            {
                yield return null;
            }

            sequencer.DisableAll();

            if (isCorrect)
            {
                stage++;
            }
            else
            {
                audioSource.Play();

                if (--attempts != 0)
                {
                    var grammar = attempts > 1 ? " ATTEMPTS" : " ATTEMPT";
                    countdownText.text = attempts + grammar + "\nREMAINING!";
                }
                else
                {
                    if (PlayerPrefs.GetInt(HIGHSCORE) < stage)
                    {
                        PlayerPrefs.SetInt(HIGHSCORE, (int)stage);
                        countdownText.text = winMessages[UnityEngine.Random.Range(0, winMessages.Length)];
                    }
                    else
                    {
                        countdownText.text = loseMessages[UnityEngine.Random.Range(0, loseMessages.Length)];
                    }

                    yield return new WaitForSeconds(longDuration);

                    countdownText.text = "YOU REACHED\nSTAGE " + stage + "!";

                    yield return new WaitForSeconds(longDuration);

                    yield return FadeOut(1.0f);

                    SceneManager.LoadScene("Scene_0");
                }
            }
        }
    }

    public void Attempt(Pad pad)
    {
        if (!isAttempting) return;

        switch (sequencer.Guess(pad))
        {
            case Sequencer.Result.COMPLETE:
                isAttempting = false;
                break;

            case Sequencer.Result.INCORRECT:
                isAttempting = isCorrect = false;
                break;
        }
    }

    private IEnumerator Countdown(uint count)
    {
        for ( /* ... */ ; count > 0; count--)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(shortDuration);
        }

        countdownText.text = "";
    }

    public Texture2D fadeImage;
    
    public float fadeAlpha { get; private set; }  = 1.0f;

    private IEnumerator FadeIn(float duration)
    {
        duration = duration > float.Epsilon ? duration : float.Epsilon;

        fadeAlpha = 1.0f;

        while (fadeAlpha > float.Epsilon)
        {
            fadeAlpha -= Time.deltaTime / duration;
            yield return null;
        }

        fadeAlpha = 0.0f;
    }

    private IEnumerator FadeOut(float duration)
    {
        duration = duration > float.Epsilon ? duration : float.Epsilon;

        fadeAlpha = 0.0f;

        while (fadeAlpha < 1.0f)
        {
            fadeAlpha += Time.deltaTime / duration;
            yield return null;
        }

        fadeAlpha = 1.0f;
    }

    private void OnGUI()
    {
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);

        GUI.color = new Color(1.0f, 1.0f, 1.0f, fadeAlpha);
        GUI.DrawTexture(rect, fadeImage, ScaleMode.StretchToFill);
    }
}