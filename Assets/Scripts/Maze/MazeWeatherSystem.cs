using UnityEngine;
using System.Collections.Generic;

public static class MazeWeatherSystem
{
    // Tipos de clima
    public enum WeatherType
    {
        Clear,      // Claro - Normal
        Rain,       // Chuva - Reduz visibilidade
        Fog,        // Névoa - Reduz alcance de tiro
        Storm,      // Tempestade - Inimigos mais agressivos
        Snow,       // Neve - Movimento mais lento
        Heat,       // Calor - Consome mais energia
        Wind,       // Vento - Afeta trajetória dos projéteis
        Darkness    // Escuridão - Visibilidade limitada
    }
    
    // Efeitos do clima
    [System.Serializable]
    public class WeatherEffect
    {
        public WeatherType type;
        public string name;
        public string description;
        public Color tintColor;
        public float duration;
        public float intensity;
        
        // Modificadores de gameplay
        public float visibilityModifier;
        public float movementModifier;
        public float damageModifier;
        public float enemyAggressionModifier;
        public float projectileSpeedModifier;
        public float energyConsumptionModifier;
        
        public WeatherEffect(WeatherType weatherType)
        {
            type = weatherType;
            SetDefaultsForType(weatherType);
        }
        
        private void SetDefaultsForType(WeatherType weatherType)
        {
            switch (weatherType)
            {
                case WeatherType.Clear:
                    name = "Claro";
                    description = "Condições normais";
                    tintColor = Color.white;
                    duration = 0f; // Permanente
                    intensity = 1f;
                    visibilityModifier = 1f;
                    movementModifier = 1f;
                    damageModifier = 1f;
                    enemyAggressionModifier = 1f;
                    projectileSpeedModifier = 1f;
                    energyConsumptionModifier = 1f;
                    break;
                    
                case WeatherType.Rain:
                    name = "Chuva";
                    description = "Reduz visibilidade e movimento";
                    tintColor = new Color(0.7f, 0.7f, 1f, 1f);
                    duration = 30f;
                    intensity = 0.8f;
                    visibilityModifier = 0.7f;
                    movementModifier = 0.9f;
                    damageModifier = 1f;
                    enemyAggressionModifier = 1.1f;
                    projectileSpeedModifier = 0.9f;
                    energyConsumptionModifier = 1.2f;
                    break;
                    
                case WeatherType.Fog:
                    name = "Névoa";
                    description = "Reduz alcance de tiro e visibilidade";
                    tintColor = new Color(0.8f, 0.8f, 0.8f, 1f);
                    duration = 45f;
                    intensity = 0.6f;
                    visibilityModifier = 0.5f;
                    movementModifier = 1f;
                    damageModifier = 0.8f;
                    enemyAggressionModifier = 1f;
                    projectileSpeedModifier = 0.7f;
                    energyConsumptionModifier = 1f;
                    break;
                    
                case WeatherType.Storm:
                    name = "Tempestade";
                    description = "Inimigos mais agressivos e visibilidade reduzida";
                    tintColor = new Color(0.3f, 0.3f, 0.5f, 1f);
                    duration = 20f;
                    intensity = 1.2f;
                    visibilityModifier = 0.6f;
                    movementModifier = 0.8f;
                    damageModifier = 1.3f;
                    enemyAggressionModifier = 1.5f;
                    projectileSpeedModifier = 0.8f;
                    energyConsumptionModifier = 1.5f;
                    break;
                    
                case WeatherType.Snow:
                    name = "Neve";
                    description = "Movimento mais lento mas inimigos também";
                    tintColor = new Color(0.9f, 0.9f, 1f, 1f);
                    duration = 40f;
                    intensity = 0.9f;
                    visibilityModifier = 0.8f;
                    movementModifier = 0.7f;
                    damageModifier = 1f;
                    enemyAggressionModifier = 0.8f;
                    projectileSpeedModifier = 0.9f;
                    energyConsumptionModifier = 1.3f;
                    break;
                    
                case WeatherType.Heat:
                    name = "Calor";
                    description = "Consome mais energia mas aumenta dano";
                    tintColor = new Color(1f, 0.8f, 0.6f, 1f);
                    duration = 35f;
                    intensity = 1.1f;
                    visibilityModifier = 1f;
                    movementModifier = 0.9f;
                    damageModifier = 1.2f;
                    enemyAggressionModifier = 1.2f;
                    projectileSpeedModifier = 1.1f;
                    energyConsumptionModifier = 1.8f;
                    break;
                    
                case WeatherType.Wind:
                    name = "Vento";
                    description = "Afeta trajetória dos projéteis";
                    tintColor = new Color(0.8f, 1f, 0.8f, 1f);
                    duration = 25f;
                    intensity = 1f;
                    visibilityModifier = 1f;
                    movementModifier = 1f;
                    damageModifier = 1f;
                    enemyAggressionModifier = 1f;
                    projectileSpeedModifier = 0.6f;
                    energyConsumptionModifier = 1.1f;
                    break;
                    
                case WeatherType.Darkness:
                    name = "Escuridão";
                    description = "Visibilidade muito limitada";
                    tintColor = new Color(0.2f, 0.2f, 0.3f, 1f);
                    duration = 30f;
                    intensity = 0.4f;
                    visibilityModifier = 0.3f;
                    movementModifier = 1f;
                    damageModifier = 0.9f;
                    enemyAggressionModifier = 1.3f;
                    projectileSpeedModifier = 1f;
                    energyConsumptionModifier = 1.4f;
                    break;
            }
        }
    }
    
