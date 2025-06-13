using UnityEngine;

public class DoorTrigger2 : MonoBehaviour
{
    public LightingController lightingController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lightingController.ActivateProfile("alegria");
        }
    }
}