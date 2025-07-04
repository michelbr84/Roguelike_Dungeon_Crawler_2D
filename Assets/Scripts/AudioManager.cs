using UnityEngine;
using System.Collections.Generic; // Added for List

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
    public AudioClip[] additionalMusic; // Músicas adicionais para variedade
    
    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;
    
    // Sistema de múltiplas músicas
    private int currentMusicIndex = 0;
    private AudioClip[] allMusic;

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
        
        // Inicializar array de músicas
        InitializeMusicArray();
    }
    
    private void InitializeMusicArray()
    {
        // Combinar música principal com músicas adicionais
        List<AudioClip> musicList = new List<AudioClip>();
        if (backgroundMusic != null)
            musicList.Add(backgroundMusic);
        
        if (additionalMusic != null)
        {
            foreach (var music in additionalMusic)
            {
                if (music != null)
                    musicList.Add(music);
            }
        }
        
        allMusic = musicList.ToArray();
        
        // Carregar índice salvo
        currentMusicIndex = PlayerPrefs.GetInt("CurrentMusicIndex", 0);
        if (currentMusicIndex >= allMusic.Length)
            currentMusicIndex = 0;
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
        if (allMusic != null && allMusic.Length > 0 && musicSource)
        {
            musicSource.clip = allMusic[currentMusicIndex];
            musicSource.loop = true;
            musicSource.Play();
        }
    }
    
    // Trocar para próxima música
    public void NextMusic()
    {
        if (allMusic != null && allMusic.Length > 1)
        {
            currentMusicIndex = (currentMusicIndex + 1) % allMusic.Length;
            PlayerPrefs.SetInt("CurrentMusicIndex", currentMusicIndex);
            PlayerPrefs.Save();
            PlayMusic();
        }
    }
    
    // Trocar para música anterior
    public void PreviousMusic()
    {
        if (allMusic != null && allMusic.Length > 1)
        {
            currentMusicIndex = (currentMusicIndex - 1 + allMusic.Length) % allMusic.Length;
            PlayerPrefs.SetInt("CurrentMusicIndex", currentMusicIndex);
            PlayerPrefs.Save();
            PlayMusic();
        }
    }
    
    // Trocar para música específica
    public void SetMusic(int index)
    {
        if (allMusic != null && index >= 0 && index < allMusic.Length)
        {
            currentMusicIndex = index;
            PlayerPrefs.SetInt("CurrentMusicIndex", currentMusicIndex);
            PlayerPrefs.Save();
            PlayMusic();
        }
    }
    
    // Obter informações sobre músicas
    public int GetCurrentMusicIndex() => currentMusicIndex;
    public int GetMusicCount() => allMusic != null ? allMusic.Length : 0;
    public string GetCurrentMusicName()
    {
        if (allMusic != null && currentMusicIndex < allMusic.Length && allMusic[currentMusicIndex] != null)
            return allMusic[currentMusicIndex].name;
        return "Nenhuma música";
    }
    
    // Trocar música baseada na dificuldade
    public void SetMusicByDifficulty(int difficultyLevel)
    {
        if (allMusic != null && allMusic.Length > 0)
        {
            // Música mais intensa para dificuldades maiores
            int musicIndex = Mathf.Clamp(difficultyLevel - 1, 0, allMusic.Length - 1);
            SetMusic(musicIndex);
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
