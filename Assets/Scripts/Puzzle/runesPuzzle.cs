using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runesPuzzle : MonoBehaviour
{
    public runes[] runes; // Array untuk batu runes
    public int[] correctSequence; // Urutan yang benar (atur di Inspector)
    public GameObject barrier; // Objek penghalan g yang akan dihilangkan
    public GameObject reward;

    private int currentIndex = 0; // Indeks urutan yang harus diikuti
    private bool puzzleCompleted = false;

    void Start()
    {
        foreach (runes rune in runes)
        {
            rune.SetPuzzle(this); // Hubungkan runes dengan puzzle utama
        }
        if (reward != null)
        {
            reward.SetActive(false); // Hilangkan penghalang
        }
    }

    public void ActivateRune(int runeID)
    {
        if (!puzzleCompleted)
        {
            if (correctSequence[currentIndex] == runeID)
            {
                currentIndex++;
                if (currentIndex >= correctSequence.Length)
                {
                    CompletePuzzle();
                }
            }
            else
            {
                ResetPuzzle();
            }
        }
    }

    private void CompletePuzzle()
    {
        if (barrier != null)
        {
            barrier.SetActive(false); // Hilangkan penghalang
        }
        if (reward != null)
        {
            reward.SetActive(true); // Hilangkan penghalang
        }
        puzzleCompleted = true;
    }

        private void ResetPuzzle()
    {
        currentIndex = 0;
        NarrativeManager.Instance.ShowNarrative(30); // Reset urutan jika salah
        // foreach (runes rune in runes)
        // {
        //     rune.ResetRune();
        // }
    }
}
