using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleporter : MonoBehaviour
{
    public Transform destination;
    public GameObject bossCamera; // Slot for the Boss Camera
    public float showBossTime = 2.5f; // How long to look at the boss (in seconds)

    private GameObject playerInZone;
    private bool isTeleporting = false;

    void Update()
    {
        if (playerInZone != null && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame && !isTeleporting)
        {
            if (destination != null)
            {
                StartCoroutine(TeleportSequence());
            }
        }
    }

    private IEnumerator TeleportSequence()
    {
        isTeleporting = true;

        // 1. Move player to the destination door
        playerInZone.transform.position = destination.position;

        // 2. If we have a boss camera, show the boss
        if (bossCamera != null)
        {
            bossCamera.SetActive(true);

            // Wait for a few seconds to let player see the boss
            yield return new WaitForSeconds(showBossTime);

            // Turn off boss camera, Cinemachine will fade back to Player automatically
            bossCamera.SetActive(false);
        }

        // Wait a little bit before allowing another teleport
        yield return new WaitForSeconds(0.5f);
        isTeleporting = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerInZone == other.gameObject)
            {
                playerInZone = null;
            }
        }
    }
}