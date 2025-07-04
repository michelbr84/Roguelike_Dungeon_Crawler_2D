using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

public static class MazeSaveSystem
{
    // Chaves para PlayerPrefs
    private const string KEY_SCORE = "MazeScore";
    private const string KEY_LEVEL = "MazeLevel";
    private const string KEY_LIVES = "MazeLives";
    private const string KEY_AMMO = "MazeAmmo";
    private const string KEY_RECORD = "MazeScoreRecord";
    private const string KEY_TOTAL_PLAY_TIME = "MazeTotalPlayTime";
    private const string KEY_TOTAL_KILLS = "MazeTotalKills";
    private const string KEY_TOTAL_POWERUPS = "MazeTotalPowerUps";
    private const string KEY_ACHIEVEMENTS = "MazeAchievements";
    private const string KEY_SETTINGS = "MazeSettings";
    private const string KEY_LAST_SAVE_TIME = "MazeLastSaveTime";
    private const string KEY_RANKING = "MazeRanking";
    private const int RANKING_SIZE = 10;
    
    // Dados de save
    [Serializable]
    public class SaveData
    {
        public int score;
        public int level;
        public int lives;
        public int ammo;
        public int record;
        public float totalPlayTime;
        public int totalKills;
        public int totalPowerUps;
        public string achievements;
        public string settings;
        public string lastSaveTime;
        
        public SaveData()
        {
            score = 0;
            level = 1;
            lives = 3;
            ammo = 10;
            record = 0;
            totalPlayTime = 0f;
            totalKills = 0;
            totalPowerUps = 0;
            achievements = "";
            settings = "";
            lastSaveTime = DateTime.Now.ToString();
        }
    }
    
    // Configurações
    [Serializable]
    public class GameSettings
    {
        public float masterVolume = 1f;
        public float musicVolume = 0.8f;
        public float sfxVolume = 1f;
        public bool touchControlsEnabled = true;
        public bool screenShakeEnabled = true;
        public bool particleEffectsEnabled = true;
        public int difficultyLevel = 1; // 1 = Fácil, 2 = Médio, 3 = Difícil
        
        public GameSettings()
        {
            masterVolume = 1f;
            musicVolume = 0.8f;
            sfxVolume = 1f;
            touchControlsEnabled = true;
            screenShakeEnabled = true;
            particleEffectsEnabled = true;
            difficultyLevel = 1;
        }
    }
    
    // Salvar dados do jogo
    public static void SaveGame(ProceduralMaze maze)
    {
        try
        {
            SaveData saveData = new SaveData();
            
            // Dados básicos do jogo
            saveData.score = maze.score;
            saveData.level = maze.currentLevel;
            saveData.lives = maze.lives;
            saveData.ammo = maze.ammo;
            saveData.record = maze.scoreRecord;
            
            // Estatísticas
            saveData.totalPlayTime = PlayerPrefs.GetFloat(KEY_TOTAL_PLAY_TIME, 0f) + Time.time;
            saveData.totalKills = PlayerPrefs.GetInt(KEY_TOTAL_KILLS, 0) + MazeAchievements.GetTotalKills();
            saveData.totalPowerUps = PlayerPrefs.GetInt(KEY_TOTAL_POWERUPS, 0) + MazeAchievements.GetTotalPowerUps();
            
            // Achievements
            saveData.achievements = MazeAchievements.SerializeAchievements();
            
            // Configurações
            saveData.settings = JsonUtility.ToJson(GetCurrentSettings());
            
            // Timestamp
            saveData.lastSaveTime = DateTime.Now.ToString();
            
            // Salvar no PlayerPrefs
            PlayerPrefs.SetString(KEY_SCORE, JsonUtility.ToJson(saveData));
            PlayerPrefs.Save();
            
            Debug.Log("Jogo salvo com sucesso!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao salvar jogo: {e.Message}");
        }
    }
    
