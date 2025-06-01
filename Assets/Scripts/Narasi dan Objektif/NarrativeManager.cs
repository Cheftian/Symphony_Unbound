using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Pastikan menggunakan TextMeshPro untuk tampilan UI yang lebih baik

public class NarrativeManager : MonoBehaviour
{
    public static NarrativeManager Instance;
    public TextMeshProUGUI narrativeText;
    public float displayDuration = 5f;
    public List<NarrativeEntry> narrativeEntries = new List<NarrativeEntry>();
    private Dictionary<int, string> narratives = new Dictionary<int, string>();
    private Coroutine currentCoroutine;
    // [SerializeField] private GameObject popupObject; // GameObject yang akan muncul saat pemain masuk
    
    // [SerializeField] private AudioClip narrativeSound;
    private AudioSource audioSource;

    [System.Serializable]
    public class NarrativeEntry
    {
        public int id;
        public string text;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // if (popupObject != null)
        // {
            // popupObject.SetActive(false); // Sembunyikan saat awal
        // }
        narrativeText.gameObject.SetActive(false);
        InitializeNarratives();
        audioSource = GetComponent<AudioSource>();
    }

    void InitializeNarratives()
    {
        narratives.Clear();
        foreach (var entry in narrativeEntries)
        {
            narratives[entry.id] = entry.text;
        }
    }

    public void ShowNarrative(int id)
    {
        if (narratives.ContainsKey(id))
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            // if (narrativeSound != null && audioSource != null)
            // {
                // audioSource.PlayOneShot(narrativeSound);
            // }
            currentCoroutine = StartCoroutine(DisplayNarrative(narratives[id]));
        }
    }

    IEnumerator DisplayNarrative(string text)
    {
        narrativeText.text = text;
        narrativeText.gameObject.SetActive(true);
        // if (popupObject != null)
        // {
            // popupObject.SetActive(true);
        // }
        yield return new WaitForSeconds(displayDuration);
        narrativeText.gameObject.SetActive(false);
        // if (popupObject != null)
        // {
            // popupObject.SetActive(false);
        // }
    }
}
