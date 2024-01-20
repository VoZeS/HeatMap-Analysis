using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapGenerator : MonoBehaviour
{
    public DatabaseReader databaseReader;
    public GameObject cubePrefab; // Prefab del cubo para representar los puntos de datos
    public GameObject arrowPrefab; // Prefab del cubo para representar los puntos de datos
    public float updateInterval = 1f; // Intervalo de actualización en segundos
    public Color killColor = Color.blue; // Color para los cubos de asesinatos
    public Color deathColor = Color.red; // Color para los cubos de muertes
    public Color pathColor = Color.green; // Color para los cubos de path
    public Vector3 cubeScale = new Vector3(1f, 1f, 1f); // Tamaño de los cubos
    public Vector3 arrowScale = new Vector3(0.5f, 0.5f, 0.5f); // Tamaño de los cubos
    [SerializeField] bool killHeathMap = false;
    [SerializeField] bool deathHeathMap = false;
    [SerializeField] bool path = false;

    public int gridSize = 20; // Número de subdivisiones en la cuadrícula
    public float cellSize = 10.0f; // Tamaño de cada celda en la cuadrícula

    float centerX;
    float centerZ;

    int[,] grid ;


    void Start()
    {
        // Inicia la actualización del heatmap cada segundo
        InvokeRepeating("GenerateHeatmap", 0f, updateInterval);
    }

    void OnDrawGizmos()
    {
        DrawGrid();
    }

    void DrawGrid()
    {
        grid = new int[gridSize, gridSize];

        Gizmos.color = Color.white;

        // Calcula la posición central del grid
        centerX = gridSize * cellSize / 2.0f;
        centerZ = gridSize * cellSize / 2.0f;

        // Dibuja las líneas verticales de la cuadrícula
        for (float i = 0; i <= gridSize; i++)
        {
            float x = i * cellSize - centerX;
            Gizmos.DrawLine(new Vector3(x, 0, -centerZ), new Vector3(x, 0, centerZ));
        }

        // Dibuja las líneas horizontales de la cuadrícula
        for (float i = 0; i <= gridSize; i++)
        {
            float z = i * cellSize - centerZ;
            Gizmos.DrawLine(new Vector3(-centerX, 0, z), new Vector3(centerX, 0, z));
        }
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


        List<Vector2> createdKillCubePositions = new List<Vector2>();




        if (killHeathMap)
        {

        foreach (var killData in killDataList)
        {
            Vector3 gridPosition = GetGridPosition(killData.playerKillerPosition);

            // Obtén una versión 2D de la posición (ignorando la coordenada y)
            Vector2 gridPosition2D = new Vector2(gridPosition.x, gridPosition.z);
            
            if (gridPosition != Vector3.zero && !createdKillCubePositions.Contains(gridPosition2D))
            {
                CreateCube(gridPosition, killColor, "KillCube");
                createdKillCubePositions.Add(gridPosition2D);
            }
        }

        }

        List<Vector2> createdDeathCubePositions = new List<Vector2>();

        if (deathHeathMap)
        {

        foreach (var deathData in deathDataList)
        {
            Vector3 gridPosition = GetGridPosition(deathData.playerDeathPosition);

            // Obtén una versión 2D de la posición (ignorando la coordenada y)
            Vector2 gridPosition2D = new Vector2(gridPosition.x, gridPosition.z);
            
            if (gridPosition != Vector3.zero && !createdDeathCubePositions.Contains(gridPosition2D))
            {
                CreateCube(gridPosition, deathColor, "DeathCube");
                createdDeathCubePositions.Add(gridPosition2D);
            }
        }


            /*for (var i = 0; i < gridSize; i++) {

                for (var j = 0; j < gridSize; j++)
                {
                   // CreateCube(gridPosition, deathColor, "DeathCube");
                }
               
            }*/

        }

        //Create the path
        if (path)
        {
            foreach (var pathData in pathDataList)
            {
                
                
                    CreatePath(pathData.playerPosition, pathData.playerRotation,pathColor, "PathCube");
                
            }
        }
    }

    Vector3 GetGridPosition(Vector3 originalPosition)
    {
        // Calcula las coordenadas de la cuadrícula para la posición dada
        float xRatio = (originalPosition.x - centerX) / (gridSize * cellSize);//50/10->5
        float zRatio = (originalPosition.z - centerZ) / (gridSize * cellSize);

        // Verifica si la posición está dentro de la cuadrícula
        if (xRatio >= -1f && xRatio < 1f && zRatio >= -1f && zRatio < 1f)
        {
            int gridX = Mathf.FloorToInt(xRatio * gridSize);
            int gridZ = Mathf.FloorToInt(zRatio * gridSize);

            // Calcula la posición central de la celda de la cuadrícula
            float cellCenterX = centerX + (gridX + 0.5f) * (cellSize / 1f);
            float cellCenterZ = centerZ + (gridZ + 0.5f) * (cellSize / 1f);

            return new Vector3(cellCenterX, originalPosition.y, cellCenterZ);
        }

        return Vector3.zero; // Devuelve Vector3.zero si la posición está fuera de la cuadrícula
    }

    void CreateCube(Vector3 position, Color color, string cubeName)
    {
        GameObject cube = Instantiate(cubePrefab, new Vector3(position.x,position.y+1,position.z), Quaternion.identity);
        cube.transform.localScale = new Vector3(cellSize / 1f, cubeScale.y, cellSize / 1f); // Ajusta el tamaño del cubo a la celda; // Aplica el tamaño de los cubos
        cube.GetComponent<Renderer>().material.color = color;

        // Agrega un identificador único para cada celda en el nombre del cubo
        //cube.name = cubeName + "_" + position.x + "_" + position.z;
    }
    void CreatePath(Vector3 position, Vector3 rotation,Color color, string cubeName)
    {
        Vector3 rotationVector = rotation; // Cambia estos valores según tus necesidades

        Quaternion rotationQuaternion = Quaternion.Euler(rotationVector);

        GameObject cube = Instantiate(arrowPrefab, position, rotationQuaternion);
        cube.transform.localScale = arrowScale; // Ajusta el tamaño del cubo a la celda; // Aplica el tamaño de los cubos
        cube.GetComponent<Renderer>().material.color = color;

        // Agrega un identificador único para cada celda en el nombre del cubo
        //cube.name = cubeName + "_" + position.x + "_" + position.z;
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

