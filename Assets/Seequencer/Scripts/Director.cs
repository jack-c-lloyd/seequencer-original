using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]

[RequireComponent(typeof(AudioSource))]

public class Director : MonoBehaviour
{
    [Range(float.Epsilon, 2.1f)] public float shortDuration = 0.7f;
    [Range(float.Epsilon, 2.1f)] public float mediumDuration = 1.4f;
    [Range(float.Epsilon, 2.1f)] public float longDuration = 2.1f;

    public TextMesh textMesh;
    public Sequencer sequencer;
    public Texture2D fadeImage;

    private float fadeAlpha = 1;
    private bool isIdle = false;
    private bool isCorrect = true;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(Direct());
    }

    private IEnumerator Direct()
    {
        uint lives = 3;
        uint stage = 1;

        sequencer.DisableAll();

        if (!PlayerPrefs.HasKey("HIGHSCORE"))
        {
            PlayerPrefs.SetInt("HIGHSCORE", 0);
        }

        textMesh.text = "- HIGHSCORE -\nSTAGE " + PlayerPrefs.GetInt("HIGHSCORE") + "!";

        while (fadeAlpha > float.Epsilon)
        {
            fadeAlpha -= Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(longDuration);

        while (true)
        {
            isCorrect = true;
            isIdle = true;

            textMesh.text = "STAGE " + stage + "!";
            yield return new WaitForSeconds(mediumDuration);
            textMesh.text = "3";
            yield return new WaitForSeconds(shortDuration);
            textMesh.text = "2";
            yield return new WaitForSeconds(shortDuration);
            textMesh.text = "1";
            yield return new WaitForSeconds(shortDuration);
            textMesh.text = "";
            
            sequencer.Generate(stage);
            sequencer.Play();

            while (sequencer.isPlaying) yield return null;

            sequencer.EnableAll();

            while (isIdle)
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

                lives--;

                if (lives != 0)
                {
                    var grammar = lives > 1 ? " ATTEMPTS" : " ATTEMPT";
                    textMesh.text = lives + grammar + "\nREMAINING!";

                    yield return new WaitForSeconds(longDuration);
                }
                else
                {
                    int highscore = PlayerPrefs.GetInt("HIGHSCORE");

                    if (highscore < stage)
                    {
                        PlayerPrefs.SetInt("HIGHSCORE", (int)stage);
                        textMesh.text = "NEW HIGHSCORE!";
                    }
                    else
                    {
                        textMesh.text = "YOU LOSE!";
                    }

                    yield return new WaitForSeconds(longDuration);

                    textMesh.text = "YOU REACHED\nSTAGE " + stage + "!";

                    yield return new WaitForSeconds(longDuration);

                    while (fadeAlpha < 1)
                    {
                        fadeAlpha += Time.deltaTime;
                        yield return null;
                    }

                    SceneManager.LoadScene("Menu");
                }
            }

            yield return new WaitForSeconds(longDuration);
        }
    }

    public void Attempt(Pad pad)
    {
        switch (sequencer.Guess(pad))
        {
            case Sequencer.Result.COMPLETE:
                isIdle = false;
                break;

            case Sequencer.Result.INCORRECT:
                isIdle = isCorrect = false;
                break;
        }
    }

    private void OnGUI()
    {
        GUI.color = new Color(1.0f, 1.0f, 1.0f, fadeAlpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeImage, ScaleMode.StretchToFill);
    }
}