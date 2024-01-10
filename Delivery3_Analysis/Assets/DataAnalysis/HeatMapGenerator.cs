using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapGenerator : MonoBehaviour
{
    public DatabaseReader databaseReader;
    public GameObject cubePrefab; // Prefab del cubo para representar los puntos de datos
    public float updateInterval = 1f; // Intervalo de actualización en segundos
    public Color killColor = Color.blue; // Color para los cubos de asesinatos
    public Color deathColor = Color.red; // Color para los cubos de muertes
    public Color pathColor = Color.green; // Color para los cubos de muertes
    public Vector3 cubeScale = new Vector3(1f, 1f, 1f); // Tamaño de los cubos
    [SerializeField] bool killHeathMap=false;
    [SerializeField] bool deathHeathMap=false;
    [SerializeField] bool path=false;

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
        List<PathData> pathDataList = databaseReader.pathDataList;


        if (killHeathMap)
        {
            foreach (var killData in killDataList)
            {
                CreateCube(killData.playerKillerPosition, killColor);
            }
        }
        if (deathHeathMap)
        {
            foreach (var deathData in deathDataList)
            {
                CreateCube(deathData.playerDeathPosition, deathColor);
            }
        }
        if (path)
        {
            foreach (var pathData in pathDataList)
            {
                CreateCube(pathData.playerPosition, pathColor);
            }
        }
        

    }

    void CreateCube(Vector3 position, Color color)
    {
        GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
        cube.transform.localScale = cubeScale; // Aplica el tamaño de los cubos
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
