using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Director : MonoBehaviour
{
    public TextMesh count;
    public Sequencer sequencer;
    public Texture2D fadeImage;

    private float fadeAlpha = 1;
    private bool waiting = false;
    private bool correct = true;

    private void Start()
    {
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        uint stage = 1;
        uint lives = 3;

        sequencer.DisableAll();

        if (!PlayerPrefs.HasKey("HIGHSCORE"))
        {
            PlayerPrefs.SetInt("HIGHSCORE", 0);
        }

        count.text = "- HIGHSCORE -\nSTAGE " + PlayerPrefs.GetInt("HIGHSCORE") + "!";

        while (fadeAlpha > 0)
        {
            fadeAlpha -= Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2.1f);

        while (true)
        {
            correct = true;

            count.text = "STAGE " + stage + "!";
            yield return new WaitForSeconds(1.4f);
            count.text = "3";
            yield return new WaitForSeconds(0.7f);
            count.text = "2";
            yield return new WaitForSeconds(0.7f);
            count.text = "1";
            yield return new WaitForSeconds(0.7f);
            count.text = "";
            
            sequencer.Generate(stage);
            sequencer.Play();

            while (sequencer.isPlaying) yield return null;

            sequencer.EnableAll();

            waiting = true;

            while (waiting) yield return null;

            sequencer.DisableAll();

            if (!correct)
            {
                lives--;

                GetComponent<AudioSource>().Play();

                if (lives != 0)
                {
                    count.text = lives + (lives > 1 ? " ATTEMPTS" : " ATTEMPT") + "\nREMAINING!";
                    yield return new WaitForSeconds(2.1f);
                }
                else
                {
                    int highscore = PlayerPrefs.GetInt("HIGHSCORE");

                    if (highscore < stage)
                    {
                        PlayerPrefs.SetInt("HIGHSCORE", (int)stage);
                        count.text = "NEW HIGHSCORE!";
                    }
                    else
                    {
                        count.text = "YOU LOSE!";
                    }

                    yield return new WaitForSeconds(2.1f);
                    count.text = "YOU REACHED\nSTAGE " + stage + "!";
                    yield return new WaitForSeconds(2.1f);

                    while (fadeAlpha < 1)
                    {
                        fadeAlpha += Time.deltaTime;
                        yield return null;
                    }

                    SceneManager.LoadScene("Menu");
                }
            }
            else
            {
                yield return new WaitForSeconds(2.1f);
                stage++;
            }
        }
    }

    public void Guess(Pad pad)
    {
        Sequencer.Result result = sequencer.Guess(pad);

        switch (result)
        {
            case Sequencer.Result.COMPLETE:
                waiting = false;
                break;
            case Sequencer.Result.INCORRECT:
                waiting = correct = false;
                break;
        }
    }

    private void OnGUI()
    {
        GUI.color = new Color(1.0f, 1.0f, 1.0f, fadeAlpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeImage, ScaleMode.StretchToFill);
    }
}