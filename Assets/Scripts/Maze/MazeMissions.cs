using UnityEngine;
using System.Collections.Generic;

public static class MazeMissions
{
    // Tipos de missões
    public enum MissionType
    {
        KillEnemies,
        CollectPowerUps,
        SurviveTime,
        ReachLevel,
        KillBoss,
        NoDamage,
        SpeedRun,
        PerfectLevel,
        KillStreak,
        UseAllPowerUps,
        ReachHighScore,
        CompleteWithoutShooting
    }
    
    // Estrutura de missão
    public class Mission
    {
        public MissionType type;
        public string description;
        public int target;
        public int current;
        public bool completed;
        public int reward;
        public bool isActive;
        
        public Mission(MissionType t, string desc, int targ, int rew)
        {
            type = t;
            description = desc;
            target = targ;
            current = 0;
            completed = false;
            reward = rew;
            isActive = true;
        }
        
        public float GetProgress()
        {
            return Mathf.Clamp01((float)current / target);
        }
    }
    
    // Lista de missões ativas
    private static List<Mission> activeMissions = new List<Mission>();
    
    // Configurações
    private static float missionCheckTimer = 0f;
    private static float missionCheckInterval = 1f;
    private static float survivalTimer = 0f;
    private static bool noDamageActive = false;
    private static float speedRunStartTime = 0f;
    
    // Inicializar sistema de missões
    public static void InitializeMissions()
    {
        activeMissions.Clear();
        missionCheckTimer = 0f;
        survivalTimer = 0f;
        noDamageActive = false;
        speedRunStartTime = 0f;
    }
    
    // Gerar missões para o nível atual
    public static void GenerateMissions(int level)
    {
        activeMissions.Clear();
        var rng = new System.Random();
        
        // Sempre ter pelo menos 2 missões
        int missionCount = 2 + level / 5; // Mais missões em níveis altos
        
        for (int i = 0; i < missionCount; i++)
        {
            MissionType type = GetRandomMissionType(rng, level);
            Mission mission = CreateMission(type, level, rng);
            
            if (mission != null)
            {
                activeMissions.Add(mission);
            }
        }
    }
    
    // Obter tipo de missão aleatório
    private static MissionType GetRandomMissionType(System.Random rng, int level)
    {
        float roll = (float)rng.NextDouble();
        
        if (roll < 0.25f) return MissionType.KillEnemies;
        if (roll < 0.45f) return MissionType.CollectPowerUps;
        if (roll < 0.60f) return MissionType.SurviveTime;
        if (roll < 0.75f) return MissionType.ReachLevel;
        if (roll < 0.85f) return MissionType.KillBoss;
        if (roll < 0.90f) return MissionType.NoDamage;
        if (roll < 0.93f) return MissionType.SpeedRun;
        if (roll < 0.95f) return MissionType.PerfectLevel;
        if (roll < 0.97f) return MissionType.KillStreak;
        if (roll < 0.98f) return MissionType.UseAllPowerUps;
        if (roll < 0.99f) return MissionType.ReachHighScore;
        return MissionType.CompleteWithoutShooting;
    }
    
    // Criar missão específica
    private static Mission CreateMission(MissionType type, int level, System.Random rng)
    {
        switch (type)
        {
            case MissionType.KillEnemies:
                int enemyTarget = 3 + level * 2;
                return new Mission(type, $"Mate {enemyTarget} inimigos", enemyTarget, 100 + level * 20);
                
            case MissionType.CollectPowerUps:
                int powerUpTarget = 2 + level / 2;
                return new Mission(type, $"Colete {powerUpTarget} power-ups", powerUpTarget, 80 + level * 15);
                
            case MissionType.SurviveTime:
                int timeTarget = 30 + level * 10;
                return new Mission(type, $"Sobreviva {timeTarget} segundos", timeTarget, 120 + level * 25);
                
            case MissionType.ReachLevel:
                int levelTarget = level + 2;
                return new Mission(type, $"Chegue ao nível {levelTarget}", levelTarget, 150 + level * 30);
                
            case MissionType.KillBoss:
                return new Mission(type, "Mate um Boss", 1, 200 + level * 50);
                
            case MissionType.NoDamage:
                return new Mission(type, "Complete o nível sem tomar dano", 1, 300 + level * 100);
                
            case MissionType.SpeedRun:
                int speedTarget = 60 - level * 5; // Menos tempo em níveis altos
                speedTarget = Mathf.Max(speedTarget, 20);
                return new Mission(type, $"Complete em {speedTarget} segundos", speedTarget, 250 + level * 40);
                
            case MissionType.PerfectLevel:
                return new Mission(type, "Complete o nível sem perder vida", 1, 400 + level * 80);
                
            case MissionType.KillStreak:
                int streakTarget = 5 + level;
                return new Mission(type, $"Mate {streakTarget} inimigos seguidos", streakTarget, 300 + level * 50);
                
            case MissionType.UseAllPowerUps:
                return new Mission(type, "Use todos os tipos de power-ups", 6, 500 + level * 100);
                
            case MissionType.ReachHighScore:
                int scoreTarget = 1000 + level * 500;
                return new Mission(type, $"Alcance {scoreTarget} pontos", scoreTarget, 600 + level * 120);
                
            case MissionType.CompleteWithoutShooting:
                return new Mission(type, "Complete sem atirar", 1, 800 + level * 150);
                
            default:
                return null;
        }
    }
    
