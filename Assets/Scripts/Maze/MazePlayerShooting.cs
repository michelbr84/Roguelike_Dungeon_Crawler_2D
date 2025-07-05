using UnityEngine;

public static class MazePlayerShooting
{
    public static void HandleShooting(ProceduralMaze mazeObj)
    {
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
} 