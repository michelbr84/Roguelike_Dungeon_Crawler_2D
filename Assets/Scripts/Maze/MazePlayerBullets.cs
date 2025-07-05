using UnityEngine;

public static class MazePlayerBullets
{
    public static void MoveBullets(ProceduralMaze mazeObj)
    {
        if (ProceduralMaze.gameState != GameState.Playing)
            return;

        float cellSize = Mathf.Min(Screen.width / (float)mazeObj.width, Screen.height / (float)mazeObj.height);

        for (int i = mazeObj.bullets.Count - 1; i >= 0; i--)
        {
            var b = mazeObj.bullets[i];
            Vector2Int next = b.pos + b.dir;
            if (!MazePlayerUtils.IsValid(mazeObj, next) || mazeObj.maze[next.x, next.y] == 1)
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
            // NOVO: Verificar colisão com o jogador (projéteis de inimigos)
            if (next == mazeObj.playerPos)
            {
                float cellSizeHit = Mathf.Min(Screen.width / (float)mazeObj.width, Screen.height / (float)mazeObj.height);
                if (MazePlayerProtection.IsSpawnProtected())
                {
                    // Bloqueia durante proteção
                    mazeObj.bullets.RemoveAt(i);
                    continue;
                }
                if (mazeObj.shieldActive)
                {
                    MazeVisualEffects.CreateShieldBlockEffect(mazeObj.playerPos, cellSizeHit);
                    if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.powerUpSound);
                    mazeObj.bullets.RemoveAt(i);
                    continue;
                }
                
                // Verificar se o projétil veio do sniper
                bool isSniperBullet = IsSniperBullet(b, mazeObj);
                float damage = isSniperBullet ? mazeObj.sniperDamage : 1f; // Sniper causa meio dano, outros causam dano completo
                
                // Dano ao jogador
                MazeVisualEffects.CreatePlayerHitEffect(mazeObj.playerPos, cellSizeHit);
                MazeShaderEffects.ApplyScreenShake(10f, 0.3f);
                MazeStatistics.OnPlayerDeath();
                
                if (isSniperBullet)
                {
                    mazeObj.ApplyFractionalDamage(damage);
                }
                else
                {
                    mazeObj.LoseLifeAndRespawn();
                }
                
                if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.playerDeathSound);
                mazeObj.bullets.RemoveAt(i);
                continue;
            }
            b.pos = next;
        }
    }

    // Método para identificar se um projétil veio do sniper
    private static bool IsSniperBullet(ProceduralMaze.Bullet bullet, ProceduralMaze maze)
    {
        // Agora é simples: usar a propriedade isSniperBullet
        return bullet.isSniperBullet;
    }
} 