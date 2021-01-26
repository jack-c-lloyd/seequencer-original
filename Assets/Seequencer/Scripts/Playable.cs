using System.Collections;
using UnityEngine;

public abstract class Playable : MonoBehaviour
{
    /* Whether or not this is playing; only internal. */
    public bool isPlaying { get; private set; } = false;

    public void Play()
    {
        /* If not playing, invoke the coroutine whilst setting the state. */
        if (!isPlaying) isPlaying = StartCoroutine(PlayCoroutine()) != null;
    }

    private IEnumerator PlayCoroutine()
    {
        yield return IEPlay();

        /* Reset state. */
        isPlaying = false;
    }

    protected abstract IEnumerator IEPlay(); /* Required. */
}