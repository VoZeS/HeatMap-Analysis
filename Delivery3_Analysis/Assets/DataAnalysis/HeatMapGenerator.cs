using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapGenerator : MonoBehaviour
{
    public DatabaseReader databaseReader;
    public GameObject cubePrefab; // Prefab del cubo para representar los puntos de datos
    public float updateInterval = 1f; // Intervalo de actualización en segundos

    void Start()
    {
        // Inicia la actualización del heatmap cada segundo
        InvokeRepeating("GenerateHeatmap", 0f, updateInterval);
    }

    void GenerateHeatmap()
    {
        if (databaseReader == null)
        {
            Debug.LogError("DatabaseReader no asignado en el inspector.");
            return;
        }

        // Elimina los cubos antiguos antes de generar el nuevo heatmap
        ClearHeatmap();

        List<HeatMapKillData> killDataList = databaseReader.killDataList;
        List<HeatMapDeathData> deathDataList = databaseReader.deathDataList;

        foreach (var killData in killDataList)
        {
            CreateCube(killData.playerKillerPosition, Color.blue);
        }

        foreach (var deathData in deathDataList)
        {
            CreateCube(deathData.playerDeathPosition, Color.red);
        }
    }

    void CreateCube(Vector3 position, Color color)
    {
        GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
        cube.GetComponent<Renderer>().material.color = color;
    }

    void ClearHeatmap()
    {
        // Destruye todos los cubos en la escena antes de la nueva generación
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("HeatmapCube");
        foreach (GameObject cube in cubes)
        {
            Destroy(cube);
        }
    }
}

