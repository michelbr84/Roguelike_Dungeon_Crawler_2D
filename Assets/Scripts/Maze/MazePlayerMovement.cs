using UnityEngine;

public static class MazePlayerMovement
{
    public static void HandleInput(ProceduralMaze mazeObj)
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
                if (MazePlayerUtils.IsValid(mazeObj, newPosBoost) && mazeObj.maze[newPosBoost.x, newPosBoost.y] == 0)
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
            if (inputDir != Vector2Int.zero && MazePlayerUtils.IsValid(mazeObj, newPos) && mazeObj.maze[newPos.x, newPos.y] == 0)
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

        // Chamar sistema de disparo
        MazePlayerShooting.HandleShooting(mazeObj);
    }
} 