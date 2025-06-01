using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatePuzzle : MonoBehaviour
{
    public PressurePlate[] plates; // Array untuk semua pressure plate
    public int[] correctSequence; // Urutan yang benar (atur di Inspector)
    public GameObject barrier; // Objek penghalang yang akan dihilangkan
    public GameObject reward; // Objek penghalang yang akan dihilangkan

    private int currentIndex = 0; // Indeks urutan yang harus diikuti

    void Start()
    {
        foreach (PressurePlate plate in plates)
        {
            plate.SetPuzzle(this); // Hubungkan setiap plate dengan puzzle utama
        }
            if (reward != null)
        {
            reward.SetActive(false); // Hilangkan penghalang
        }
    }

    public void ActivatePlate(int plateID)
    {
        if (correctSequence[currentIndex] == plateID)
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
    }

    private void ResetPuzzle()
    {
        currentIndex = 0; // Reset urutan jika salah
    }
}
