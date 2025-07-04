using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.IO;

public static class MazeBuildSystem
{
    // Configurações de build
    private static readonly string[] scenes = {
        "Assets/Scenes/Start.unity",
        "Assets/Scenes/Mobile.unity"
    };
    
    private static readonly string buildPath = "Builds/";
    private static readonly string gameName = "RoguelikeDungeonCrawler";
    
    // Build para Windows
    [MenuItem("Build/Build Windows")]
    public static void BuildWindows()
    {
        string path = buildPath + gameName + "_Windows.exe";
        BuildGame(BuildTarget.StandaloneWindows64, path);
    }
    
    // Build para Android
    [MenuItem("Build/Build Android")]
    public static void BuildAndroid()
    {
        string path = buildPath + gameName + "_Android.apk";
        BuildGame(BuildTarget.Android, path);
    }
    
    // Build para WebGL
    [MenuItem("Build/Build WebGL")]
    public static void BuildWebGL()
    {
        string path = buildPath + gameName + "_WebGL";
        BuildGame(BuildTarget.WebGL, path);
    }
    
    // Build para iOS
    [MenuItem("Build/Build iOS")]
    public static void BuildIOS()
    {
        string path = buildPath + gameName + "_iOS";
        BuildGame(BuildTarget.iOS, path);
    }
    
    // Build para macOS
    [MenuItem("Build/Build macOS")]
    public static void BuildMacOS()
    {
        string path = buildPath + gameName + "_macOS.app";
        BuildGame(BuildTarget.StandaloneOSX, path);
    }
    
    // Build para Linux
    [MenuItem("Build/Build Linux")]
    public static void BuildLinux()
    {
        string path = buildPath + gameName + "_Linux";
        BuildGame(BuildTarget.StandaloneLinux64, path);
    }
    
    // Build para todas as plataformas
    [MenuItem("Build/Build All Platforms")]
    public static void BuildAllPlatforms()
    {
        BuildWindows();
        BuildAndroid();
        BuildWebGL();
        BuildMacOS();
        BuildLinux();
    }
    
    // Método principal de build
    private static void BuildGame(BuildTarget target, string path)
    {
        // Criar diretório de build se não existir
        Directory.CreateDirectory(buildPath);
        
        // Configurar build settings
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = path;
        buildPlayerOptions.target = target;
        buildPlayerOptions.options = BuildOptions.None;
        
        // Configurações específicas por plataforma
        switch (target)
        {
            case BuildTarget.Android:
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.yourcompany.roguelikedungeoncrawler");
                PlayerSettings.productName = "Roguelike Dungeon Crawler";
                PlayerSettings.companyName = "Your Company";
                break;
                
            case BuildTarget.iOS:
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, "com.yourcompany.roguelikedungeoncrawler");
                PlayerSettings.productName = "Roguelike Dungeon Crawler";
                PlayerSettings.companyName = "Your Company";
                break;
                
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.StandaloneOSX:
            case BuildTarget.StandaloneLinux64:
                PlayerSettings.productName = "Roguelike Dungeon Crawler";
                PlayerSettings.companyName = "Your Company";
                break;
        }
        
        // Executar build
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        
        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"Build succeeded: {path}");
        }
        else
        {
            Debug.LogError($"Build failed: {path}");
        }
    }
    
    // Limpar builds antigas
    [MenuItem("Build/Clean Builds")]
    public static void CleanBuilds()
    {
        if (Directory.Exists(buildPath))
        {
            Directory.Delete(buildPath, true);
            Debug.Log("Builds limpas!");
        }
    }
    
    // Validar build
    [MenuItem("Build/Validate Build")]
    public static void ValidateBuild()
    {
        Debug.Log("Validando build...");
        
        // Verificar se todas as cenas existem
        foreach (string scene in scenes)
        {
            if (!File.Exists(scene))
            {
                Debug.LogError($"Cena não encontrada: {scene}");
                return;
            }
        }
        
        // Verificar se todos os assets necessários existem
        string[] requiredAssets = {
            "Assets/Audio/Music/Background.mp3",
            "Assets/Images/Player.png",
            "Assets/Images/Enemy.png",
            "Assets/Images/Wall.png"
        };
        
        foreach (string asset in requiredAssets)
        {
            if (!File.Exists(asset))
            {
                Debug.LogError($"Asset não encontrado: {asset}");
                return;
            }
        }
        
        Debug.Log("Build validada com sucesso!");
    }
    
    // Configurar qualidade de build
    [MenuItem("Build/Set High Quality")]
    public static void SetHighQuality()
    {
        QualitySettings.SetQualityLevel(2); // High
        QualitySettings.vSyncCount = 0;
        PlayerSettings.runInBackground = true;
        Debug.Log("Qualidade configurada para High");
    }
    
    [MenuItem("Build/Set Medium Quality")]
    public static void SetMediumQuality()
    {
        QualitySettings.SetQualityLevel(1); // Medium
        QualitySettings.vSyncCount = 0;
        PlayerSettings.runInBackground = true;
        Debug.Log("Qualidade configurada para Medium");
    }
    
    [MenuItem("Build/Set Low Quality")]
    public static void SetLowQuality()
    {
        QualitySettings.SetQualityLevel(0); // Low
        QualitySettings.vSyncCount = 0;
        PlayerSettings.runInBackground = true;
        Debug.Log("Qualidade configurada para Low");
    }
} 