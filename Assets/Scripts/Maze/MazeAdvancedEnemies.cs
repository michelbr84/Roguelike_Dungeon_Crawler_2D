using UnityEngine;
using System.Collections.Generic;

public static class MazeAdvancedEnemies
{
    // Tipos de inimigos avançados
    public enum AdvancedEnemyType
    {
        Normal,
        Boss,
        Sniper,
        Kamikaze,
        Spawner
    }
    
    // Dados dos inimigos avançados
    public class AdvancedEnemyData
    {
        public Vector2Int position;
        public AdvancedEnemyType type;
        public float health;
        public float maxHealth;
        public float specialTimer;
        public int spawnCount; // Para spawners
        public Vector2Int lastPlayerPos; // Para sniper
        
        public AdvancedEnemyData(Vector2Int pos, AdvancedEnemyType t)
        {
            position = pos;
            type = t;
            maxHealth = GetMaxHealth(t);
            health = maxHealth;
            specialTimer = 0f;
            spawnCount = 0;
            lastPlayerPos = Vector2Int.zero;
        }
        
        private float GetMaxHealth(AdvancedEnemyType type)
        {
            switch (type)
            {
                case AdvancedEnemyType.Boss: return 5f;
                case AdvancedEnemyType.Sniper: return 2f;
                case AdvancedEnemyType.Kamikaze: return 1f;
                case AdvancedEnemyType.Spawner: return 3f;
                default: return 1f;
            }
        }
    }
    
    // Lista de inimigos avançados
    private static List<AdvancedEnemyData> advancedEnemies = new List<AdvancedEnemyData>();
    
    // Configurações
    private static float bossMoveInterval = 1.2f;
    private static float sniperShootInterval = 2f;
    private static float kamikazeSpeed = 1.5f;
    private static float spawnerSpawnInterval = 4f;
    
    // Inicializar inimigos avançados
    public static void InitializeAdvancedEnemies()
    {
        advancedEnemies.Clear();
    }
    
    // Spawn de inimigos avançados baseado no nível
    public static void SpawnAdvancedEnemies(ProceduralMaze maze, int level)
    {
        // Verificar se inimigos avançados estão habilitados
        if (!MazeDifficultySystem.AreAdvancedEnemiesEnabled())
            return;
            
        var rng = new System.Random();
        
        // Boss a cada 5 níveis (se habilitado)
        if (level % 5 == 0 && MazeDifficultySystem.IsBossEnabled())
        {
            Vector2Int bossPos = FindSafePosition(maze, rng);
            if (bossPos != Vector2Int.zero)
            {
                advancedEnemies.Add(new AdvancedEnemyData(bossPos, AdvancedEnemyType.Boss));
                MazeShaderEffects.CreateBossGlow(bossPos);
            }
        }
        
        // Sniper a cada 3 níveis
        if (level % 3 == 0)
        {
            Vector2Int sniperPos = FindSafePosition(maze, rng);
            if (sniperPos != Vector2Int.zero)
            {
                advancedEnemies.Add(new AdvancedEnemyData(sniperPos, AdvancedEnemyType.Sniper));
            }
        }
        
        // Kamikaze a cada 2 níveis
        if (level % 2 == 0)
        {
            Vector2Int kamikazePos = FindSafePosition(maze, rng);
            if (kamikazePos != Vector2Int.zero)
            {
                advancedEnemies.Add(new AdvancedEnemyData(kamikazePos, AdvancedEnemyType.Kamikaze));
            }
        }
        
        // Spawner a cada 4 níveis
        if (level % 4 == 0)
        {
            Vector2Int spawnerPos = FindSafePosition(maze, rng);
            if (spawnerPos != Vector2Int.zero)
            {
                advancedEnemies.Add(new AdvancedEnemyData(spawnerPos, AdvancedEnemyType.Spawner));
            }
        }
    }
    
    // Encontrar posição segura para spawn
    private static Vector2Int FindSafePosition(ProceduralMaze maze, System.Random rng)
    {
        for (int tries = 0; tries < 50; tries++)
        {
            int x = rng.Next(0, maze.width);
            int y = rng.Next(0, maze.height);
            Vector2Int pos = new Vector2Int(x, y);
            
            if (maze.maze[x, y] == 0 && pos != maze.playerPos && pos != maze.exitPos && 
                !maze.enemies.Contains(pos) && !IsAdvancedEnemyAt(pos))
            {
                return pos;
            }
        }
        return Vector2Int.zero;
    }
    
