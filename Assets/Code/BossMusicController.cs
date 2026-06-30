using UnityEngine;

public class BossMusicController : MonoBehaviour
{
    public AudioSource zoneAudioSource;
    public AudioClip bossFightMusic;
    public AudioClip victoryMusic;
    public GameObject bossObject;

    private bool isBossDefeated = false;

    void Start()
    {
        // ตั้งค่าให้เล่นเพลงตอนสู้บอสเป็นค่าเริ่มต้น
        if (zoneAudioSource != null && bossFightMusic != null)
        {
            zoneAudioSource.clip = bossFightMusic;
            zoneAudioSource.loop = true;
        }
    }

    void Update()
    {
        // เช็คว่าถ้าตัวบอสถูกทำลายหายไปจากฉาก (เป็น null) และยังไม่ได้เปลี่ยนเพลง
        if (bossObject == null && !isBossDefeated)
        {
            isBossDefeated = true;
            PlayVictoryMusic();
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