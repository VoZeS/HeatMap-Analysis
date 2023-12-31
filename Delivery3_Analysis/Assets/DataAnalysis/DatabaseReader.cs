using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;


[System.Serializable]
public class HeatMapKillData
{
    public int KillID;
    public int SessionID;
    public int RunID;

    // Positions
    public int PlayerKiller_PositionX;
    public int PlayerKiller_PositionY;
    public int PlayerKiller_PositionZ;

    public int EnemyDeath_PositionX;
    public int EnemyDeath_PositionY;
    public int EnemyDeath_PositionZ;

    // Vectors
    public Vector3 playerKillerPosition;
    public Vector3 enemyDeathPosition;

    public string Time;

    public HeatMapKillData(int kill_id, int session_id, int run_id, int playerKillerX, int playerKillerY, int playerKillerZ,
                           int enemyDeathX, int enemyDeathY, int enemyDeathZ, string _time)
    {
        KillID = kill_id;
        SessionID = session_id;
        RunID = run_id;
        playerKillerPosition = new Vector3(playerKillerX, playerKillerY, playerKillerZ);
        enemyDeathPosition = new Vector3(enemyDeathX, enemyDeathY, enemyDeathZ);
        Time = _time;
    }
}

[System.Serializable]
public class HeatMapDeathData
{
    public int DeathID;
    public int SessionID;
    public int RunID;

    // Positions
    public int PlayerDeath_PositionX;
    public int PlayerDeath_PositionY;
    public int PlayerDeath_PositionZ;

    public int EnemyKiller_PositionX;
    public int EnemyKiller_PositionY;
    public int EnemyKiller_PositionZ;

    // Vectors
    public Vector3 playerDeathPosition;
    public Vector3 enemyKillerPosition;

    public string Time;

    public HeatMapDeathData(int death_id, int session_id, int run_id, int playerDeathX, int playerDeathY, int playerDeathZ,
                           int enemyKillerX, int enemyKillerY, int enemyKillerZ, string _time)
    {
        DeathID = death_id;
        SessionID = session_id;
        RunID = run_id;
        playerDeathPosition = new Vector3(playerDeathX, playerDeathY, playerDeathZ);
        enemyKillerPosition = new Vector3(enemyKillerX, enemyKillerY, enemyKillerZ);
        Time = _time;
    }
}

[System.Serializable]
public class PathData
{
    public int PathID;
    public int SessionID;
    public int RunID;

    // Positions
    public int Player_PositionX;
    public int Player_PositionY;
    public int Player_PositionZ;

    public int Player_RotationX;
    public int Player_RotationY;
    public int Player_RotationZ;

    // Vectors
    public Vector3 playerPosition;
    public Vector3 playerRotation;

    public string Time;

    public PathData(int path_id, int session_id, int run_id, int playerPositionX, int playerPositionY, int playerPositionZ,
                           int playerRotationX, int playerRotationY, int playerRotationZ, string _time)
    {
        PathID = path_id;
        SessionID = session_id;
        RunID = run_id;
        playerPosition = new Vector3(playerPositionX, playerPositionY, playerPositionZ);
        playerRotation = new Vector3(playerRotationX, playerRotationY, playerRotationZ);
        Time = _time;
    }
}

public class DatabaseReader : MonoBehaviour
{
    public List<HeatMapKillData> killDataList = new List<HeatMapKillData>();
    public List<HeatMapDeathData> deathDataList = new List<HeatMapDeathData>();
    public List<PathData> pathDataList = new List<PathData>();

    void Start()
    {
        // Called in START, but can be called when NECESSARY
        StartCoroutine(ReadKillDataFromPHP());
        StartCoroutine(ReadDeathDataFromPHP());
        StartCoroutine(ReadPathDataFromPHP());
    }

    IEnumerator ReadKillDataFromPHP()
    {
  
        UnityWebRequest www = UnityWebRequest.Get("https://citmalumnes.upc.es/~davidbo5/Database_KillReader.php");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            string jsonString = "";
            Debug.Log("Error al leer datos desde PHP: " + www.error);
        }
        else
        {
            // Parser JSON
            string jsonString = www.downloadHandler.text;
            Debug.Log(jsonString);

            // Deserialize JSON to an array
            HeatMapKillData[] dataArray = JsonHelper.FromJson<HeatMapKillData>(jsonString);

            foreach (var data in dataArray)
            {
                HeatMapKillData heatmapKillData = new HeatMapKillData(data.KillID, data.SessionID, data.RunID,
                data.PlayerKiller_PositionX, data.PlayerKiller_PositionY, data.PlayerKiller_PositionZ,
                data.EnemyDeath_PositionX, data.EnemyDeath_PositionY, data.EnemyDeath_PositionZ, data.Time);


                
                killDataList.Add(heatmapKillData);

            }
            // Guardar el JSON en un archivo de texto
            SaveJsonToFile("Data.json", jsonString);

            SaveJsonAsTextFile("Data.txt", jsonString);
        }

