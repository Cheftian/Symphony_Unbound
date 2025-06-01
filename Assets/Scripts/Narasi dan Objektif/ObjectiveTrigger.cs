using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    public int objectiveID;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ObjectiveManager.Instance.ShowObjective(objectiveID);
            Destroy(gameObject); // Hancurkan trigger setelah dipicu
        }
    }
}
