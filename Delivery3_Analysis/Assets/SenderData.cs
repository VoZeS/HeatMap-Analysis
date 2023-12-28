using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class SenderData : MonoBehaviour
{
    public Gamekit3D.Damageable damageablePlayerScript;

    // FER-HO MES OPTIM!!! ------ Sistema de herencias da datos --------

    public string serverURL = "https://citmalumnes.upc.es/~davidbo5/ServerHeatmapPhP.php"; // Server URL
    //private bool startingSessionBool = true;

    // Crear un formulario para los datos
    WWWForm formSessions;
    WWWForm formEndSessions;

    // IDs
    int run_id = 0;
    int session_id = 0;

    uint killId_uint;
    uint deathId_uint;
    uint pathId_uint;

    private void OnEnable()
    {
        session_id++;

        damageablePlayerScript.OnDeath.AddListener(SendKillData);

        //damageableScript.OnReceiveDamage.AddListener(func);
        //damageableScript.OnHitWhileInvulnerable.AddListener(func);
        //damageableScript.OnBecomeVulnerable.AddListener(func);
        //damageableScript.OnResetDamage.AddListener(func);

       


    }
    private void OnDisable()
    {

        damageablePlayerScript.OnDeath.RemoveListener(SendKillData);

        //damageableScript.OnReceiveDamage.RemoveListener(func);
        //damageableScript.OnHitWhileInvulnerable.RemoveListener(func);
        //damageableScript.OnBecomeVulnerable.RemoveListener(func);
        //damageableScript.OnResetDamage.RemoveListener(func);



    }

    // -------------------------------------------------------------------------------------------------------------------- SEND HEATMAP KILL DATA
    public void SendKillData()
    {
        // ------------------------- WORK IN PROGRESS
        int sessionID = session_id;
        int runID = run_id;
        Vector3 playerPosKill = GameObject.Find("Ellen").transform.position; // POSITION PLAYER 
        Vector3 enemyPosDeath = damageablePlayerScript.onDamageMessageReceivers[0].transform.position; // RECEIVER DAMAGE (enemy)
        DateTime time = DateTime.Now;

        StartCoroutine(SendPlayerKillCoroutine(sessionID, runID, playerPosKill, enemyPosDeath, time));

    }

    // ------------------------- WORK IN PROGRESS

    private IEnumerator SendPlayerKillCoroutine(int sessionID, int runID, Vector3 enemyPosDeath, Vector3 playerPosKill, DateTime time)
    {
        // Define un formato de fecha personalizado
        string formatoPersonalizado = "yyyy-MM-dd HH:mm:ss";

        // Convierte la fecha en una cadena con el formato personalizado
        string fechaFormateada = time.ToString(formatoPersonalizado);

        // Crear un formulario para los datos
        WWWForm formUser = new WWWForm();
        formUser.AddField("SessionID", sessionID);
        formUser.AddField("RunID", runID);
        formUser.AddField("PlayerKiller_PositionX", ((int)playerPosKill.x));
        formUser.AddField("PlayerKiller_PositionY", ((int)playerPosKill.y));
        formUser.AddField("PlayerKiller_PositionZ", ((int)playerPosKill.z));
        formUser.AddField("EnemyDeath_PositionX", ((int)enemyPosDeath.x));
        formUser.AddField("EnemyDeath_PositionY", ((int)enemyPosDeath.y));
        formUser.AddField("EnemyDeath_PositionZ", ((int)enemyPosDeath.z));
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

            if (uint.TryParse(userId_String, out killId_uint))
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
        public void SendDeathData()
    {
        // ------------------------- WORK DONE
        run_id++;

        // ------------------------- WORK IN PROGRESS
        int sessionID = session_id;
        int runID = run_id;
        Vector3 playerPosDeath = GameObject.Find("Ellen").transform.position; // POSITION PLAYER 
        Vector3 enemyPosKill = damageablePlayerScript.onDamageMessageReceivers[0].transform.position; // RECEIVER DAMAGE (enemy)
        DateTime time = DateTime.Now;

        StartCoroutine(SendPlayerDeathCoroutine(sessionID, runID, playerPosDeath, enemyPosKill, time));

    }
    private IEnumerator SendPlayerDeathCoroutine(int sessionID, int runID, Vector3 playerPosDeath, Vector3 enemyPosKill, DateTime time)
    {
        
        string formatoPersonalizado = "yyyy-MM-dd HH:mm:ss";

        string fechaFormateada = time.ToString(formatoPersonalizado);

        WWWForm formUser = new WWWForm();
        formUser.AddField("SessionID", sessionID);
        formUser.AddField("RunID", runID);
        formUser.AddField("PlayerDeath_PositionX", ((int)playerPosDeath.x)); 
        formUser.AddField("PlayerDeath_PositionY", ((int)playerPosDeath.y)); 
        formUser.AddField("PlayerDeath_PositionZ", ((int)playerPosDeath.z)); 
        formUser.AddField("EnemyKiller_PositionX", ((int)enemyPosKill.x));
        formUser.AddField("EnemyKiller_PositionY", ((int)enemyPosKill.y));
        formUser.AddField("EnemyKiller_PositionZ", ((int)enemyPosKill.z));
        formUser.AddField("Time", fechaFormateada);


        UnityWebRequest www = UnityWebRequest.Post(serverURL, formUser);


        yield return www.SendWebRequest();

        // Verificar si hubo un error en la solicitud
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Datos DE LA DEATH enviados con exito al servidor.");
            Debug.Log(www.downloadHandler.text);

            string userId_String = www.downloadHandler.text;

            if (uint.TryParse(userId_String, out deathId_uint))
            {
                // La conversión fue exitosa, y valorComoInt contiene el valor entero.
                //CallbackEvents.OnAddPlayerCallback?.Invoke(userId_uInt);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                // La conversión falló, puedes manejar el error aquí.
                Debug.Log("Error DEATHID");
                Debug.Log(www.downloadHandler.text);
            }

        }
        else
        {
            Debug.LogError("Error al enviar datos DE LA DEATH al servidor: " + www.error);
        }




    }

    // --------------------------------------------------------------------------------------------------------------------

    // -------------------------------------------------------------------------------------------------------------------- SEND HEATMAP PATH
    public void SendPathData(int sessionID, int runID, Transform playerPos, DateTime time)
    {
        // ------------------------- WORK DONE BUT NECESSARY??

        StartCoroutine(SendPlayerDeathCoroutine(sessionID, runID, playerPos, time));

    }
    private IEnumerator SendPlayerDeathCoroutine(int sessionID, int runID, Transform playerPos, DateTime time)
    {
        string formatoPersonalizado = "yyyy-MM-dd HH:mm:ss";

        string fechaFormateada = time.ToString(formatoPersonalizado);

        WWWForm formUser = new WWWForm();
        formUser.AddField("SessionID", sessionID);
        formUser.AddField("RunID", runID);
        formUser.AddField("Player_PositionX", ((int)playerPos.transform.position.x)); 
        formUser.AddField("Player_PositionY", ((int)playerPos.transform.position.y)); 
        formUser.AddField("Player_PositionZ", ((int)playerPos.transform.position.z)); 
        formUser.AddField("Time", fechaFormateada);


        UnityWebRequest www = UnityWebRequest.Post(serverURL, formUser);


        yield return www.SendWebRequest();

        // Verificar si hubo un error en la solicitud
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Datos DEL PATH enviados con exito al servidor.");
            Debug.Log(www.downloadHandler.text);

            string userId_String = www.downloadHandler.text;

            if (uint.TryParse(userId_String, out pathId_uint))
            {
                // La conversión fue exitosa, y valorComoInt contiene el valor entero.
                //CallbackEvents.OnAddPlayerCallback?.Invoke(userId_uInt);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                // La conversión falló, puedes manejar el error aquí.
                Debug.Log("Error PATHID");
                Debug.Log(www.downloadHandler.text);
            }

        }
        else
        {
            Debug.LogError("Error al enviar datos DEL PATH al servidor: " + www.error);
        }



    }
}


