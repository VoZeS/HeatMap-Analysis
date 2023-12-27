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

        damageableScript.OnDeath.AddListener(SendKillData);

        //damageableScript.OnReceiveDamage.AddListener(func);
        //damageableScript.OnHitWhileInvulnerable.AddListener(func);
        //damageableScript.OnBecomeVulnerable.AddListener(func);
        //damageableScript.OnResetDamage.AddListener(func);

       


    }
    private void OnDisable()
    {

        damageableScript.OnDeath.RemoveListener(SendKillData);

        //damageableScript.OnReceiveDamage.RemoveListener(func);
        //damageableScript.OnHitWhileInvulnerable.RemoveListener(func);
        //damageableScript.OnBecomeVulnerable.RemoveListener(func);
        //damageableScript.OnResetDamage.RemoveListener(func);



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
        // ------------------------- WORK DONE BUT NECESSARY??

        StartCoroutine(SendPlayerDeathCoroutine(sessionID, runID, playerPosDeath, enemyPosKill, time));

    }
    private IEnumerator SendPlayerDeathCoroutine(int sessionID, int runID, Transform playerPosDeath, Transform enemyPosKill, DateTime time)
    {
        
        string formatoPersonalizado = "yyyy-MM-dd HH:mm:ss";

        string fechaFormateada = time.ToString(formatoPersonalizado);

        WWWForm formUser = new WWWForm();
        formUser.AddField("SessionID", sessionID);
        formUser.AddField("RunID", runID);
        formUser.AddField("PlayerKiller_PositionX", ((int)playerPosDeath.transform.position.x)); //Revisar si el text entre cometes es correcte pel php
        formUser.AddField("PlayerKiller_PositionY", ((int)playerPosDeath.transform.position.y)); //Revisar si el text entre cometes es correcte pel php
        formUser.AddField("PlayerKiller_PositionZ", ((int)playerPosDeath.transform.position.z)); //Revisar si el text entre cometes es correcte pel php
        formUser.AddField("EnemyDeath_PositionX", ((int)enemyPosKill.transform.position.x));
        formUser.AddField("EnemyDeath_PositionY", ((int)enemyPosKill.transform.position.y));
        formUser.AddField("EnemyDeath_PositionZ", ((int)enemyPosKill.transform.position.z));
        formUser.AddField("Time", fechaFormateada);


        UnityWebRequest www = UnityWebRequest.Post(serverURL, formUser);


        yield return www.SendWebRequest();

        // Verificar si hubo un error en la solicitud
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Datos DE LA DEATH enviados con exito al servidor.");
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
        formUser.AddField("PlayerKiller_PositionX", ((int)playerPos.transform.position.x)); //Revisar si el text entre cometes es correcte pel php
        formUser.AddField("PlayerKiller_PositionY", ((int)playerPos.transform.position.y)); //Revisar si el text entre cometes es correcte pel php
        formUser.AddField("PlayerKiller_PositionZ", ((int)playerPos.transform.position.z)); //Revisar si el text entre cometes es correcte pel php
        formUser.AddField("Time", fechaFormateada);


        UnityWebRequest www = UnityWebRequest.Post(serverURL, formUser);


        yield return www.SendWebRequest();

        // Verificar si hubo un error en la solicitud
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Datos DEL PATH enviados con exito al servidor.");
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


