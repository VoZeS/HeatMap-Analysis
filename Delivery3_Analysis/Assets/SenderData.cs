using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class SenderData : MonoBehaviour
{
    public string serverURL = "https://citmalumnes.upc.es/~davidbo5/ServerPhP.php"; // Reemplaza con la URL de tu servidor
    //private bool startingSessionBool = true;
    //private string startSessionTime;
    //private string endSessionTime;

    // Crear un formulario para los datos
    WWWForm formSessions;
    WWWForm formEndSessions;

    // IDs
    uint userId_uInt;
    uint sessionId_uInt;

    private void OnEnable()
    {
        //Simulator.OnNewPlayer += SendData;
        //Simulator.OnNewSession += SendNewSessionTime;
        //Simulator.OnEndSession += SendEndSessionTime;
        //Simulator.OnBuyItem += SendbuyInfo;

    }
    private void OnDisable()
    {
        //Simulator.OnNewPlayer -= SendData;
        //Simulator.OnNewSession -= SendNewSessionTime;
        //Simulator.OnEndSession -= SendEndSessionTime;
        //Simulator.OnBuyItem -= SendbuyInfo;

    }

    // -------------------------------------------------------------------------------------------------------------------- Send Player Data & Get ID
    public void SendData(Transform playerPosDead, Transform enemyPosDead, /*Path,*/ Transform playerPosKill, float timeAlive,int timesAttacked)
    {

        //StartCoroutine(SendUserDataCoroutine());

    }

    private IEnumerator SendPlayerKillCoroutine(Transform enemyPosDead, Transform playerPosKill,int runID, DateTime date,int SessionID)
    {
        // Define un formato de fecha personalizado
        string formatoPersonalizado = "yyyy-MM-dd HH:mm:ss";

        // Convierte la fecha en una cadena con el formato personalizado
        string fechaFormateada = date.ToString(formatoPersonalizado);

        // Crear un formulario para los datos
       /* WWWForm formUser = new WWWForm();
        formUser.AddField("Name", name);
        formUser.AddField("Age", age);
        formUser.AddField("Gender", gender);
        formUser.AddField("Country", country);
        formUser.AddField("Date", fechaFormateada);*/

        // Crear una solicitud POST con el formulario
       // UnityWebRequest www = UnityWebRequest.Post(serverURL, formUser);


        // Enviar la solicitud al servidor
       // yield return www.SendWebRequest();

        // Verificar si hubo un error en la solicitud
      /*  if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Datos DEL USER enviados con exito al servidor.");
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
                Debug.Log("Error USERID");
                Debug.Log(www.downloadHandler.text);
            }

        }
        else
        {
            Debug.LogError("Error al enviar datos DEL USER al servidor: " + www.error);
        }*/
    }
    //Sistema de herencias da datos
    private IEnumerator SendPlayerDeadCoroutine(Transform playerPosDead, Transform path, Vector3 angulosEuler, DateTime Date)
    {

    }

    // --------------------------------------------------------------------------------------------------------------------
    // -------------------------------------------------------------------------------------------------------------------- Send StartSessionTime
    public void SendNewSessionTime(DateTime sessionTime)
    {
        StartCoroutine(SendNewSessionTimeStamp(sessionTime));
    }

    private IEnumerator SendNewSessionTimeStamp(DateTime sessionTime)
    {
        // Define un formato de fecha personalizado
        string formatoPersonalizado = "yyyy-MM-dd HH:mm:ss";

        // Convierte la fecha en una cadena con el formato personalizado
        string fechaFormateada = sessionTime.ToString(formatoPersonalizado);

        formSessions = new WWWForm();
        formSessions.AddField("startSessionTime", fechaFormateada);
        formSessions.AddField("userID", (int)userId_uInt);

        // Crear una solicitud POST con el formulario
        UnityWebRequest www = UnityWebRequest.Post(serverURL, formSessions);

        // Enviar la solicitud al servidor
        yield return www.SendWebRequest();

        // Verificar si hubo un error en la solicitud
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Datos DEL INICIO DE LA SESION enviados con exito al servidor.");

            string sessionId_String = www.downloadHandler.text;

            if (uint.TryParse(sessionId_String, out sessionId_uInt))
            {
                // La conversión fue exitosa, y valorComoInt contiene el valor entero.
                //CallbackEvents.OnNewSessionCallback?.Invoke(sessionId_uInt);
                Debug.Log(www.downloadHandler.text);

            }
            else
            {
                // La conversión falló, puedes manejar el error aquí.
                Debug.Log(www.downloadHandler.text);
            }

        }
        else
        {
            Debug.LogError("Error al enviar datos DEL INICIO DE LA SESION al servidor: " + www.error);

        }

    }

    public void SendEndSessionTime(DateTime sessionTime)
    {
        StartCoroutine(SendEndSessionTimeStamp(sessionTime));
    }

    private IEnumerator SendEndSessionTimeStamp(DateTime sessionTime)
    {
        // Define un formato de fecha personalizado
        string formatoPersonalizado = "yyyy-MM-dd HH:mm:ss";

        // Convierte la fecha en una cadena con el formato personalizado
        string fechaFormateada = sessionTime.ToString(formatoPersonalizado);

        formEndSessions = new WWWForm();
        formEndSessions.AddField("endSessionTime", fechaFormateada);
        formEndSessions.AddField("sessionID", (int)sessionId_uInt);

        // Crear una solicitud POST con el formulario
        UnityWebRequest www = UnityWebRequest.Post(serverURL, formEndSessions);

        // Enviar la solicitud al servidor
        yield return www.SendWebRequest();

        // Verificar si hubo un error en la solicitud
        if (www.result == UnityWebRequest.Result.Success)
        {

            Debug.Log("Datos DEL FIN DE LA SESSION enviados con exito al servidor.");

            //CallbackEvents.OnEndSessionCallback?.Invoke(sessionId_uInt);
            Debug.Log(www.downloadHandler.text);

        }
        else
        {
            Debug.LogError("Error al enviar datos DEL FIN DE LA SESION al servidor: " + www.error);

        }

    }
    // --------------------------------------------------------------------------------------------------------------------
    // -------------------------------------------------------------------------------------------------------------------- Buying items

    public void SendbuyInfo(int itemID, DateTime dateBuy)
    {
        StartCoroutine(SendBuyingData(itemID, dateBuy));
    }

    private IEnumerator SendBuyingData(int itemID, DateTime dateBuy)
    {
        // Define un formato de fecha personalizado
        string formatoPersonalizado = "yyyy-MM-dd HH:mm:ss";

        // Convierte la fecha en una cadena con el formato personalizado
        string fechaFormateada = dateBuy.ToString(formatoPersonalizado);


        // Crear un formulario para los datos
        WWWForm formShop = new WWWForm();

        //La tabla requiere de los siguientes campos: ShoppingID -> (itemID??) , User-ID, Session-ID, Date, MoneySpent 

        float itemPrice = 0;

        switch (itemID)
        {
            case 1:
                itemPrice = 0.99f;
                break;
            case 2:
                itemPrice = 1.99f;
                break;
            case 3:
                itemPrice = 9.99f;
                break;
            case 4:
                itemPrice = 49.99f;
                break;
            case 5:
                itemPrice = 99.99f;
                break;

            default: break;

        }

        formShop.AddField("moneySpent", itemPrice.ToString());
        formShop.AddField("buyTime", fechaFormateada);
        formShop.AddField("userID", (int)userId_uInt);
        formShop.AddField("sessionID", (int)sessionId_uInt);

        // Crear una solicitud POST con el formulario
        UnityWebRequest www = UnityWebRequest.Post(serverURL, formShop);

        // Enviar la solicitud al servidor
        yield return www.SendWebRequest();

        // Verificar si hubo un error en la solicitud
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Datos DE LA COMPRA enviados con exito al servidor.");

            //CallbackEvents.OnItemBuyCallback?.Invoke();
            Debug.Log(www.downloadHandler.text);

        }
        else
        {
            Debug.LogError("Error al enviar datos DE LA COMPRA al servidor: " + www.error);
        }


    }



}


