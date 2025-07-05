using UnityEngine;

public static class MazePlayerProtection
{
    private static float spawnProtectionTimer = 0f;
    private const float SPAWN_PROTECTION_DURATION = 3f;

    public static void ActivateSpawnProtection()
    {
        spawnProtectionTimer = SPAWN_PROTECTION_DURATION;
    }

    public static bool IsSpawnProtected()
    {
        return spawnProtectionTimer > 0f;
    }

    public static void UpdateProtectionTimer()
    {
        if (spawnProtectionTimer > 0f)
            spawnProtectionTimer -= Time.deltaTime;
    }
} 