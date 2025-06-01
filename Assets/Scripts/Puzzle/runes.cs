using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runes : MonoBehaviour
{
    public int runeID;
    public float glowDuration = 2f;
    public AudioClip soundEffect;

    private runesPuzzle puzzle;
    private Renderer runeRenderer;
    private AudioSource audioSource;

    void Start()
    {
        runeRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetPuzzle(runesPuzzle puzzleScript)
    {
        puzzle = puzzleScript;
    }

    public void ActivateRune()
    {
        Debug.Log("Activating rune: " + runeID);
        puzzle.ActivateRune(runeID);
        StartCoroutine(GlowRune());
    }

    private IEnumerator GlowRune()
    {
        runeRenderer.material.SetColor("_EmissionColor", Color.yellow);
        if (soundEffect != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundEffect);
        }
        yield return new WaitForSeconds(glowDuration);
        runeRenderer.material.SetColor("_EmissionColor", Color.black);
    }
}