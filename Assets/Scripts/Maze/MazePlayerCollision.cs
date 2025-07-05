using UnityEngine;

public static class MazePlayerCollision
{
    public static void HandleCollisions(ProceduralMaze mazeObj)
    {
        if (ProceduralMaze.gameState != GameState.Playing)
            return;

        // Atualizar timer de proteção ao spawnar
        MazePlayerProtection.UpdateProtectionTimer();

        // Verificar colisão com inimigos normais
        for (int i = mazeObj.enemies.Count - 1; i >= 0; i--)
        {
            if (mazeObj.playerPos == mazeObj.enemies[i])
            {
                float cellSize = Mathf.Min(Screen.width / (float)mazeObj.width, Screen.height / (float)mazeObj.height);
                
                // Só perde vida se não estiver com proteção ao spawnar
                if (!MazePlayerProtection.IsSpawnProtected())
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
            if (!MazePlayerProtection.IsSpawnProtected())
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
} 