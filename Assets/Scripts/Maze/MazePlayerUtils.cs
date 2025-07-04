// Assets/Scripts/Maze/MazePlayerUtils.cs
using System.Collections.Generic;
using UnityEngine;

public static class MazePlayerUtils
{
    // Proteção ao spawnar
    private static float spawnProtectionTimer = 0f;
    private const float SPAWN_PROTECTION_DURATION = 3f;

    /// <summary>
    /// Checa se a posição está dentro dos limites do maze.
    /// </summary>
    public static bool IsValid(ProceduralMaze mazeObj, Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < mazeObj.width && pos.y >= 0 && pos.y < mazeObj.height;
    }

    /// <summary>
    /// Entrada do jogador (movimento e disparo). Chamar no Update principal.
    /// </summary>
    public static void HandlePlayerInput(ProceduralMaze mazeObj)
    {
        if (ProceduralMaze.gameState != GameState.Playing)
            return;

        Vector2Int inputDir = Vector2Int.zero;
        bool triedMove = false;
        bool moved = false;

        // Input de teclado
        if (Input.GetKeyDown(mazeObj.upKey))    { inputDir = Vector2Int.up; triedMove = true; }
        if (Input.GetKeyDown(mazeObj.downKey))  { inputDir = Vector2Int.down; triedMove = true; }
        if (Input.GetKeyDown(mazeObj.leftKey))  { inputDir = Vector2Int.left; triedMove = true; }
        if (Input.GetKeyDown(mazeObj.rightKey)) { inputDir = Vector2Int.right; triedMove = true; }
        
        // Input touch (sobrescreve teclado se disponível)
        if (MazeTouchControls.IsTouchEnabled())
        {
            Vector2Int touchInput = MazeTouchControls.ProcessTouchInput();
            if (touchInput != Vector2Int.zero)
            {
                inputDir = touchInput;
                triedMove = true;
            }
        }

        Vector2Int moveDir = new Vector2Int(
            inputDir.x * mazeObj.horizontalMultiplier,
            inputDir.y * mazeObj.verticalMultiplier
        );

        // Aplicar modificadores de clima e eventos
        float movementModifier = MazeWeatherSystem.GetMovementModifier() * MazeEventSystem.GetPlayerSpeedModifier();
        float speedBoostMultiplier = mazeObj.speedBoostMultiplier * movementModifier;
        
        // Speed boost: move duas vezes se ativo
        if (mazeObj.speedBoostActive && inputDir != Vector2Int.zero)
        {
            for (int i = 0; i < Mathf.RoundToInt(speedBoostMultiplier); i++)
            {
                Vector2Int newPosBoost = mazeObj.playerPos + moveDir;
                if (IsValid(mazeObj, newPosBoost) && mazeObj.maze[newPosBoost.x, newPosBoost.y] == 0)
                {
                    mazeObj.playerPos = newPosBoost;
                    mazeObj.playerDir = inputDir;
                    moved = true;
                    
                    // Criar efeito de glow do jogador (mais intenso para speed boost)
                    MazeShaderEffects.CreatePlayerGlow(newPosBoost);
                    
                    // Criar trilha de partícula
                    float cellSize = Mathf.Min(Screen.width / (float)mazeObj.width, Screen.height / (float)mazeObj.height);
                    Vector2 particlePos = new Vector2(newPosBoost.x * cellSize + cellSize/2, newPosBoost.y * cellSize + cellSize/2);
                    Vector2 particleVel = new Vector2(Random.Range(-30f, 30f), Random.Range(-30f, 30f));
                    MazeShaderEffects.CreateParticleTrail(particlePos, particleVel, Color.cyan, MazeShaderEffects.TrailType.Player);
                    
                    if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.moveSound);
                    MazePowerUpUtils.HandlePowerUpCollisions(mazeObj);
                    if (mazeObj.playerPos == mazeObj.exitPos)
                    {
                        int scoreToAdd = 250 + mazeObj.currentLevel * 10;
                        if (mazeObj.scoreBoosterActive) scoreToAdd = Mathf.RoundToInt(scoreToAdd * mazeObj.scoreBoosterMultiplier);
                        MazeHUD.AddScore(scoreToAdd);
                        mazeObj.score = MazeHUD.score;
                        mazeObj.NextLevel();
                        return;
                    }
                }
            }
        }
        else
        {
            Vector2Int newPos = mazeObj.playerPos + moveDir;
            if (inputDir != Vector2Int.zero && IsValid(mazeObj, newPos) && mazeObj.maze[newPos.x, newPos.y] == 0)
            {
                mazeObj.playerPos = newPos;
                mazeObj.playerDir = inputDir;
                moved = true;

                // Criar efeito de glow do jogador
                MazeShaderEffects.CreatePlayerGlow(newPos);
                
                // Criar trilha de partícula
                float cellSize = Mathf.Min(Screen.width / (float)mazeObj.width, Screen.height / (float)mazeObj.height);
                Vector2 particlePos = new Vector2(newPos.x * cellSize + cellSize/2, newPos.y * cellSize + cellSize/2);
                Vector2 particleVel = new Vector2(Random.Range(-20f, 20f), Random.Range(-20f, 20f));
                MazeShaderEffects.CreateParticleTrail(particlePos, particleVel, Color.cyan, MazeShaderEffects.TrailType.Player);

                if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.moveSound);

                // Checa power-ups ao mover!
                MazePowerUpUtils.HandlePowerUpCollisions(mazeObj);

                if (mazeObj.playerPos == mazeObj.exitPos)
                {
                    int scoreToAdd = 250 + mazeObj.currentLevel * 10;
                    if (mazeObj.scoreBoosterActive) scoreToAdd = Mathf.RoundToInt(scoreToAdd * mazeObj.scoreBoosterMultiplier);
                    MazeHUD.AddScore(scoreToAdd);
                    mazeObj.score = MazeHUD.score;
                    mazeObj.NextLevel();
                    return;
                }
            }
        }
        // Corrigido: este bloco deve estar fora do else acima
        if (triedMove && !moved)
        {
            if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.hitWallSound);
        }

        // Disparo: só se tiver munição! (teclado + touch)
        bool shootPressed = Input.GetKeyDown(mazeObj.shootKey) || MazeTouchControls.IsShootButtonPressed();
        if (shootPressed && mazeObj.ammo > 0)
        {
            Vector2Int bulletDir = new Vector2Int(
                mazeObj.playerDir.x * mazeObj.horizontalMultiplier,
                mazeObj.playerDir.y * mazeObj.verticalMultiplier
            );
            if (bulletDir == Vector2Int.zero)
                bulletDir = new Vector2Int(0, 1 * mazeObj.verticalMultiplier);

            // Lógica de power-up de tiro
            if (mazeObj.tripleShotActive)
            {
                // Dispara três projéteis: frente, esquerda, direita
                mazeObj.bullets.Add(new ProceduralMaze.Bullet(mazeObj.playerPos, bulletDir));
                mazeObj.bullets.Add(new ProceduralMaze.Bullet(mazeObj.playerPos, new Vector2Int(-bulletDir.y, bulletDir.x)));
                mazeObj.bullets.Add(new ProceduralMaze.Bullet(mazeObj.playerPos, new Vector2Int(bulletDir.y, -bulletDir.x)));
                mazeObj.ammo = Mathf.Max(mazeObj.ammo - 3, 0);
            }
            else if (mazeObj.doubleShotActive)
            {
                // Dispara dois projéteis: frente e lateral
                mazeObj.bullets.Add(new ProceduralMaze.Bullet(mazeObj.playerPos, bulletDir));
                mazeObj.bullets.Add(new ProceduralMaze.Bullet(mazeObj.playerPos, new Vector2Int(-bulletDir.y, bulletDir.x)));
                mazeObj.ammo = Mathf.Max(mazeObj.ammo - 2, 0);
            }
            else
            {
                mazeObj.bullets.Add(new ProceduralMaze.Bullet(mazeObj.playerPos, bulletDir));
                mazeObj.ammo--; // Gasta munição
            }

            MazeStatistics.OnShotFired();
            if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.shootSound);
        }
        else if (shootPressed && mazeObj.ammo <= 0)
        {
            if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.hitWallSound);
            MazeHUD.ShowStatusMessage("Sem munição!");
        }
    }

    /// <summary>
    /// Move os projéteis (balas) disparados pelo player. Chamar em Update.
    /// </summary>
    public static void MoveBullets(ProceduralMaze mazeObj)
    {
        if (ProceduralMaze.gameState != GameState.Playing)
            return;

        float cellSize = Mathf.Min(Screen.width / (float)mazeObj.width, Screen.height / (float)mazeObj.height);

        for (int i = mazeObj.bullets.Count - 1; i >= 0; i--)
        {
            var b = mazeObj.bullets[i];
            Vector2Int next = b.pos + b.dir;
            if (!IsValid(mazeObj, next) || mazeObj.maze[next.x, next.y] == 1)
            {
                mazeObj.bullets.RemoveAt(i);
                continue;
            }
            // Verificar colisão com inimigos normais
            int enemyIdx = mazeObj.enemies.FindIndex(e => e == next);
            if (enemyIdx != -1)
            {
                // Criar efeito de morte do inimigo
                MazeVisualEffects.CreateEnemyDeathEffect(next, cellSize);
                
                mazeObj.enemies.RemoveAt(enemyIdx);
                mazeObj.bullets.RemoveAt(i);
                int scoreToAdd = 50 + mazeObj.currentLevel * 2;
                if (mazeObj.scoreBoosterActive) scoreToAdd = Mathf.RoundToInt(scoreToAdd * mazeObj.scoreBoosterMultiplier);
                
                // Aplicar modificadores de dano de eventos
                scoreToAdd = Mathf.RoundToInt(scoreToAdd * MazeEventSystem.GetPlayerDamageModifier());
                MazeHUD.AddScore(scoreToAdd);
                mazeObj.score = MazeHUD.score;
                
                // Criar efeito de popup de score
                MazeVisualEffects.CreateScorePopupEffect(next, cellSize, scoreToAdd);
                
                // Registrar achievement, estatísticas e missão
                MazeAchievements.OnEnemyKilled();
                MazeStatistics.OnEnemyKilled();
                MazeStatistics.OnShotHit();
                MazeAchievements.OnScoreUpdated(mazeObj.score);
                MazeMissions.OnEnemyKilled();
                
                if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyDeathSound);
                continue;
            }
            
            // Verificar colisão com inimigos avançados
            if (MazeAdvancedEnemies.DamageAdvancedEnemy(next, mazeObj))
            {
                mazeObj.bullets.RemoveAt(i);
                continue;
            }
            b.pos = next;
        }
    }

    /// <summary>
    /// Colisões principais: se player colidiu com inimigo (considera shield).
    /// </summary>
    public static void HandleCollisions(ProceduralMaze mazeObj)
    {
        if (ProceduralMaze.gameState != GameState.Playing)
            return;

        // Atualizar timer de proteção ao spawnar
        if (spawnProtectionTimer > 0f)
        {
            spawnProtectionTimer -= Time.deltaTime;
        }

        // Verificar colisão com inimigos normais
        for (int i = mazeObj.enemies.Count - 1; i >= 0; i--)
        {
            if (mazeObj.playerPos == mazeObj.enemies[i])
            {
                float cellSize = Mathf.Min(Screen.width / (float)mazeObj.width, Screen.height / (float)mazeObj.height);
                
                // Só perde vida se não estiver com proteção ao spawnar
                if (spawnProtectionTimer <= 0f)
                {
                    // Verificar se ataques de inimigos estão desabilitados por evento
                    if (MazeEventSystem.AreEnemyAttacksDisabled())
                    {
                        // Inimigo é destruído sem causar dano
                        MazeVisualEffects.CreateEnemyDeathEffect(mazeObj.enemies[i], cellSize);
                        mazeObj.enemies.RemoveAt(i);
                        MazeHUD.AddScore(10);
                        if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyDeathSound);
                        return;
                    }
                    
                    if (mazeObj.shieldActive)
                    {
                        // Criar efeito de bloqueio de escudo
                        MazeVisualEffects.CreateShieldBlockEffect(mazeObj.playerPos, cellSize);
                        if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.powerUpSound);
                        return;
                    }
                    
                    // Criar efeito de hit do jogador
                    MazeVisualEffects.CreatePlayerHitEffect(mazeObj.playerPos, cellSize);
                    MazeShaderEffects.ApplyScreenShake(10f, 0.3f);
                    MazeStatistics.OnPlayerDeath();
                    mazeObj.LoseLifeAndRespawn();
                    if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.playerDeathSound);
                }
                else
                {
                    // Inimigo é destruído durante proteção
                    MazeVisualEffects.CreateEnemyDeathEffect(mazeObj.enemies[i], cellSize);
                    mazeObj.enemies.RemoveAt(i);
                    MazeHUD.AddScore(10);
                    if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyDeathSound);
                }
                return;
            }
        }
        
        // Verificar colisão com inimigos avançados
        if (MazeAdvancedEnemies.CheckAdvancedEnemyCollision(mazeObj.playerPos))
        {
            float cellSize = Mathf.Min(Screen.width / (float)mazeObj.width, Screen.height / (float)mazeObj.height);
            
            // Só perde vida se não estiver com proteção ao spawnar
            if (spawnProtectionTimer <= 0f)
            {
                if (mazeObj.shieldActive)
                {
                    // Criar efeito de bloqueio de escudo
                    MazeVisualEffects.CreateShieldBlockEffect(mazeObj.playerPos, cellSize);
                    if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.powerUpSound);
                    return;
                }
                
                // Criar efeito de hit do jogador
                MazeVisualEffects.CreatePlayerHitEffect(mazeObj.playerPos, cellSize);
                MazeShaderEffects.ApplyScreenShake(10f, 0.3f);
                MazeStatistics.OnPlayerDeath();
                mazeObj.LoseLifeAndRespawn();
                if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.playerDeathSound);
            }
            else
            {
                // Inimigo avançado é danificado durante proteção
                MazeAdvancedEnemies.DamageAdvancedEnemy(mazeObj.playerPos, mazeObj);
            }
            return;
        }
    }

    // Método para ativar proteção ao spawnar
    public static void ActivateSpawnProtection()
    {
        spawnProtectionTimer = SPAWN_PROTECTION_DURATION;
    }

    // Método para verificar se está protegido
    public static bool IsSpawnProtected()
    {
        return spawnProtectionTimer > 0f;
    }
}
