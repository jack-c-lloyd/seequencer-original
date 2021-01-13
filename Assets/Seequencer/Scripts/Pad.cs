using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]

public class Pad : Interactable
{
    public Light light;

    private AudioSource audioSource;
    private MeshRenderer meshRenderer;
    private Director director;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        meshRenderer = GetComponent<MeshRenderer>();
        director = FindObjectOfType<Director>();
    }

    public bool isPlaying { get; private set; } = false;

    public void Play(AudioClip audioClip, Color color, float duration)
    {
        if (!isPlaying)
        {
            isPlaying = StartCoroutine(IEPlay(audioClip, color, duration)) != null ? true : false;
        }
    }

    private IEnumerator IEPlay(AudioClip audioClip, Color color, float duration)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        meshRenderer.material.color = color;
        light.color = color;
        light.enabled = true;
        yield return new WaitForSeconds(duration);
        light.enabled = false;
        meshRenderer.material.color = Color.grey;
        isPlaying = false;
    }

    public override void Interact()
    {
        director.Attempt(this);
    }
}