    // Clima atual
    private static WeatherEffect currentWeather;
    private static float weatherTimer;
    private static bool isWeatherActive = false;
    
    // Lista de climas disponíveis
    private static List<WeatherType> availableWeathers = new List<WeatherType>();
    
    // Inicializar sistema
    public static void Initialize()
    {
        // Clima padrão
        currentWeather = new WeatherEffect(WeatherType.Clear);
        isWeatherActive = false;
        
        // Configurar climas disponíveis
        availableWeathers.Clear();
        availableWeathers.Add(WeatherType.Clear);
        availableWeathers.Add(WeatherType.Rain);
        availableWeathers.Add(WeatherType.Fog);
        availableWeathers.Add(WeatherType.Storm);
        availableWeathers.Add(WeatherType.Snow);
        availableWeathers.Add(WeatherType.Heat);
        availableWeathers.Add(WeatherType.Wind);
        availableWeathers.Add(WeatherType.Darkness);
    }
    
    // Atualizar sistema
    public static void Update(float deltaTime)
    {
        if (isWeatherActive && currentWeather.type != WeatherType.Clear)
        {
            weatherTimer -= deltaTime;
            
            if (weatherTimer <= 0f)
            {
                ClearWeather();
            }
        }
    }
    
    // Aplicar clima
    public static void ApplyWeather(WeatherType weatherType)
    {
        if (weatherType == WeatherType.Clear)
        {
            ClearWeather();
            return;
        }
        
        currentWeather = new WeatherEffect(weatherType);
        weatherTimer = currentWeather.duration;
        isWeatherActive = true;
        
        // Mostrar mensagem
        MazeHUD.ShowStatusMessage($"Clima mudou para: {currentWeather.name}");
        
        // Aplicar efeitos visuais
        ApplyVisualEffects();
    }
    
    // Limpar clima
    public static void ClearWeather()
    {
        currentWeather = new WeatherEffect(WeatherType.Clear);
        isWeatherActive = false;
        weatherTimer = 0f;
        
        // Remover efeitos visuais
        RemoveVisualEffects();
        
        MazeHUD.ShowStatusMessage("Clima voltou ao normal");
    }
    
    // Gerar clima aleatório
    public static void GenerateRandomWeather()
    {
        if (Random.Range(0f, 1f) < 0.3f) // 30% chance de mudar clima
        {
            WeatherType randomWeather = availableWeathers[Random.Range(0, availableWeathers.Count)];
            if (randomWeather != WeatherType.Clear)
            {
                ApplyWeather(randomWeather);
            }
        }
    }
    
    // Obter clima atual
    public static WeatherEffect GetCurrentWeather()
    {
        return currentWeather;
    }
    
    // Verificar se clima está ativo
    public static bool IsWeatherActive()
    {
        return isWeatherActive;
    }
    
    // Obter tempo restante do clima
    public static float GetWeatherTimeRemaining()
    {
        return isWeatherActive ? weatherTimer : 0f;
    }
    
    // Aplicar efeitos visuais
    private static void ApplyVisualEffects()
    {
        // Aqui você pode implementar efeitos visuais específicos
        // como partículas de chuva, neve, etc.
        
        // Por enquanto, apenas aplicar tint
        if (currentWeather.type != WeatherType.Clear)
        {
            // Implementar efeito de tint na câmera ou renderer
        }
    }
    
