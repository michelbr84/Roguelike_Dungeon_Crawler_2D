// PowerUp.cs
using UnityEngine;

/// <summary>
/// Tipos de power-ups suportados.
/// </summary>
public enum PowerUpType
{
    Ammo,   // + munição
    Life,   // + vida extra
    Shield, // escudo temporário
    DoubleShot, // tiros duplos temporários
    TripleShot, // tiros triplos temporários
    SpeedBoost, // velocidade extra temporária
    Invisibility, // inimigos ignoram o jogador por alguns segundos
    Teleport, // teleporta para local seguro/aleatório
    ScoreBooster // dobra pontos por X segundos
    // Adicione outros tipos se quiser
}

/// <summary>
/// Representa um power-up colocado no labirinto.
/// </summary>
public class PowerUp
{
    public Vector2Int position;    // Posição do power-up no grid
    public PowerUpType type;       // Tipo do power-up
    public bool active;            // Ativo ou não (útil para sumiço temporário ou respawn)

    public PowerUp(Vector2Int position, PowerUpType type)
    {
        this.position = position;
        this.type = type;
        this.active = true;
    }
}