        // DEBUG EXAMPLE!
        //Debug.Log("Debug Example 1: " + killDataList[50].KillID + " " + killDataList[50].playerKillerPosition);
        //Debug.Log("Debug Example 2: " + killDataList[57].KillID + " " + killDataList[57].playerKillerPosition);
        //Debug.Log("Debug Example 3: " + killDataList[80].KillID + " " + killDataList[80].playerKillerPosition);
        //Debug.Log("Debug Example 4: " + killDataList[10].KillID + " " + killDataList[10].playerKillerPosition);

    }

    // M�todo para guardar el JSON en un archivo de texto
    private void SaveJsonToFile(string fileName, string json)
    {
        string subfolderPath = "HeatMap/Assets";
        string filePath = Path.Combine(Application.dataPath, subfolderPath, fileName);

        try
        {
            // Escribe el JSON en el archivo
            File.WriteAllText(filePath, json);
            Debug.Log("JSON guardado exitosamente en: " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Error al guardar el JSON en el archivo: " + e.Message);
        }
    }

    private void SaveJsonAsTextFile(string fileName, string json)
    {
        string subfolderPath = "HeatMap/Assets";
        string filePath = Path.Combine(Application.dataPath, subfolderPath, fileName);

        try
        {
            File.WriteAllText(filePath, json);
            Debug.Log("Contenido JSON guardado como archivo de texto en: " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Error al guardar el contenido JSON como archivo de texto: " + e.Message);
        }
    }

    IEnumerator ReadDeathDataFromPHP()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://citmalumnes.upc.es/~davidbo5/Database_DeathReader.php");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error al leer datos desde PHP: " + www.error);
        }
        else
        {
            // Parser JSON
            string jsonString = www.downloadHandler.text;
            Debug.Log(jsonString);

            // Deserialize JSON to an array
            HeatMapDeathData[] dataArray = JsonHelper.FromJson<HeatMapDeathData>(jsonString);

            foreach (var data in dataArray)
            {

                 HeatMapDeathData heatmapDeathData = new HeatMapDeathData(data.DeathID, data.SessionID, data.RunID,
                data.PlayerDeath_PositionX, data.PlayerDeath_PositionY, data.PlayerDeath_PositionZ,
                data.EnemyKiller_PositionX, data.EnemyKiller_PositionY, data.EnemyKiller_PositionZ, data.Time);

                
                deathDataList.Add(heatmapDeathData);

            }

            // DEBUG EXAMPLE!
           // Debug.Log("Debug Example 1: " + deathDataList[50].DeathID + " " + deathDataList[50].playerDeathPosition);
           // Debug.Log("Debug Example 2: " + deathDataList[57].DeathID + " " + deathDataList[57].playerDeathPosition);
           // Debug.Log("Debug Example 3: " + deathDataList[80].DeathID + " " + deathDataList[80].playerDeathPosition);
            //Debug.Log("Debug Example 4: " + deathDataList[10].DeathID + " " + deathDataList[10].playerDeathPosition);
        }
    }

    IEnumerator ReadPathDataFromPHP()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://citmalumnes.upc.es/~davidbo5/Database_PathReader.php");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error al leer datos desde PHP: " + www.error);
        }
        else
        {
            // Parser JSON
            string jsonString = www.downloadHandler.text;
            Debug.Log(jsonString);

            // Deserialize JSON to an array
            PathData[] dataArray = JsonHelper.FromJson<PathData>(jsonString);

            foreach (var data in dataArray)
            {

                PathData pathData = new PathData(data.PathID, data.SessionID, data.RunID,
               data.Player_PositionX, data.Player_PositionY, data.Player_PositionZ,
               data.Player_RotationX, data.Player_RotationY, data.Player_RotationZ, data.Time);


                pathDataList.Add(pathData);

            }

            // DEBUG EXAMPLE!
            // Debug.Log("Debug Example 1: " + pathDataList[50].PathID + " " + pathDataList[50].playerPosition);
            // Debug.Log("Debug Example 2: " + pathDataList[57].PathID + " " + pathDataList[57].playerPosition);
            // Debug.Log("Debug Example 3: " + pathDataList[80].PathID + " " + pathDataList[80].playerPosition);
            //Debug.Log("Debug Example 4: " + pathDataList[10].PathID + " " + pathDataList[10].playerPosition);
        }
    }

    // Definir una clase de utilidad para deserializar arrays JSON
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{\"Items\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.Items;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }

}
