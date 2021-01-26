using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class Sequencer : Playable
{
    public AudioClip[] audioClips = new AudioClip[6];

    public Color incorrect = Color.HSVToRGB(1.0f, 1, 1);
    public Color[] rainbow = { /* Seequencer rainbow: */
        Color.HSVToRGB(0.1f, 1, 1), /* S */
        Color.HSVToRGB(0.2f, 1, 1), /* E */
        Color.HSVToRGB(0.3f, 1, 1), /* E */
        Color.HSVToRGB(0.4f, 1, 1), /* Q */
        Color.HSVToRGB(0.5f, 1, 1), /* U */
        Color.HSVToRGB(0.6f, 1, 1), /* E */
        Color.HSVToRGB(0.7f, 1, 1), /* N */
        Color.HSVToRGB(0.8f, 1, 1), /* C */
        Color.HSVToRGB(0.9f, 1, 1), /* E */
        Color.HSVToRGB(1.0f, 1, 1)  /* R */
    };

    [Range(float.Epsilon, 3.0f)] public float duration = 2.0f;

    public static List<Pad> register { get; private set; } = new List<Pad>();

    private void Awake()
    {
        register.Clear();
        sequence.Clear();

        var arr = FindObjectsOfType<Pad>();

        register = new List<Pad>(arr);
    }

    public static void Register(Pad pad)
    {
        if (!register.Contains(pad))
        {
            register.Add(pad);
        }
    }

    public void DisableAll()
    {
        foreach (Pad pad in register)
        {
            pad.GetComponent<Collider>().enabled = false;
        }
    }

    public void EnableAll()
    {
        foreach (Pad pad in register)
        {
            pad.GetComponent<Collider>().enabled = true;
        }
    }

    public void PreviewAll(Color color, float duration)
    {
        foreach (Pad pad in register)
        {
            pad.Play(null, color, duration);
        }
    }

    public List<Tuple<Pad, AudioClip, Color>> sequence { get; private set; } = new List<Tuple<Pad, AudioClip, Color>>();

    private int index = 0;

    public void Generate(uint length)
    {
        sequence.Clear();
        index = 0;

        for (uint i = 0; i < length; i++)
        {
            Pad pad = register[UnityEngine.Random.Range(0, register.Count)];
            AudioClip audioClip = audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
            Color color = rainbow[UnityEngine.Random.Range(0, rainbow.Length)];

            sequence.Add(new Tuple<Pad, AudioClip, Color>(pad, audioClip, color));
        }
    }

    protected override IEnumerator IEPlay()
    {
        foreach (var tuple in sequence)
        {
            Pad pad = tuple.Item1;

            pad.Play(tuple.Item2, tuple.Item3, duration);

            while (pad.isPlaying)
            {
                yield return null;
            }

            yield return new WaitForSeconds(1);
        }
    }

    public enum Result
    {
        COMPLETE,
        CORRECT,
        INCORRECT
    }

    public Result Guess(Pad pad)
    {
        if (pad == sequence[index].Item1)
        {
            pad.Play(sequence[index].Item2, sequence[index].Item3, duration);

            return ++index < sequence.Count ? Result.CORRECT : Result.COMPLETE;
        }

        PreviewAll(incorrect, duration);

        return Result.INCORRECT;
    }
}
