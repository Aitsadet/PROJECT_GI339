using UnityEngine;

public class BossMusicController : MonoBehaviour
{
    public AudioSource zoneAudioSource;
    public AudioClip bossFightMusic;
    public AudioClip victoryMusic;
    public GameObject bossObject;
    public GameObject goalDoor; // เพิ่มตัวแปรประตูเข้ามา

    private bool isBossDefeated = false;

    void Start()
    {
        if (zoneAudioSource != null && bossFightMusic != null)
        {
            zoneAudioSource.clip = bossFightMusic;
            zoneAudioSource.loop = true;
        }
    }

    void Update()
    {
        if (bossObject == null && !isBossDefeated)
        {
            isBossDefeated = true;
            PlayVictoryMusic();

            // ปลุกประตูให้โผล่ขึ้นมา!
            if (goalDoor != null)
            {
                goalDoor.SetActive(true);
            }
        }
    }

    private void PlayVictoryMusic()
    {
        if (zoneAudioSource != null && victoryMusic != null)
        {
            zoneAudioSource.Stop();
            zoneAudioSource.clip = victoryMusic;
            zoneAudioSource.Play();
        }
    }
}