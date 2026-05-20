using UnityEngine;

public class KillZone : MonoBehaviour
{
    private SeparationManager separationManager;

    void Start()
    {
        separationManager = FindFirstObjectByType<SeparationManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
            separationManager?.OnInstantKill();
    }
}