    // Atualizar missões
    public static void UpdateMissions(ProceduralMaze maze)
    {
        if (ProceduralMaze.gameState != GameState.Playing)
            return;
            
        missionCheckTimer += Time.deltaTime;
        
        if (missionCheckTimer >= missionCheckInterval)
        {
            missionCheckTimer = 0f;
            CheckMissionProgress(maze);
        }
        
        // Atualizar timers específicos
        UpdateMissionTimers(maze);
    }
    
    // Verificar progresso das missões
    private static void CheckMissionProgress(ProceduralMaze maze)
    {
        foreach (var mission in activeMissions)
        {
            if (mission.completed || !mission.isActive)
                continue;
                
            switch (mission.type)
            {
                case MissionType.KillEnemies:
                    // Atualizado via eventos
                    break;
                    
                case MissionType.CollectPowerUps:
                    // Atualizado via eventos
                    break;
                    
                case MissionType.SurviveTime:
                    mission.current = Mathf.RoundToInt(survivalTimer);
                    break;
                    
                case MissionType.ReachLevel:
                    mission.current = maze.currentLevel;
                    break;
                    
                case MissionType.KillBoss:
                    // Atualizado via eventos
                    break;
                    
                case MissionType.NoDamage:
                    if (maze.lives < maze.startingLives)
                    {
                        mission.isActive = false; // Falhou
                    }
                    break;
                    
                case MissionType.SpeedRun:
                    float elapsedTime = Time.time - speedRunStartTime;
                    mission.current = Mathf.RoundToInt(elapsedTime);
                    break;
            }
            
            // Verificar se completou
            if (mission.current >= mission.target && mission.isActive)
            {
                CompleteMission(mission);
            }
        }
    }
    
    // Atualizar timers específicos
    private static void UpdateMissionTimers(ProceduralMaze maze)
    {
        // Timer de sobrevivência
        if (HasActiveMission(MissionType.SurviveTime))
        {
            survivalTimer += Time.deltaTime;
        }
        
        // Timer de speed run
        if (HasActiveMission(MissionType.SpeedRun) && speedRunStartTime == 0f)
        {
            speedRunStartTime = Time.time;
        }
    }
    
    // Verificar se há missão ativa de um tipo
    private static bool HasActiveMission(MissionType type)
    {
        foreach (var mission in activeMissions)
        {
            if (mission.type == type && mission.isActive && !mission.completed)
                return true;
        }
        return false;
    }
    
    // Eventos para atualizar missões
    public static void OnEnemyKilled()
    {
        UpdateMissionProgress(MissionType.KillEnemies, 1);
    }
    
    public static void OnPowerUpCollected()
    {
        UpdateMissionProgress(MissionType.CollectPowerUps, 1);
    }
    
    public static void OnBossKilled()
    {
        UpdateMissionProgress(MissionType.KillBoss, 1);
    }
    
    public static void OnLevelCompleted()
    {
        UpdateMissionProgress(MissionType.ReachLevel, 1);
        UpdateMissionProgress(MissionType.SpeedRun, 0); // Verificar se completou no tempo
    }
    
    // Atualizar progresso de missão
    private static void UpdateMissionProgress(MissionType type, int amount)
    {
        foreach (var mission in activeMissions)
        {
            if (mission.type == type && mission.isActive && !mission.completed)
            {
                mission.current += amount;
                
                if (mission.current >= mission.target)
                {
                    CompleteMission(mission);
                }
            }
        }
    }
    
    // Completar missão
    private static void CompleteMission(Mission mission)
    {
        mission.completed = true;
        mission.isActive = false;
        
        // Dar recompensa
        MazeHUD.AddScore(mission.reward);
        
        // Mostrar notificação
        MazeHUD.ShowStatusMessage($"Missão Completa: {mission.description} (+{mission.reward} pontos)");
        
        // Criar efeito visual
        float cellSize = Mathf.Min(Screen.width / 20f, Screen.height / 15f);
        Vector2Int effectPos = new Vector2Int(10, 7);
        MazeShaderEffects.CreateGlowEffect(effectPos, Color.green, 1.5f, 2f, MazeShaderEffects.GlowType.PowerUp);
        
        // Registrar achievement
        MazeAchievements.OnMissionCompleted();
    }
    
    // Obter missões ativas
    public static List<Mission> GetActiveMissions()
    {
        return activeMissions;
    }
    
    // Obter missões completadas
    public static List<Mission> GetCompletedMissions()
    {
        List<Mission> completed = new List<Mission>();
        foreach (var mission in activeMissions)
        {
            if (mission.completed)
                completed.Add(mission);
        }
        return completed;
    }
    
    // Verificar se todas as missões foram completadas
    public static bool AllMissionsCompleted()
    {
        foreach (var mission in activeMissions)
        {
            if (!mission.completed)
                return false;
        }
        return activeMissions.Count > 0;
    }
    
    // Resetar missões para novo nível
    public static void ResetMissionsForNewLevel()
    {
        // Manter apenas missões que continuam entre níveis
        List<Mission> persistentMissions = new List<Mission>();
        
        foreach (var mission in activeMissions)
        {
            if (mission.type == MissionType.ReachLevel || mission.type == MissionType.SurviveTime)
            {
                persistentMissions.Add(mission);
            }
        }
        
        activeMissions = persistentMissions;
    }
} 