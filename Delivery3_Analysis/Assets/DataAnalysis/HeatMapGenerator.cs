using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapGenerator : MonoBehaviour
{
    public DatabaseReader databaseReader;
    public GameObject cubePrefab; // Prefab del cubo para representar los puntos de datos
    public float updateInterval = 1f; // Intervalo de actualización en segundos
    public Color killColor = new Color(0f, 0f, 1f); // Color para los cubos de asesinatos (azul por defecto)
    public Color deathColor = new Color(1f, 0f, 0f); // Color para los cubos de muertes (rojo por defecto)
    public Vector3 cubeScale = new Vector3(1f, 1f, 1f); // Tamaño de los cubos
    public float cubeAlpha = 1f; // Transparencia de los cubos (0 completamente transparente, 1 completamente opaco)
    public float searchRadius = 2f; // Radio de búsqueda para determinar la densidad de puntos

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
            Color adjustedColor = GetAdjustedColor(killData.playerKillerPosition, killColor);
            CreateCube(killData.playerKillerPosition, adjustedColor);
        }

        foreach (var deathData in deathDataList)
        {
            Color adjustedColor = GetAdjustedColor(deathData.playerDeathPosition, deathColor);
            CreateCube(deathData.playerDeathPosition, adjustedColor);
        }
    }

    void CreateCube(Vector3 position, Color color)
    {
        GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
        cube.transform.localScale = cubeScale; // Aplica el tamaño de los cubos
        color.a = cubeAlpha; // Asigna la transparencia al color
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

    Color GetAdjustedColor(Vector3 position, Color baseColor)
    {
        Collider[] colliders = Physics.OverlapSphere(position, searchRadius);

        int sameTypeCount = 0;

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("HeatmapCube") && collider.GetComponent<Renderer>().material.color == baseColor)
            {
                
            }
        }

        float intensityMultiplier = 1f + (sameTypeCount * 0.1f); // Ajusta según tu preferencia

        return baseColor * intensityMultiplier;
    }
}