    // Remover efeitos visuais
    private static void RemoveVisualEffects()
    {
        // Remover efeitos visuais aplicados
    }
    
    // Obter modificadores de gameplay
    public static float GetVisibilityModifier()
    {
        return currentWeather.visibilityModifier;
    }
    
    public static float GetMovementModifier()
    {
        return currentWeather.movementModifier;
    }
    
    public static float GetDamageModifier()
    {
        return currentWeather.damageModifier;
    }
    
    public static float GetEnemyAggressionModifier()
    {
        return currentWeather.enemyAggressionModifier;
    }
    
    public static float GetProjectileSpeedModifier()
    {
        return currentWeather.projectileSpeedModifier;
    }
    
    public static float GetEnergyConsumptionModifier()
    {
        return currentWeather.energyConsumptionModifier;
    }
    
    // Obter nome do tipo de clima
    public static string GetWeatherTypeName(WeatherType weatherType)
    {
        switch (weatherType)
        {
            case WeatherType.Clear: return "Claro";
            case WeatherType.Rain: return "Chuva";
            case WeatherType.Fog: return "Névoa";
            case WeatherType.Storm: return "Tempestade";
            case WeatherType.Snow: return "Neve";
            case WeatherType.Heat: return "Calor";
            case WeatherType.Wind: return "Vento";
            case WeatherType.Darkness: return "Escuridão";
            default: return "Desconhecido";
        }
    }
    
    // Obter cor do tipo de clima
    public static Color GetWeatherTypeColor(WeatherType weatherType)
    {
        switch (weatherType)
        {
            case WeatherType.Clear: return Color.white;
            case WeatherType.Rain: return Color.blue;
            case WeatherType.Fog: return Color.gray;
            case WeatherType.Storm: return Color.black;
            case WeatherType.Snow: return Color.cyan;
            case WeatherType.Heat: return Color.red;
            case WeatherType.Wind: return Color.green;
            case WeatherType.Darkness: return Color.magenta;
            default: return Color.white;
        }
    }
    
    // Desbloquear novo tipo de clima
    public static void UnlockWeather(WeatherType weatherType)
    {
        if (!availableWeathers.Contains(weatherType))
        {
            availableWeathers.Add(weatherType);
            string weatherName = GetWeatherTypeName(weatherType);
            MazeHUD.ShowStatusMessage($"Novo clima desbloqueado: {weatherName}!");
        }
    }
    
    // Gerar clima baseado no nível
    public static WeatherType GenerateWeatherForLevel(int level)
    {
        // Climas mais difíceis aparecem em níveis mais altos
        if (level >= 20 && Random.Range(0f, 1f) < 0.4f)
        {
            return WeatherType.Darkness;
        }
        else if (level >= 15 && Random.Range(0f, 1f) < 0.3f)
        {
            return WeatherType.Storm;
        }
        else if (level >= 10 && Random.Range(0f, 1f) < 0.25f)
        {
            return WeatherType.Heat;
        }
        else if (level >= 5 && Random.Range(0f, 1f) < 0.2f)
        {
            return WeatherType.Fog;
        }
        else if (Random.Range(0f, 1f) < 0.15f)
        {
            return WeatherType.Rain;
        }
        
        return WeatherType.Clear;
    }
    
    // Salvar estado do clima
    public static void SaveWeatherState()
    {
        PlayerPrefs.SetInt("CurrentWeather", (int)currentWeather.type);
        PlayerPrefs.SetFloat("WeatherTimer", weatherTimer);
        PlayerPrefs.SetInt("IsWeatherActive", isWeatherActive ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    // Carregar estado do clima
    public static void LoadWeatherState()
    {
        WeatherType savedWeather = (WeatherType)PlayerPrefs.GetInt("CurrentWeather", 0);
        weatherTimer = PlayerPrefs.GetFloat("WeatherTimer", 0f);
        isWeatherActive = PlayerPrefs.GetInt("IsWeatherActive", 0) == 1;
        
        if (isWeatherActive && savedWeather != WeatherType.Clear)
        {
            currentWeather = new WeatherEffect(savedWeather);
        }
        else
        {
            currentWeather = new WeatherEffect(WeatherType.Clear);
            isWeatherActive = false;
        }
    }
} 