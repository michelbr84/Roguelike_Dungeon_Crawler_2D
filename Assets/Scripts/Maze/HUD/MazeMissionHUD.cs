using UnityEngine;

public static class MazeMissionHUD
{
    public static void Draw()
    {
        // Implementação do sistema de missões
        var missions = MazeMissions.GetActiveMissions();
        if (missions != null && missions.Count > 0)
        {
            GUIStyle missionStyle = new GUIStyle(GUI.skin.label);
            missionStyle.fontSize = 16;
            missionStyle.normal.textColor = Color.cyan;
            missionStyle.alignment = TextAnchor.UpperLeft;

            int y = 10;
            foreach (var mission in missions)
            {
                GUI.Label(new Rect(10, y, 300, 20), mission.description, missionStyle);
                y += 20;
            }
        }
    }
} 