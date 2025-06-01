using UnityEngine;

public class NarrativeTrigger : MonoBehaviour
{
    public int narrativeID;
    private bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            NarrativeManager.Instance.ShowNarrative(narrativeID);
            hasTriggered = true;
        }
    }
}
