// Assets/Scripts/Maze/MazePowerUpUtils.cs
using System.Collections.Generic;
using UnityEngine;

public static class MazePowerUpUtils
{
    /// <summary>
    /// Spawna power-ups colecionáveis no mapa (inclui Shield).
    /// </summary>
    public static void SpawnPowerUps(ProceduralMaze mazeObj)
    {
        if (mazeObj.powerUps == null)
            mazeObj.powerUps = new List<PowerUp>();
        mazeObj.powerUps.Clear();
        var rng = new System.Random();
        int baseSpawn = 1 + mazeObj.currentLevel / 3;
        int toSpawn = Mathf.RoundToInt(baseSpawn * MazeDifficultySystem.GetPowerUpSpawnRateMultiplier());
        int spawned = 0, tries = 0;
        while (spawned < toSpawn && tries < 1000)
        {
            int x = rng.Next(0, mazeObj.width);
            int y = rng.Next(0, mazeObj.height);
            Vector2Int pos = new Vector2Int(x, y);

            bool spotOK = mazeObj.maze[x, y] == 0 &&
                pos != mazeObj.playerPos &&
                pos != mazeObj.exitPos &&
                !mazeObj.enemies.Contains(pos) &&
                mazeObj.powerUps.Find(pu => pu.position == pos) == null;

            if (spotOK)
            {
                // Sorteio balanceado:
                // 40% Ammo, 20% Life, 10% Shield, 10% DoubleShot, 7% SpeedBoost, 5% Invisibility, 4% Teleport, 4% ScoreBooster
                double roll = rng.NextDouble();
                PowerUpType type;
                if (roll < 0.40)
                    type = PowerUpType.Ammo;
                else if (roll < 0.60)
                    type = PowerUpType.Life;
                else if (roll < 0.70)
                    type = PowerUpType.Shield;
                else if (roll < 0.80)
                    type = PowerUpType.DoubleShot;
                else if (roll < 0.87)
                    type = PowerUpType.SpeedBoost;
                else if (roll < 0.92)
                    type = PowerUpType.Invisibility;
                else if (roll < 0.96)
                    type = PowerUpType.Teleport;
                else
                    type = PowerUpType.ScoreBooster;

                mazeObj.powerUps.Add(new PowerUp(pos, type));
                spawned++;
            }
            tries++;
        }
    }

    /// <summary>
    /// Coleta power-ups quando o player pisa no tile (agora inclui escudo/invencibilidade).
    /// </summary>
    public static void HandlePowerUpCollisions(ProceduralMaze mazeObj)
    {
        for (int i = mazeObj.powerUps.Count - 1; i >= 0; i--)
        {
            var powerUp = mazeObj.powerUps[i];
            if (mazeObj.playerPos == powerUp.position)
            {
                PowerUpType type = powerUp.type;
                mazeObj.CollectPowerUp(type); // Inclui lógica do shield
                mazeObj.powerUps.RemoveAt(i);

                // Criar efeitos visuais
                float cellSize = Mathf.Min(Screen.width / (float)mazeObj.width, Screen.height / (float)mazeObj.height);
                MazeVisualEffects.CreatePowerUpCollectEffect(powerUp.position, cellSize);
                MazeShaderEffects.CreatePowerUpGlow(powerUp.position);
                
                // Criar trilha de partícula
                Vector2 particlePos = new Vector2(powerUp.position.x * cellSize + cellSize/2, powerUp.position.y * cellSize + cellSize/2);
                Vector2 particleVel = new Vector2(Random.Range(-50f, 50f), Random.Range(-50f, 50f));
                MazeShaderEffects.CreateParticleTrail(particlePos, particleVel, Color.yellow, MazeShaderEffects.TrailType.Player);

                // Registrar missão
                MazeMissions.OnPowerUpCollected();

                // Troque o som para power-up se desejar, padrão usa som do "exit"
                if (AudioManager.Instance)
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.exitReachedSound);
            }
        }
    }
}
