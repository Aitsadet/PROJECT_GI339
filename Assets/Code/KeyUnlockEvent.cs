using System.Collections;
using UnityEngine;

public class KeyUnlockEvent : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject doorCamera;

    [Header("Target Object")]
    public GameObject barrierToDestroy;

    [Header("Timings")]
    public float timeToPanToDoor = 1.5f;
    public float timeToLookAtDoor = 1.0f;

    private bool isCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            StartCoroutine(UnlockSequence());
        }
    }

    private IEnumerator UnlockSequence()
    {
        if (doorCamera != null)
        {
            doorCamera.SetActive(true);
        }

        yield return new WaitForSeconds(timeToPanToDoor);

        if (barrierToDestroy != null)
        {
            barrierToDestroy.SetActive(false);
        }

        yield return new WaitForSeconds(timeToLookAtDoor);

        if (doorCamera != null)
        {
            doorCamera.SetActive(false);
        }

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}