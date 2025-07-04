using UnityEngine;
using System.Collections.Generic;

public static class MazeTouchControls
{
    // Configurações de touch
    private static bool touchEnabled = false;
    private static Vector2 joystickCenter = Vector2.zero;
    private static Vector2 joystickCurrent = Vector2.zero;
    private static bool joystickActive = false;
    private static float joystickRadius = 80f;
    private static float joystickDeadzone = 20f;
    
    // Botões de ação
    private static Rect shootButtonRect;
    private static Rect teleportButtonRect;
    private static bool shootButtonPressed = false;
    private static bool teleportButtonPressed = false;
    
    // Configurações de botões
    private static float buttonSize = 80f;
    private static float buttonMargin = 20f;
    
    // Inicializar controles touch
    public static void InitializeTouchControls()
    {
        touchEnabled = Application.isMobilePlatform || Input.touchSupported;
        
        if (touchEnabled)
        {
            // Posicionar joystick no canto inferior esquerdo
            joystickCenter = new Vector2(buttonMargin + joystickRadius, Screen.height - buttonMargin - joystickRadius);
            
            // Posicionar botões no canto inferior direito
            float buttonY = Screen.height - buttonMargin - buttonSize;
            shootButtonRect = new Rect(Screen.width - buttonMargin - buttonSize * 2 - 10f, buttonY, buttonSize, buttonSize);
            teleportButtonRect = new Rect(Screen.width - buttonMargin - buttonSize, buttonY, buttonSize, buttonSize);
        }
    }
    
    // Processar input touch
    public static Vector2Int ProcessTouchInput()
    {
        if (!touchEnabled) return Vector2Int.zero;
        
        Vector2Int input = Vector2Int.zero;
        
        // Processar toques
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector2 touchPos = touch.position;
            
            // Verificar se é toque no joystick
            if (IsJoystickTouch(touchPos))
            {
                ProcessJoystickTouch(touch);
                input = GetJoystickDirection();
            }
            
            // Verificar botões de ação
            if (touch.phase == TouchPhase.Began)
            {
                if (shootButtonRect.Contains(touchPos))
                {
                    shootButtonPressed = true;
                }
                else if (teleportButtonRect.Contains(touchPos))
                {
                    teleportButtonPressed = true;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (shootButtonRect.Contains(touchPos))
                {
                    shootButtonPressed = false;
                }
                else if (teleportButtonRect.Contains(touchPos))
                {
                    teleportButtonPressed = false;
                }
            }
        }
        
        return input;
    }
    
    // Verificar se toque está na área do joystick
    private static bool IsJoystickTouch(Vector2 touchPos)
    {
        float distance = Vector2.Distance(touchPos, joystickCenter);
        return distance <= joystickRadius * 1.5f; // Área um pouco maior para facilitar
    }
    
    // Processar toque no joystick
    private static void ProcessJoystickTouch(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            joystickActive = true;
        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            Vector2 direction = touch.position - joystickCenter;
            float distance = direction.magnitude;
            
            if (distance > joystickDeadzone)
            {
                if (distance > joystickRadius)
                {
                    direction = direction.normalized * joystickRadius;
                }
                joystickCurrent = direction;
            }
            else
            {
                joystickCurrent = Vector2.zero;
            }
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            joystickActive = false;
            joystickCurrent = Vector2.zero;
        }
    }
    
    // Obter direção do joystick
    private static Vector2Int GetJoystickDirection()
    {
        if (!joystickActive || joystickCurrent.magnitude < joystickDeadzone)
            return Vector2Int.zero;
        
        // Converter para direção discreta (4 direções)
        float angle = Mathf.Atan2(joystickCurrent.y, joystickCurrent.x) * Mathf.Rad2Deg;
        
        if (angle >= -45f && angle < 45f) return Vector2Int.right;   // Direita
        if (angle >= 45f && angle < 135f) return Vector2Int.up;      // Cima
        if (angle >= 135f || angle < -135f) return Vector2Int.left;  // Esquerda
        return Vector2Int.down; // Baixo
    }
    
    // Verificar se botão de tiro foi pressionado
    public static bool IsShootButtonPressed()
    {
        return shootButtonPressed;
    }
    
    // Verificar se botão de teleport foi pressionado
    public static bool IsTeleportButtonPressed()
    {
        return teleportButtonPressed;
    }
    
    // Renderizar controles touch
    public static void RenderTouchControls()
    {
        if (!touchEnabled) return;
        
        // Estilo para controles
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;
        
        // Desenhar joystick
        Color oldColor = GUI.color;
        
        // Base do joystick
        GUI.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        GUI.DrawTexture(new Rect(joystickCenter.x - joystickRadius, joystickCenter.y - joystickRadius, 
                                joystickRadius * 2, joystickRadius * 2), Texture2D.whiteTexture);
        
        // Contorno do joystick
        GUI.color = new Color(0.8f, 0.8f, 0.8f, 0.6f);
        GUI.DrawTexture(new Rect(joystickCenter.x - joystickRadius + 2, joystickCenter.y - joystickRadius + 2, 
                                joystickRadius * 2 - 4, joystickRadius * 2 - 4), Texture2D.whiteTexture);
        
        // Alavanca do joystick
        if (joystickActive)
        {
            GUI.color = new Color(0.3f, 0.7f, 1f, 0.9f);
            Vector2 stickPos = joystickCenter + joystickCurrent;
            GUI.DrawTexture(new Rect(stickPos.x - 15, stickPos.y - 15, 30, 30), Texture2D.whiteTexture);
        }
        
        // Botão de tiro
        GUI.color = shootButtonPressed ? new Color(1f, 0.3f, 0.3f, 0.9f) : new Color(0.8f, 0.2f, 0.2f, 0.8f);
        GUI.DrawTexture(shootButtonRect, Texture2D.whiteTexture);
        GUI.color = Color.white;
        GUI.Label(shootButtonRect, "🔫", style);
        
        // Botão de teleport
        GUI.color = teleportButtonPressed ? new Color(0.3f, 0.3f, 1f, 0.9f) : new Color(0.2f, 0.2f, 0.8f, 0.8f);
        GUI.DrawTexture(teleportButtonRect, Texture2D.whiteTexture);
        GUI.color = Color.white;
        GUI.Label(teleportButtonRect, "⚡", style);
        
        GUI.color = oldColor;
    }
    
    // Verificar se controles touch estão habilitados
    public static bool IsTouchEnabled()
    {
        return touchEnabled;
    }
} 