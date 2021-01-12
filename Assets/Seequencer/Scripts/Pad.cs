using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]

public class Pad : Interactable
{
    public Light light;

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
        GetComponent<AudioSource>().clip = audioClip;
        GetComponent<AudioSource>().Play();
        GetComponent<MeshRenderer>().material.color = color;
        light.color = color;
        light.enabled = true;
        yield return new WaitForSeconds(duration);
        light.enabled = false;
        GetComponent<MeshRenderer>().material.color = Color.grey;
        isPlaying = false;
    }

    public override void Interact()
    {
        FindObjectOfType<Director>().Guess(this);
    }
}