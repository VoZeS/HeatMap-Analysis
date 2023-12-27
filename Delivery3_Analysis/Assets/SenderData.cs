using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class SenderData : MonoBehaviour
{
    // FER-HO MES OPTIM!!! ------ Sistema de herencias da datos --------

    public string serverURL = "https://citmalumnes.upc.es/~davidbo5/ServerPhP.php"; // Server URL
    //private bool startingSessionBool = true;

    // Crear un formulario para los datos
    WWWForm formSessions;
    WWWForm formEndSessions;

    // IDs
    uint userId_uInt;
    uint sessionId_uInt;

    private void OnEnable()
    {
        // ------------------------- WORK IN PROGRESS

        //damageableScript.OnDeath += SendKillData;
        //Simulator.OnNewSession += SendNewSessionTime;
        //Simulator.OnEndSession += SendEndSessionTime;
        //Simulator.OnBuyItem += SendbuyInfo;

    }
    private void OnDisable()
    {
        // ------------------------- WORK IN PROGRESS

        //Simulator.OnNewPlayer -= SendData;
        //Simulator.OnNewSession -= SendNewSessionTime;
        //Simulator.OnEndSession -= SendEndSessionTime;
        //Simulator.OnBuyItem -= SendbuyInfo;

    }

    // -------------------------------------------------------------------------------------------------------------------- SEND HEATMAP KILL DATA
    public void SendKillData(int sessionID, int runID, Transform playerPosKill, Transform enemyPosDeath, DateTime time)
    {
        // ------------------------- WORK IN PROGRESS

        StartCoroutine(SendPlayerKillCoroutine(sessionID, runID, playerPosKill, enemyPosDeath, time));

    }

    // ------------------------- WORK IN PROGRESS

    private IEnumerator SendPlayerKillCoroutine(int sessionID, int runID, Transform enemyPosDeath, Transform playerPosKill, DateTime time)
    {
        // Define un formato de fecha personalizado
        string formatoPersonalizado = "yyyy-MM-dd HH:mm:ss";

        // Convierte la fecha en una cadena con el formato personalizado
        string fechaFormateada = time.ToString(formatoPersonalizado);

        // Crear un formulario para los datos
        WWWForm formUser = new WWWForm();
        formUser.AddField("SessionID", sessionID);
        formUser.AddField("RunID", runID);
        formUser.AddField("PlayerKiller_PositionX", ((int)playerPosKill.transform.position.x));
        formUser.AddField("PlayerKiller_PositionY", ((int)playerPosKill.transform.position.y));
        formUser.AddField("PlayerKiller_PositionZ", ((int)playerPosKill.transform.position.z));
        formUser.AddField("EnemyDeath_PositionX", ((int)enemyPosDeath.transform.position.x));
        formUser.AddField("EnemyDeath_PositionY", ((int)enemyPosDeath.transform.position.y));
        formUser.AddField("EnemyDeath_PositionZ", ((int)enemyPosDeath.transform.position.z));
        formUser.AddField("Time", fechaFormateada);

        // Crear una solicitud POST con el formulario
        UnityWebRequest www = UnityWebRequest.Post(serverURL, formUser);


        // Enviar la solicitud al servidor
        yield return www.SendWebRequest();

        // Verificar si hubo un error en la solicitud
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Datos DE LA KILL enviados con exito al servidor.");
            Debug.Log(www.downloadHandler.text);

            string userId_String = www.downloadHandler.text;

            if (uint.TryParse(userId_String, out userId_uInt))
            {
                // La conversión fue exitosa, y valorComoInt contiene el valor entero.
                //CallbackEvents.OnAddPlayerCallback?.Invoke(userId_uInt);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                // La conversión falló, puedes manejar el error aquí.
                Debug.Log("Error KILLID");
                Debug.Log(www.downloadHandler.text);
            }

        }
        else
        {
            Debug.LogError("Error al enviar datos DE LA KILL al servidor: " + www.error);
        }
    }

        // -------------------------------------------------------------------------------------------------------------------- SEND HEATMAP DEATH DATA
        public void SendDeathData(int sessionID, int runID, Transform playerPosDeath, Transform enemyPosKill, DateTime time)
    {
        // ------------------------- WORK IN PROGRESS

        //StartCoroutine(SendPlayerDeathCoroutine(sessionID, runID, playerPosDeath, enemyPosKill, time));

    }
    //private IEnumerator SendPlayerDeathCoroutine(int sessionID, int runID, Transform playerPosDeath, Transform enemyPosKill, DateTime time)
    //{

    //}

    // --------------------------------------------------------------------------------------------------------------------

    // -------------------------------------------------------------------------------------------------------------------- SEND HEATMAP PATH
    public void SendPathData(int sessionID, int runID, Transform playerPos, DateTime time)
    {
        // ------------------------- WORK IN PROGRESS

        //StartCoroutine(SendPlayerDeathCoroutine(sessionID, runID, playerPos, time));

    }
    //private IEnumerator SendPlayerDeathCoroutine(int sessionID, int runID, Transform playerPos, DateTime time)
    //{

    //}
}