    // Verificar se há inimigo avançado na posição
    private static bool IsAdvancedEnemyAt(Vector2Int pos)
    {
        foreach (var enemy in advancedEnemies)
        {
            if (enemy.position == pos) return true;
        }
        return false;
    }
    
    // Atualizar inimigos avançados
    public static void UpdateAdvancedEnemies(ProceduralMaze maze)
    {
        if (ProceduralMaze.gameState != GameState.Playing)
            return;
            
        for (int i = advancedEnemies.Count - 1; i >= 0; i--)
        {
            var enemy = advancedEnemies[i];
            enemy.specialTimer += Time.deltaTime;
            
            switch (enemy.type)
            {
                case AdvancedEnemyType.Boss:
                    UpdateBoss(enemy, maze);
                    break;
                case AdvancedEnemyType.Sniper:
                    UpdateSniper(enemy, maze);
                    break;
                case AdvancedEnemyType.Kamikaze:
                    UpdateKamikaze(enemy, maze);
                    break;
                case AdvancedEnemyType.Spawner:
                    UpdateSpawner(enemy, maze);
                    break;
            }
        }
    }
    
    // Comportamento do Boss
    private static void UpdateBoss(AdvancedEnemyData boss, ProceduralMaze maze)
    {
        if (boss.specialTimer >= bossMoveInterval)
        {
            boss.specialTimer = 0f;
            
            // Boss persegue o jogador de forma inteligente
            Vector2Int direction = GetDirectionToPlayer(boss.position, maze.playerPos);
            Vector2Int newPos = boss.position + direction;
            
            if (IsValidMove(newPos, maze))
            {
                boss.position = newPos;
            }
            else
            {
                // Tenta direções alternativas
                Vector2Int[] alternatives = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
                foreach (var alt in alternatives)
                {
                    Vector2Int altPos = boss.position + alt;
                    if (IsValidMove(altPos, maze))
                    {
                        boss.position = altPos;
                        break;
                    }
                }
            }
        }
    }
    
