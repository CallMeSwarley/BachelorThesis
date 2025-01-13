using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class ServerCommunicator : MonoBehaviour
{
    private static bool receivedGLTF = false;
    private static bool receivedBIN = false;
    static string serverHost = "192.168.154.65";//localhost
    public static IEnumerator uploadGLTF(string filePath, string savePath)
    {
        receivedGLTF = false;
        // Erstelle eine UnityWebRequest-Instanz
        UnityWebRequest request = UnityWebRequest.Post("http://" + serverHost + ":8000/uploadGLTF", "");
        Debug.Log("GLTF Upload läuft");
        // Lese den Dateiinhalt in ein Byte[] Array ein
        byte[] fileContent = File.ReadAllBytes(filePath);
        // Erstelle eine UploadHandlerRaw-Instanz mit dem Array
        request.uploadHandler = new UploadHandlerRaw(fileContent);
        // Setze den Content-Type Header
        request.SetRequestHeader("Content-Type", "application/gltf+json");

        // Sende den Request an den Server 
        request.SendWebRequest();

        // Warte, bis der Request abgeschlossen ist
        while (!request.isDone)
        {
            yield return null;
        }

        // überprfe, ob der Request erfolgreich war
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Irgendetwas ist schief gelaufen");
            Debug.LogError(request.error);
            yield break;
        }
        else
        {
            // Lese die Antwort des Servers in ein Byte[] Array ein
            byte[] responseData = request.downloadHandler.data;
            // Schreibe das Array in eine Datei
            File.WriteAllBytes(savePath, responseData);
            Debug.Log($"Die Antwort wurde in {savePath} gespeichert.");
            receivedGLTF = true;
        }
    }
    //
    public static IEnumerator uploadBIN(string filePath, string savePath, string gltfPath, Stopwatch stopwatch, PopupMessage popupMessage, GameObject loadingScreen)
    {
        receivedBIN = false;
        // Erstelle eine UnityWebRequest-Instanz
        var request = UnityWebRequest.Post("http://" + serverHost + ":8000/uploadBIN", "");
        Debug.Log("BIN Upload läuft");
        // Lese den Dateiinhalt in ein Byte[] Array ein
        byte[] fileContent = File.ReadAllBytes(filePath);
        // Erstelle eine UploadHandlerRaw-Instanz mit dem Array
        request.uploadHandler = new UploadHandlerRaw(fileContent);
        // Setze den Content-Type Header
        request.SetRequestHeader("Content-Type", "application/octet-stream");
        // Sende den Request an den Server 
        request.SendWebRequest();

        // Warte, bis der Request abgeschlossen ist
        while (!request.isDone)
        {
            yield return null;
        }

        // überprfe, ob der Request erfolgreich war
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            popupMessage.PopUp("Serververbindung ist fehlgeschlagen! Code: " + request.responseCode);
            Debug.Log(request.result.ToString());
            loadingScreen.SetActive(false);
            yield break;
        }
        else
        {
            // Lese die Antwort des Servers in ein Byte[] Array ein
            byte[] responseData = request.downloadHandler.data;
            // Schreibe das Array in eine Datei
            File.WriteAllBytes(savePath, responseData);
            Debug.Log($"Die Antwort wurde in {savePath} gespeichert.");
            receivedBIN = true;
            if (receivedBIN && receivedGLTF)
            {
                loadingScreen.SetActive(false);
                GLTFstuff.ImportGameObjectFromPath(GameObject.Find("ImageTarget"), gltfPath);
                stopwatch.Stop();
                string msg = "Total time taken to up- & download and visualize .gltf & .bin from server: " + stopwatch.ElapsedMilliseconds + " milliseconds.";
                Debug.Log(msg);
                popupMessage.PopUp(msg);
            }
            else
            {
                loadingScreen.SetActive(false);
                popupMessage.PopUp("File Upload ist fehlgeschlagen");
            }
        }
    }


    public static IEnumerator downloadGLTF(string savePath)
    {
        receivedGLTF = false;

        // Send a GET request to the server
        using UnityWebRequest request = UnityWebRequest.Get("http://" + serverHost + ":8000/downloadGLTF");
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Irgendetwas ist schief gelaufen");
            Debug.LogError(request.error);
            yield break;
        }
        else
        {
            // Write the file to disk
            byte[] data = request.downloadHandler.data;
            File.WriteAllBytes(savePath, data);
            Debug.Log("File saved to: " + savePath);
            receivedGLTF = true;
        }
    }
    public static IEnumerator downloadBIN(string savePath, string gltfPath, Stopwatch stopwatch, PopupMessage popupMessage, GameObject loadingScreen)
    {

        receivedBIN = false;
        // Send a GET request to the server
        using UnityWebRequest request = UnityWebRequest.Get("http://" + serverHost + ":8000/downloadBIN");
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            popupMessage.PopUp("Serververbindung ist fehlgeschlagen");
            Debug.Log(request.result.ToString());
            loadingScreen.SetActive(false);
            yield break;
        }
        else
        {
            // Write the file to disk
            byte[] data = request.downloadHandler.data;
            File.WriteAllBytes(savePath, data);
            Debug.Log("File saved to: " + savePath);
            receivedBIN = true;
            if (receivedBIN && receivedGLTF)
            {
                loadingScreen.SetActive(false);
                GLTFstuff.ImportGameObjectFromPath(GameObject.Find("ImageTarget"), gltfPath);
                stopwatch.Stop();
                string msg = "Total time taken to download and visualize .gltf & .bin from server: " + stopwatch.ElapsedMilliseconds + " milliseconds.";
                Debug.Log(msg);
                popupMessage.PopUp(msg);
            }
            else
            {
                loadingScreen.SetActive(false);
                popupMessage.PopUp("File Download ist fehlgeschlagen");
            }
        }
    }
}
