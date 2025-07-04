// Assets/Scripts/Maze/MazeEnemyUtils.cs
using System.Collections.Generic;
using UnityEngine;

public static class MazeEnemyUtils
{
    // Estrutura para armazenar dados do inimigo
    public class EnemyData
    {
        public Vector2Int position;
        public Vector2Int direction;
        
        public EnemyData(Vector2Int pos, Vector2Int dir)
        {
            position = pos;
            direction = dir;
        }
    }
    
    // Lista de dados dos inimigos (posição + direção)
    private static List<EnemyData> enemyDataList = new List<EnemyData>();

    /// <summary>
    /// Gera e posiciona os inimigos no labirinto, evitando posições inválidas.
    /// </summary>
    public static void SpawnEnemies(ProceduralMaze mazeObj)
    {
        mazeObj.enemies.Clear();
        enemyDataList.Clear();
        var rng = new System.Random();
        int tries = 0;
        while (mazeObj.enemies.Count < mazeObj.enemyCount && tries < 500)
        {
            int x = rng.Next(0, mazeObj.width);
            int y = rng.Next(0, mazeObj.height);
            Vector2Int pos = new Vector2Int(x, y);
            if (mazeObj.maze[x, y] == 0 && pos != mazeObj.playerPos && pos != mazeObj.exitPos && !mazeObj.enemies.Contains(pos))
            {
                mazeObj.enemies.Add(pos);
                // Inicializar direção aleatória
                Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
                Vector2Int randomDir = directions[rng.Next(directions.Length)];
                enemyDataList.Add(new EnemyData(pos, randomDir));
            }
            tries++;
        }
    }

