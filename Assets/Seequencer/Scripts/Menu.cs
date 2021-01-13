using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : Interactable
{
    public Texture2D fadeImage;
    private float fadeAlpha = 1;
    public AudioSource audioSource;
    private float volume = 1;

    public bool isPlaying { get; private set; } = false;

    private void Awake()
    {
        StartCoroutine(Intro());
    }

    public override void Interact()
    {
        if (!isPlaying)
        {
            isPlaying = StartCoroutine(Outro()) != null ? true : false;
        }
    }

    private IEnumerator Intro()
    {
        while (fadeAlpha > 0)
        {
            fadeAlpha -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Outro()
    {
        GetComponent<AudioSource>().Play();

        while (fadeAlpha < 1)
        {
            fadeAlpha += Time.deltaTime;
            volume -= Time.deltaTime;
            audioSource.volume = volume;
            yield return null;
        }

        SceneManager.LoadScene("Demo");
    }

    private void OnGUI()
    {
        GUI.color = new Color(1.0f, 1.0f, 1.0f, fadeAlpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeImage, ScaleMode.StretchToFill);
    }
}
