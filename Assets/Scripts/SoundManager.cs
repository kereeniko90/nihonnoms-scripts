using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    
    public static SoundManager Instance { get; private set;}
    public enum Sound {
        BuildingPlaced,
        BuildingDestroyed,
        BuildingDamaged,
        EnemyDie,
        EnemyHit,
        EnemyWaveStarting,
        GameOver,
        Blob,
        Spawn,
        ConchShell,
        BuildingDone,
        BossSpawn,
        BossDie,
    }
    private AudioSource audioSource;

    private Dictionary< Sound, AudioClip> soundDictionary;
    private float volume = 0.5f;
    private void Awake() {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat("soundVolume", 0.5f);

        soundDictionary = new Dictionary< Sound, AudioClip >();

        foreach (Sound sound in System.Enum.GetValues(typeof(Sound))) {

            soundDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
            
        }
    }

    public void PlaySound(Sound sound) {
        audioSource.PlayOneShot(soundDictionary[sound], volume);
    }

    public void IncreaseVolume() {
        volume += 0.1f;
        volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("soundVolume", volume);
    }

    public void DecreaseVolume() {
        volume -= 0.1f;
        volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("soundVolume", volume);
    }

    public float GetVolume() {
        return volume;
    }
}