    /// <summary>
    /// Move cada inimigo conforme a IA: persegue o player se perto, senão anda aleatório.
    /// </summary>
    public static void MoveEnemies(ProceduralMaze mazeObj)
    {
        if (ProceduralMaze.gameState != GameState.Playing)
            return;

        mazeObj.enemyMoveTimer += Time.deltaTime;
        if (mazeObj.enemyMoveTimer >= mazeObj.enemyMoveInterval)
        {
            mazeObj.enemyMoveTimer = 0f;
            var rng = new System.Random();
            for (int i = 0; i < mazeObj.enemies.Count; i++)
            {
                Vector2Int oldPos = mazeObj.enemies[i];
                Vector2Int newPos = oldPos;
                Vector2Int moveDirection = Vector2Int.zero;

                // --- 1. Inimigo 0: perseguição preditiva ---
                if (i == 0)
                {
                    // Tenta prever o próximo passo do jogador
                    Vector2Int playerNext = mazeObj.playerPos + mazeObj.playerDir;
                    Vector2Int best = oldPos;
                    float bestDist = Vector2Int.Distance(oldPos, playerNext);
                    foreach (var d in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
                    {
                        Vector2Int n = oldPos + d;
                        if (MazePlayerUtils.IsValid(mazeObj, n) && mazeObj.maze[n.x, n.y] == 0 &&
                            n != mazeObj.playerPos && !mazeObj.enemies.Contains(n))
                        {
                            float dist = Vector2Int.Distance(n, playerNext);
                            if (dist < bestDist)
                            {
                                best = n;
                                bestDist = dist;
                                moveDirection = d;
                            }
                        }
                    }
                    if (best != oldPos)
                    {
                        newPos = best;
                        mazeObj.enemies[i] = newPos;
                        if (i < enemyDataList.Count)
                            enemyDataList[i].direction = moveDirection;
                    }
                    continue;
                }

                // --- 2. Inimigo 1: foge de balas próximas ---
                if (i == 1 && mazeObj.bullets.Count > 0)
                {
                    bool bulletNear = false;
                    foreach (var b in mazeObj.bullets)
                    {
                        if (Vector2Int.Distance(oldPos, b.pos) <= 2f)
                        {
                            bulletNear = true;
                            break;
                        }
                    }
                    if (bulletNear)
                    {
                        // Foge do player e das balas
                        Vector2Int best = oldPos;
                        float bestDist = Vector2Int.Distance(oldPos, mazeObj.playerPos);
                        foreach (var d in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
                        {
                            Vector2Int n = oldPos + d;
                            if (MazePlayerUtils.IsValid(mazeObj, n) && mazeObj.maze[n.x, n.y] == 0 &&
                                n != mazeObj.playerPos && !mazeObj.enemies.Contains(n))
                            {
                                float dist = Vector2Int.Distance(n, mazeObj.playerPos);
                                if (dist > bestDist)
                                {
                                    best = n;
                                    bestDist = dist;
                                    moveDirection = d;
                                }
                            }
                        }
                        if (best != oldPos)
                        {
                            newPos = best;
                            mazeObj.enemies[i] = newPos;
                            if (i < enemyDataList.Count)
                                enemyDataList[i].direction = moveDirection;
                        }
                        continue;
                    }
                }

                // --- 3. Restantes: patrulha simples (anda em linha até bater) ---
                if (i >= 2)
                {
                    // Salva direção de patrulha no próprio index (alternando entre up/right)
                    Vector2Int patrolDir = (i % 2 == 0) ? Vector2Int.up : Vector2Int.right;
                    Vector2Int n = oldPos + patrolDir;
                    if (MazePlayerUtils.IsValid(mazeObj, n) && mazeObj.maze[n.x, n.y] == 0 &&
                        n != mazeObj.playerPos && !mazeObj.enemies.Contains(n))
                    {
                        newPos = n;
                        moveDirection = patrolDir;
                    }
                    else
                    {
                        // Se bater, tenta direção oposta
                        patrolDir = -patrolDir;
                        n = oldPos + patrolDir;
                        if (MazePlayerUtils.IsValid(mazeObj, n) && mazeObj.maze[n.x, n.y] == 0 &&
                            n != mazeObj.playerPos && !mazeObj.enemies.Contains(n))
                        {
                            newPos = n;
                            moveDirection = patrolDir;
                        }
                    }
                    
                    if (newPos != oldPos)
                    {
                        mazeObj.enemies[i] = newPos;
                        if (i < enemyDataList.Count)
                            enemyDataList[i].direction = moveDirection;
                    }
                    continue;
                }

                // --- Comportamento padrão: perseguição se perto, senão aleatório ---
                if (Vector2Int.Distance(oldPos, mazeObj.playerPos) <= 4f)
                {
                    Vector2Int best = oldPos;
                    float bestDist = Vector2Int.Distance(oldPos, mazeObj.playerPos);

                    foreach (var d in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
                    {
                        Vector2Int n = oldPos + d;
                        if (MazePlayerUtils.IsValid(mazeObj, n) && mazeObj.maze[n.x, n.y] == 0 &&
                            n != mazeObj.playerPos && !mazeObj.enemies.Contains(n))
                        {
                            float dist = Vector2Int.Distance(n, mazeObj.playerPos);
                            if (dist < bestDist)
                            {
                                best = n;
                                bestDist = dist;
                                moveDirection = d;
                            }
                        }
                    }
                    if (best != oldPos)
                    {
                        newPos = best;
                        mazeObj.enemies[i] = newPos;
                        if (i < enemyDataList.Count)
                            enemyDataList[i].direction = moveDirection;
                    }
                }
                else
                {
                    List<Vector2Int> options = new List<Vector2Int>();
                    foreach (var d in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
                    {
                        Vector2Int n = oldPos + d;
                        if (MazePlayerUtils.IsValid(mazeObj, n) && mazeObj.maze[n.x, n.y] == 0 &&
                            n != mazeObj.playerPos && !mazeObj.enemies.Contains(n))
                            options.Add(d);
                    }
                    if (options.Count > 0)
                    {
                        moveDirection = options[rng.Next(options.Count)];
                        newPos = oldPos + moveDirection;
                        mazeObj.enemies[i] = newPos;
                        if (i < enemyDataList.Count)
                            enemyDataList[i].direction = moveDirection;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Obtém a direção atual de um inimigo específico
    /// </summary>
    public static Vector2Int GetEnemyDirection(int enemyIndex)
    {
        if (enemyIndex >= 0 && enemyIndex < enemyDataList.Count)
        {
            return enemyDataList[enemyIndex].direction;
        }
        return Vector2Int.right; // Direção padrão
    }

    /// <summary>
    /// Obtém o ângulo de rotação baseado na direção do inimigo
    /// </summary>
    public static float GetEnemyRotation(int enemyIndex)
    {
        Vector2Int direction = GetEnemyDirection(enemyIndex);
        
        if (direction == Vector2Int.right) return 0f;      // Olhando para direita
        if (direction == Vector2Int.left) return 180f;     // Olhando para esquerda
        if (direction == Vector2Int.up) return 90f;        // Olhando para cima
        if (direction == Vector2Int.down) return 270f;     // Olhando para baixo
        
        return 0f; // Padrão
    }

    /// <summary>
    /// Colisão de inimigo com o jogador: só causa dano se escudo não estiver ativo.
    /// </summary>
    public static void HandleEnemyPlayerCollision(ProceduralMaze mazeObj)
    {
        if (ProceduralMaze.gameState != GameState.Playing)
            return;

        // Colisão com inimigo só afeta se escudo não estiver ativo
        if (mazeObj.enemies.Contains(mazeObj.playerPos))
        {
            if (mazeObj.shieldActive)
            {
                // Dano ignorado, pode tocar um som de escudo!
                if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.powerUpSound);
                // Opcional: resetar escudo ao contato, ou apenas ignorar dano
                return;
            }

            if (AudioManager.Instance) AudioManager.Instance.PlaySFX(AudioManager.Instance.playerDeathSound);
            mazeObj.LoseLifeAndRespawn();
        }
    }
}
