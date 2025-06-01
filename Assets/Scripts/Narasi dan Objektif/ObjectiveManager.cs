using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;
    
    public TextMeshProUGUI objectiveText;
    public RectTransform objectiveBox; // UI Box untuk animasi
    public float slideSpeed = 0.5f;

    private Dictionary<int, string> objectives = new Dictionary<int, string>();
    private Coroutine currentCoroutine;

    [System.Serializable]
    public class ObjectiveEntry
    {
        public int id;
        public string text;
    }

    public List<ObjectiveEntry> objectiveEntries = new List<ObjectiveEntry>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        objectiveBox.gameObject.SetActive(false);
        InitializeObjectives();
    }

    void InitializeObjectives()
    {
        objectives.Clear();
        foreach (var entry in objectiveEntries)
        {
            objectives[entry.id] = entry.text;
        }
    }

    public void ShowObjective(int id)
    {
        if (objectives.ContainsKey(id))
        {
            string newText = objectives[id];

            if (newText.ToLower() == "none") // Jika objektif adalah "none", maka sembunyikan box
            {
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                }
                currentCoroutine = StartCoroutine(SlideOut());
            }
            else
            {
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                    StartCoroutine(SlideOutAndShowNewObjective(newText));
                }
                else
                {
                    currentCoroutine = StartCoroutine(DisplayObjective(newText));
                }
            }
        }
    }

    IEnumerator DisplayObjective(string text)
    {
        objectiveText.text = text;
        objectiveBox.gameObject.SetActive(true);

        // Animasi Slide In (Dari kiri ke kanan)
        float startX = -objectiveBox.rect.width;
        float endX = 0f; // Posisi masuk ke layar
        float elapsedTime = 0f;

        while (elapsedTime < slideSpeed)
        {
            float newX = Mathf.Lerp(startX, endX, elapsedTime / slideSpeed);
            objectiveBox.anchoredPosition = new Vector2(newX, objectiveBox.anchoredPosition.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        objectiveBox.anchoredPosition = new Vector2(endX, objectiveBox.anchoredPosition.y);
    }

    IEnumerator SlideOutAndShowNewObjective(string newText)
    {
        yield return StartCoroutine(SlideOut());

        // Jika objektif baru adalah "none", maka berhenti di sini
        if (newText.ToLower() == "none")
            yield break;

        // Tampilkan objective baru setelah slide out
        currentCoroutine = StartCoroutine(DisplayObjective(newText));
    }

    IEnumerator SlideOut()
    {
        // Animasi Slide Out (Ke kiri)
        float startX = objectiveBox.anchoredPosition.x;
        float endX = -objectiveBox.rect.width;
        float elapsedTime = 0f;

        while (elapsedTime < slideSpeed)
        {
            float newX = Mathf.Lerp(startX, endX, elapsedTime / slideSpeed);
            objectiveBox.anchoredPosition = new Vector2(newX, objectiveBox.anchoredPosition.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectiveBox.anchoredPosition = new Vector2(endX, objectiveBox.anchoredPosition.y);
        objectiveBox.gameObject.SetActive(false);
    }
}
