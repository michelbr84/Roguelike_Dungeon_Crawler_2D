using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("SFX Clips")]
    public AudioClip moveSound;
    public AudioClip shootSound;
    public AudioClip hitWallSound;
    public AudioClip enemyMoveSound;
    public AudioClip enemyDeathSound;
    public AudioClip playerDeathSound;
    public AudioClip exitReachedSound;
    public AudioClip powerUpSound;     // Novo: ao pegar power-up (pode ser o mesmo que victorySound)
    public AudioClip victorySound;     // Novo: vitória ou coletável importante
    public AudioClip failSound;        // Novo: erro de ação (ex: sem munição)

    // Novos SFX para power-ups
    public AudioClip doubleShotSound;
    public AudioClip tripleShotSound;
    public AudioClip speedBoostSound;
    public AudioClip invisibilitySound;
    public AudioClip teleportSound;
    public AudioClip scoreBoosterSound;

    [Header("Music")]
    public AudioClip backgroundMusic;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    private void Awake()
    {
        // Singleton para facilitar acesso
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip && sfxSource)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic()
    {
        if (backgroundMusic && musicSource)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
    
    // Métodos de volume para compatibilidade com MazeSaveSystem
    public void SetMasterVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        AudioListener.volume = volume;
    }
    
    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        if (musicSource)
            musicSource.volume = volume;
    }
    
    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        if (sfxSource)
            sfxSource.volume = volume;
    }
    
    public float GetMasterVolume()
    {
        return AudioListener.volume;
    }
    
    public float GetMusicVolume()
    {
        return musicSource ? musicSource.volume : 1f;
    }
    
    public float GetSFXVolume()
    {
        return sfxSource ? sfxSource.volume : 1f;
    }
}