    // Comportamento do Sniper
    private static void UpdateSniper(AdvancedEnemyData sniper, ProceduralMaze maze)
    {
        if (sniper.specialTimer >= sniperShootInterval)
        {
            sniper.specialTimer = 0f;
            
            // Sniper atira em linha reta se o jogador estiver alinhado
            if (IsPlayerAligned(sniper.position, maze.playerPos))
            {
                // Criar projétil do sniper
                Vector2Int bulletDir = GetDirectionToPlayer(sniper.position, maze.playerPos);
                maze.bullets.Add(new ProceduralMaze.Bullet(sniper.position, bulletDir));
                
                if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.shootSound);
            }
            
            sniper.lastPlayerPos = maze.playerPos;
        }
    }
    
    // Comportamento do Kamikaze
    private static void UpdateKamikaze(AdvancedEnemyData kamikaze, ProceduralMaze maze)
    {
        if (kamikaze.specialTimer >= kamikazeSpeed)
        {
            kamikaze.specialTimer = 0f;
            
            // Kamikaze corre direto para o jogador
            Vector2Int direction = GetDirectionToPlayer(kamikaze.position, maze.playerPos);
            Vector2Int newPos = kamikaze.position + direction;
            
            if (IsValidMove(newPos, maze))
            {
                kamikaze.position = newPos;
            }
        }
    }
    
    // Comportamento do Spawner
    private static void UpdateSpawner(AdvancedEnemyData spawner, ProceduralMaze maze)
    {
        if (spawner.specialTimer >= spawnerSpawnInterval && spawner.spawnCount < 3)
        {
            spawner.specialTimer = 0f;
            
            // Spawner cria inimigos normais
            Vector2Int spawnPos = FindSpawnPosition(spawner.position, maze);
            if (spawnPos != Vector2Int.zero)
            {
                maze.enemies.Add(spawnPos);
                spawner.spawnCount++;
                
                // Criar efeito visual de spawn
                float cellSize = Mathf.Min(Screen.width / (float)maze.width, Screen.height / (float)maze.height);
                MazeVisualEffects.CreateEnemyDeathEffect(spawnPos, cellSize);
            }
        }
    }
    
    // Verificar se jogador está alinhado com sniper
    private static bool IsPlayerAligned(Vector2Int sniperPos, Vector2Int playerPos)
    {
        return sniperPos.x == playerPos.x || sniperPos.y == playerPos.y;
    }
    
    // Obter direção para o jogador
    private static Vector2Int GetDirectionToPlayer(Vector2Int from, Vector2Int to)
    {
        Vector2Int diff = to - from;
        
        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            return new Vector2Int(diff.x > 0 ? 1 : -1, 0);
        }
        else
        {
            return new Vector2Int(0, diff.y > 0 ? 1 : -1);
        }
    }
    
    // Verificar se movimento é válido
    private static bool IsValidMove(Vector2Int pos, ProceduralMaze maze)
    {
        if (pos.x < 0 || pos.x >= maze.width || pos.y < 0 || pos.y >= maze.height)
            return false;
            
        if (maze.maze[pos.x, pos.y] == 1)
            return false;
            
        if (pos == maze.playerPos || pos == maze.exitPos)
            return false;
            
        if (maze.enemies.Contains(pos) || IsAdvancedEnemyAt(pos))
            return false;
            
        return true;
    }
    
    // Encontrar posição para spawn de inimigo
    private static Vector2Int FindSpawnPosition(Vector2Int spawnerPos, ProceduralMaze maze)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        
        foreach (var dir in directions)
        {
            Vector2Int spawnPos = spawnerPos + dir;
            if (IsValidMove(spawnPos, maze))
            {
                return spawnPos;
            }
        }
        
        return Vector2Int.zero;
    }
    
    // Verificar colisão com inimigos avançados
    public static bool CheckAdvancedEnemyCollision(Vector2Int playerPos)
    {
        foreach (var enemy in advancedEnemies)
        {
            if (enemy.position == playerPos)
            {
                return true;
            }
        }
        return false;
    }
    
    // Processar dano em inimigo avançado
    public static bool DamageAdvancedEnemy(Vector2Int pos, ProceduralMaze maze)
    {
        for (int i = 0; i < advancedEnemies.Count; i++)
        {
            if (advancedEnemies[i].position == pos)
            {
                advancedEnemies[i].health--;
                
                if (advancedEnemies[i].health <= 0)
                {
                    // Inimigo morreu
                    float cellSize = Mathf.Min(Screen.width / (float)maze.width, Screen.height / (float)maze.height);
                    MazeVisualEffects.CreateEnemyDeathEffect(pos, cellSize);
                    
                    // Dar score baseado no tipo
                    int scoreToAdd = GetEnemyScore(advancedEnemies[i].type);
                    MazeHUD.AddScore(scoreToAdd);
                    
                    // Registrar achievement e missão
                    MazeAchievements.OnEnemyKilled();
                    MazeStatistics.OnEnemyKilled();
                    MazeStatistics.OnShotHit();
                    
                    // Verificar se era um boss
                    if (advancedEnemies[i].type == AdvancedEnemyType.Boss)
                    {
                        MazeStatistics.OnBossKilled();
                        MazeMissions.OnBossKilled();
                    }
                    
                    advancedEnemies.RemoveAt(i);
                    return true;
                }
                return true;
            }
        }
        return false;
    }
    
    // Obter score baseado no tipo de inimigo
    private static int GetEnemyScore(AdvancedEnemyType type)
    {
        switch (type)
        {
            case AdvancedEnemyType.Boss: return 200;
            case AdvancedEnemyType.Sniper: return 100;
            case AdvancedEnemyType.Kamikaze: return 75;
            case AdvancedEnemyType.Spawner: return 150;
            default: return 50;
        }
    }
    
    // Obter lista de inimigos avançados para renderização
    public static List<AdvancedEnemyData> GetAdvancedEnemies()
    {
        return advancedEnemies;
    }
    
    // Limpar todos os inimigos avançados
    public static void ClearAdvancedEnemies()
    {
        advancedEnemies.Clear();
    }
} 