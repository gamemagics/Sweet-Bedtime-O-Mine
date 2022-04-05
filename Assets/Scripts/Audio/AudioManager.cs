using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource audioSource;
    [SerializeField]
    private AudioClip shootAudio, hurtAudio, attackAudio, collectAudio, monsterHurtAudio;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);

    }

    public void ShootAudio()
    {
        audioSource.clip = shootAudio;
        audioSource.Play();
    }

    public void AttackAudio()
    {
        audioSource.clip = attackAudio;
        audioSource.Play();
    }

    public void HurtAudio()
    {
        audioSource.clip = hurtAudio;
        audioSource.Play();
    }

    public void CollectAudio()
    {
        audioSource.clip = collectAudio;
        audioSource.Play();
    }
    public void MonsterHurtAudio()
    {
        audioSource.clip = monsterHurtAudio;
        audioSource.Play();
    }
}
