using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {
    private AudioClip audio1;
    private AudioClip audio2;
    private AudioClip audio3;

    private AudioSource audioSource;

    [SerializeField] MusicManager musicManager;
    public float fadeDuration = 2.0f; 
    public List<AudioClip> musicClips; 

    
    private List<AudioClip> shuffledClips;
    private int currentIndex = 0;
    private List<AudioSource> audioSources;
    private int currentSourceIndex = 0;

    private void Awake() {
        audio1 = Resources.Load<AudioClip>("Nihon Noms - Soundtrack 1");
        audio2 = Resources.Load<AudioClip>("Nihon Noms - Soundtrack 2");
        audio3 = Resources.Load<AudioClip>("Nihon Noms - Soundtrack 3");
        
        musicClips = new List<AudioClip> { audio1, audio2, audio3 };
    }

    void Start() {
        
        audioSources = new List<AudioSource>();

    
        for (int i = 0; i < 3; i++) {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        audioSources.Add(newSource);
        }
        ShuffleClips();

        
        PlayNextClip();
    }

    private void Update() {
        
        float currentVolume = musicManager.GetVolume();

        
        foreach (AudioSource source in audioSources) {
            source.volume = currentVolume;
        }

        if (!audioSources[currentSourceIndex].isPlaying) {
            PlayNextClip(); // Play the next clip if the current one is finished
        }
    }

    
    private void ShuffleClips() {
        shuffledClips = new List<AudioClip>(musicClips);

        for (int i = 0; i < shuffledClips.Count; i++)
        {
            AudioClip temp = shuffledClips[i];
            int randomIndex = Random.Range(i, shuffledClips.Count);
            shuffledClips[i] = shuffledClips[randomIndex];
            shuffledClips[randomIndex] = temp;
        }

        currentIndex = 0; 
    }

    private void PlayNextClip() {
        if (shuffledClips == null || shuffledClips.Count == 0) {
            Debug.LogWarning("No shuffled clips available to play.");
            return; // Exit if no clips to play
        }

        if (currentIndex >= shuffledClips.Count) {
            // Reset and shuffle again if we reach the end of the list
            ShuffleClips();
        }

        // Play the next clip
        AudioClip nextClip = shuffledClips[currentIndex];
        currentIndex++; // Move to the next clip for the next call
        CrossfadeTo(nextClip);
    }

    
    public void CrossfadeTo(AudioClip newClip) {
        int nextSourceIndex = (currentSourceIndex + 1) % audioSources.Count; // Get the next audio source index
        StartCoroutine(FadeTracks(audioSources[currentSourceIndex], audioSources[nextSourceIndex], newClip));
        currentSourceIndex = nextSourceIndex; // Update the current source index
    }

    
    private IEnumerator FadeTracks(AudioSource fromSource, AudioSource toSource, AudioClip newClip) {
        toSource.clip = newClip;
        toSource.Play();

        float time = 0;
        while (time < fadeDuration) {
            fromSource.volume = Mathf.Lerp(1, 0, time / fadeDuration); 
            toSource.volume = Mathf.Lerp(0, 1, time / fadeDuration); 
            time += Time.deltaTime;
            yield return null;
        }

        fromSource.Stop(); 
        
    }

    
    public void PlayNextMusicTrack() {
        PlayNextClip();
    }
}