    // Carregar dados do jogo
    public static bool LoadGame(ProceduralMaze maze)
    {
        try
        {
            if (!PlayerPrefs.HasKey(KEY_SCORE))
            {
                Debug.Log("Nenhum save encontrado. Iniciando novo jogo.");
                return false;
            }
            
            string saveJson = PlayerPrefs.GetString(KEY_SCORE);
            SaveData saveData = JsonUtility.FromJson<SaveData>(saveJson);
            
            if (saveData == null)
            {
                Debug.LogError("Erro ao deserializar dados de save.");
                return false;
            }
            
            // Restaurar dados básicos
            maze.score = saveData.score;
            maze.currentLevel = saveData.level;
            maze.lives = saveData.lives;
            maze.ammo = saveData.ammo;
            maze.scoreRecord = saveData.record;
            
            // Restaurar estatísticas
            PlayerPrefs.SetFloat(KEY_TOTAL_PLAY_TIME, saveData.totalPlayTime);
            PlayerPrefs.SetInt(KEY_TOTAL_KILLS, saveData.totalKills);
            PlayerPrefs.SetInt(KEY_TOTAL_POWERUPS, saveData.totalPowerUps);
            
            // Restaurar achievements
            if (!string.IsNullOrEmpty(saveData.achievements))
            {
                MazeAchievements.DeserializeAchievements(saveData.achievements);
            }
            
            // Restaurar configurações
            if (!string.IsNullOrEmpty(saveData.settings))
            {
                GameSettings settings = JsonUtility.FromJson<GameSettings>(saveData.settings);
                ApplySettings(settings);
            }
            
            Debug.Log("Jogo carregado com sucesso!");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao carregar jogo: {e.Message}");
            return false;
        }
    }
    
    // Salvar configurações
    public static void SaveSettings(GameSettings settings)
    {
        try
        {
            string settingsJson = JsonUtility.ToJson(settings);
            PlayerPrefs.SetString(KEY_SETTINGS, settingsJson);
            PlayerPrefs.Save();
            
            ApplySettings(settings);
            Debug.Log("Configurações salvas!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao salvar configurações: {e.Message}");
        }
    }
    
