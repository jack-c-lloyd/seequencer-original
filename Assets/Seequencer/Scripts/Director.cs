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
        uint level = 1;
        uint lives = 3;

        sequencer.DisableAll();

        while (fadeAlpha > 0)
        {
            fadeAlpha -= Time.deltaTime;
            yield return null;
        }

        count.text = "GET READY!";
        yield return new WaitForSeconds(2);

        while (true)
        {
            correct = true;

            count.text = "3";
            yield return new WaitForSeconds(1);
            count.text = "2";
            yield return new WaitForSeconds(1);
            count.text = "1";
            yield return new WaitForSeconds(1);
            count.text = "";
            
            sequencer.Generate(level);
            sequencer.Play();

            while (sequencer.isPlaying) yield return null;

            sequencer.EnableAll();

            waiting = true;

            while (waiting) yield return null;

            sequencer.DisableAll();

            if (!correct)
            {
                lives--;

                if (lives != 0)
                {
                    count.text = lives + (lives > 1 ? " ATTEMPTS" : " ATTEMPT") + "\nREMAINING!";
                    yield return new WaitForSeconds(3.0f);
                }
                else
                {
                    count.text = "LEVEL " + level + "!";
                    yield return new WaitForSeconds(3.0f);

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
                yield return new WaitForSeconds(3);
                level++;
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