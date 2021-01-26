using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : Interactable
{
    public Texture2D fadeImage;
    public AudioSource theme;

    private float fadeAlpha = 1;
    private float volume = 1;

    private AudioSource audioSource;

    public bool isPlaying { get; private set; } = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
        audioSource.Play();

        while (fadeAlpha < 1)
        {
            fadeAlpha += Time.deltaTime;
            volume -= Time.deltaTime;
            theme.volume = volume;
            yield return null;
        }

        SceneManager.LoadScene("Scene_1");
    }

    private void OnGUI()
    {
        GUI.color = new Color(1.0f, 1.0f, 1.0f, fadeAlpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeImage, ScaleMode.StretchToFill);
    }
}