    // Carregar configurações
    public static GameSettings LoadSettings()
    {
        try
        {
            if (PlayerPrefs.HasKey(KEY_SETTINGS))
            {
                string settingsJson = PlayerPrefs.GetString(KEY_SETTINGS);
                GameSettings settings = JsonUtility.FromJson<GameSettings>(settingsJson);
                
                if (settings != null)
                {
                    ApplySettings(settings);
                    return settings;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao carregar configurações: {e.Message}");
        }
        
        // Retornar configurações padrão
        GameSettings defaultSettings = new GameSettings();
        ApplySettings(defaultSettings);
        return defaultSettings;
    }
    
    // Aplicar configurações
    private static void ApplySettings(GameSettings settings)
    {
        // Aplicar volumes
        if (AudioManager.Instance)
        {
            AudioManager.Instance.SetMasterVolume(settings.masterVolume);
            AudioManager.Instance.SetMusicVolume(settings.musicVolume);
            AudioManager.Instance.SetSFXVolume(settings.sfxVolume);
        }
        
        // Aplicar configurações de efeitos
        if (!settings.screenShakeEnabled)
        {
            // Desabilitar screen shake
        }
        
        if (!settings.particleEffectsEnabled)
        {
            // Desabilitar partículas
        }
        
        // Aplicar dificuldade
        // (implementar lógica de dificuldade)
    }
    
    // Obter configurações atuais
    public static GameSettings GetCurrentSettings()
    {
        GameSettings settings = new GameSettings();
        
        if (AudioManager.Instance)
        {
            settings.masterVolume = AudioManager.Instance.GetMasterVolume();
            settings.musicVolume = AudioManager.Instance.GetMusicVolume();
            settings.sfxVolume = AudioManager.Instance.GetSFXVolume();
        }
        
        return settings;
    }
    
    // Salvar apenas o recorde
    public static void SaveRecord(int record)
    {
        PlayerPrefs.SetInt(KEY_RECORD, record);
        PlayerPrefs.Save();
    }
    
    // Carregar recorde
    public static int LoadRecord()
    {
        return PlayerPrefs.GetInt(KEY_RECORD, 0);
    }
    
    // Salvar estatísticas
    public static void SaveStatistics(float playTime, int kills, int powerUps)
    {
        float totalPlayTime = PlayerPrefs.GetFloat(KEY_TOTAL_PLAY_TIME, 0f) + playTime;
        int totalKills = PlayerPrefs.GetInt(KEY_TOTAL_KILLS, 0) + kills;
        int totalPowerUps = PlayerPrefs.GetInt(KEY_TOTAL_POWERUPS, 0) + powerUps;
        
        PlayerPrefs.SetFloat(KEY_TOTAL_PLAY_TIME, totalPlayTime);
        PlayerPrefs.SetInt(KEY_TOTAL_KILLS, totalKills);
        PlayerPrefs.SetInt(KEY_TOTAL_POWERUPS, totalPowerUps);
        PlayerPrefs.Save();
    }
    
    // Obter estatísticas
    public static (float playTime, int kills, int powerUps) GetStatistics()
    {
        float playTime = PlayerPrefs.GetFloat(KEY_TOTAL_PLAY_TIME, 0f);
        int kills = PlayerPrefs.GetInt(KEY_TOTAL_KILLS, 0);
        int powerUps = PlayerPrefs.GetInt(KEY_TOTAL_POWERUPS, 0);
        
        return (playTime, kills, powerUps);
    }
    
    // Verificar se existe save
    public static bool HasSave()
    {
        return PlayerPrefs.HasKey(KEY_SCORE);
    }
    
    // Obter informações do save
    public static SaveInfo GetSaveInfo()
    {
        if (!HasSave())
            return null;
            
        try
        {
            string saveJson = PlayerPrefs.GetString(KEY_SCORE);
            SaveData saveData = JsonUtility.FromJson<SaveData>(saveJson);
            
            if (saveData != null)
            {
                return new SaveInfo
                {
                    level = saveData.level,
                    score = saveData.score,
                    lastSaveTime = saveData.lastSaveTime
                };
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao obter informações do save: {e.Message}");
        }
        
        return null;
    }
    
    // Informações do save para exibição
    public class SaveInfo
    {
        public int level;
        public int score;
        public string lastSaveTime;
    }
    
    // Deletar save
    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey(KEY_SCORE);
        PlayerPrefs.Save();
        Debug.Log("Save deletado!");
    }
    
    // Resetar todas as estatísticas
    public static void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Todos os dados foram resetados!");
    }
    
    // Auto-save (chamar periodicamente)
    public static void AutoSave(ProceduralMaze maze)
    {
        // Salvar apenas dados essenciais para auto-save
        PlayerPrefs.SetInt(KEY_SCORE, maze.score);
        PlayerPrefs.SetInt(KEY_LEVEL, maze.currentLevel);
        PlayerPrefs.SetInt(KEY_LIVES, maze.lives);
        PlayerPrefs.SetInt(KEY_AMMO, maze.ammo);
        PlayerPrefs.Save();
    }

    // Exportar configurações para JSON
    public static void ExportSettingsToJson(string path)
    {
        var settings = LoadSettings();
        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(path, json);
    }
    // Importar configurações de JSON
    public static void ImportSettingsFromJson(string path)
    {
        if (!File.Exists(path)) return;
        string json = File.ReadAllText(path);
        var settings = JsonUtility.FromJson<GameSettings>(json);
        if (settings != null)
            SaveSettings(settings);
    }

    // Adicionar score ao ranking local
    public static void AddScoreToRanking(int score)
    {
        List<int> ranking = GetRanking();
        ranking.Add(score);
        ranking.Sort((a, b) => b.CompareTo(a)); // decrescente
        if (ranking.Count > RANKING_SIZE)
            ranking.RemoveAt(ranking.Count - 1);
        string rankingStr = string.Join(",", ranking);
        PlayerPrefs.SetString(KEY_RANKING, rankingStr);
        PlayerPrefs.Save();
    }

    // Obter ranking local
    public static List<int> GetRanking()
    {
        string rankingStr = PlayerPrefs.GetString(KEY_RANKING, "");
        List<int> ranking = new List<int>();
        if (!string.IsNullOrEmpty(rankingStr))
        {
            string[] parts = rankingStr.Split(',');
            foreach (var p in parts)
            {
                if (int.TryParse(p, out int val))
                    ranking.Add(val);
            }
        }
        return ranking;
    }

    // Limpar ranking
    public static void ClearRanking()
    {
        PlayerPrefs.DeleteKey(KEY_RANKING);
        PlayerPrefs.Save();
    }
} 