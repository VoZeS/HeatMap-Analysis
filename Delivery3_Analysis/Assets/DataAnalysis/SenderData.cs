using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UIElements;

namespace Gamekit3D
{
    public class SenderData : MonoBehaviour
    {
        public Damageable damageablePlayerScript;

        public Damageable[] damageableEnemiesScritps;

        // FER-HO MES OPTIM!!! ------ Sistema de herencias da datos --------

        public string serverURL = "https://citmalumnes.upc.es/~davidbo5/ServerHeatmapPhP.php"; // Server URL
                                                                                               //private bool startingSessionBool = true;

        // Crear un formulario para los datos
        WWWForm formSessions;
        WWWForm formEndSessions;

        // IDs
        int run_id = 1;

        private const string SessionIDKey = "session_id";
        int session_id = 0;

        uint killId_uint;
        uint deathId_uint;
        uint pathId_uint;

        private void Start()
        {
            // Load Session ID
            LoadSessionID();

            session_id++;

            // Save new Session ID
            SaveSessionID();

            Debug.Log("Session ID: " + session_id);


        }

        private void LoadSessionID()
        {
            if (PlayerPrefs.HasKey(SessionIDKey))
            {
                session_id = PlayerPrefs.GetInt(SessionIDKey);
            }
        }

        private void SaveSessionID()
        {
            PlayerPrefs.SetInt(SessionIDKey, session_id);
            PlayerPrefs.Save();
        }

        private void OnEnable()
        {
            // Encuentra todos los objetos del tipo Damageable en la escena
            Damageable[] allDamageableObjects = GameObject.FindObjectsOfType<Damageable>();

            // Filtra los objetos por nombre
            damageableEnemiesScritps = new Damageable[allDamageableObjects.Length];
            int i = 0;

            foreach (Damageable damageableObj in allDamageableObjects)
            {
                if (damageableObj.gameObject.name == "Chomper" || damageableObj.gameObject.name == "Spitter" || damageableObj.gameObject.name == "Grenadier")
                {
                    damageableEnemiesScritps[i] = damageableObj;
                    damageableEnemiesScritps[i].OnDeath.AddListener(SendKillData);
                    damageableEnemiesScritps[i].OnDeath.AddListener(SendPathData); //Provisional
                    i++;
                }
            }

            damageablePlayerScript.OnDeath.AddListener(SendDeathData);

            //Path sending provisional
            {
                damageablePlayerScript.OnDeath.AddListener(SendPathData);
                damageablePlayerScript.OnReceiveDamage.AddListener(SendPathData);
                damageablePlayerScript.OnHitWhileInvulnerable.AddListener(SendPathData);
                damageablePlayerScript.OnBecomeVulnerable.AddListener(SendPathData);
                damageablePlayerScript.OnResetDamage.AddListener(SendPathData);

            }



        }
        private void OnDisable()
        {

            for (int i = 0; i < damageableEnemiesScritps.Length; i++)
            {
                if (damageableEnemiesScritps[i] != null)
                {
                    damageableEnemiesScritps[i].OnDeath.RemoveListener(SendKillData);
                    damageableEnemiesScritps[i].OnDeath.RemoveListener(SendPathData);// Provisional
                }

            }

            damageablePlayerScript.OnDeath.RemoveListener(SendDeathData);


            damageablePlayerScript.OnDeath.RemoveListener(SendPathData);
            damageablePlayerScript.OnReceiveDamage.RemoveListener(SendPathData);
            damageablePlayerScript.OnHitWhileInvulnerable.RemoveListener(SendPathData);
            damageablePlayerScript.OnBecomeVulnerable.RemoveListener(SendPathData);
            damageablePlayerScript.OnResetDamage.RemoveListener(SendPathData);





        }

        // -------------------------------------------------------------------------------------------------------------------- SEND HEATMAP KILL DATA
        public void SendKillData()
        {
            // ------------------------- WORK IN PROGRESS
            int sessionID = session_id;
            int runID = run_id;
            Vector3 playerPosKill = GameObject.Find("Ellen").transform.position; // POSITION PLAYER 
            Vector3 enemyPosDeath = new Vector3(-100, -100, -100);

            for (int i = 0; i < damageableEnemiesScritps.Length; i++)
            {
                if (damageableEnemiesScritps[i] != null && damageableEnemiesScritps[i].currentHitPoints <= 0)
                {
                    enemyPosDeath = damageableEnemiesScritps[i].transform.position; // RECEIVER DAMAGE (enemy)

                }
            }
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
            int sessionID = session_id;
            int runID = run_id;
            Vector3 playerPosDeath = GameObject.Find("Ellen").transform.position; // POSITION PLAYER 
            Vector3 enemyPosKill = playerPosDeath + damageablePlayerScript.positionToDamager;
            DateTime time = DateTime.Now;

            run_id++;

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
        public void SendPathData()
        {
            // ------------------------- WORK DONE BUT NECESSARY??
            int sessionID = session_id;
            int runID = run_id;
            Vector3 playerPos = GameObject.Find("Ellen").transform.position; // POSITION PLAYER 
            Vector3 playerRot = GameObject.Find("Ellen").transform.rotation.eulerAngles;
            DateTime time = DateTime.Now;

            StartCoroutine(SendPathCoroutine(sessionID, runID, playerPos, playerRot, time));

        }
        private IEnumerator SendPathCoroutine(int sessionID, int runID, Vector3 playerPos, Vector3 playerRot, DateTime time)
        {
            string formatoPersonalizado = "yyyy-MM-dd HH:mm:ss";

            string fechaFormateada = time.ToString(formatoPersonalizado);

            WWWForm formUser = new WWWForm();
            formUser.AddField("SessionID", sessionID);
            formUser.AddField("RunID", runID);
            formUser.AddField("Player_PositionX", ((int)playerPos.x));
            formUser.AddField("Player_PositionY", ((int)playerPos.y));
            formUser.AddField("Player_PositionZ", ((int)playerPos.z));
            formUser.AddField("Player_RotationX", ((int)playerRot.x));
            formUser.AddField("Player_RotationY", ((int)playerRot.y));
            formUser.AddField("Player_RotationZ", ((int)playerRot.z));
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
                    Debug.Log("Error PATHSTEPID");
                    Debug.Log(www.downloadHandler.text);
                }

            }
            else
            {
                Debug.LogError("Error al enviar datos DEL PATH al servidor: " + www.error);
            }



        }
    }
}


