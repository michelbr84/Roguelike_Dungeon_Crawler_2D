using UnityEngine;

public static class MazePowerUpRenderer
{
    public static void DrawPowerUps(ProceduralMaze mazeObj, float cellSize)
    {
        if (mazeObj.powerUps != null)
        {
            foreach (var pu in mazeObj.powerUps)
            {
                Rect cellRect = new Rect(pu.position.x * cellSize, pu.position.y * cellSize, cellSize, cellSize);
                Color puColor = Color.white;
                Texture2D puTexture = null;
                switch (pu.type)
                {
                    case PowerUpType.Ammo:
                        puColor = Color.cyan;
                        puTexture = mazeObj.ammoTexture;
                        break;
                    case PowerUpType.Life:
                        puColor = Color.red;
                        puTexture = mazeObj.extraLifeTexture;
                        break;
                    case PowerUpType.Shield:
                        puColor = Color.green;
                        puTexture = mazeObj.shieldTexture;
                        break;
                    case PowerUpType.DoubleShot:
                        puColor = Color.yellow;
                        puTexture = mazeObj.doubleShotTexture;
                        break;
                    case PowerUpType.TripleShot:
                        puColor = Color.magenta;
                        puTexture = mazeObj.tripleShotTexture;
                        break;
                    case PowerUpType.SpeedBoost:
                        puColor = Color.cyan;
                        puTexture = mazeObj.speedBoostTexture;
                        break;
                    case PowerUpType.Invisibility:
                        puColor = Color.gray;
                        puTexture = mazeObj.invisibilityTexture;
                        break;
                    case PowerUpType.Teleport:
                        puColor = Color.blue;
                        puTexture = mazeObj.teleportTexture;
                        break;
                    case PowerUpType.ScoreBooster:
                        puColor = Color.green;
                        puTexture = mazeObj.scoreBoosterTexture;
                        break;
                    default:
                        puColor = Color.white;
                        break;
                }
                if (puTexture)
                {
                    GUI.DrawTexture(cellRect, puTexture, ScaleMode.ScaleToFit);
                }
                else
                {
                    Color oldColor = GUI.color;
                    GUI.color = puColor;
                    GUI.DrawTexture(cellRect, Texture2D.whiteTexture);
                    GUI.color = oldColor;
                }
            }
        }
    }
} 