using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseManager : MonoBehaviour
{
    string username;
    string password;
    string uri;
    string contentType = "application/json";

    private void Awake()
    {
        LoadCredentials();
    }

    private string CreateJSON(string tabla, string name, string gender, int age, int progress)
    {
        //Construye JSON para la petición REST         
        string json = $@"{{
            ""username"":""{username}"",
            ""password"":""{password}"",
            ""table"":""{tabla}"",
            ""data"": {{
                ""name"": ""{name}"",
                ""gender"": ""{gender}"",
                ""age"": {age},
                ""progress"": {progress},
            }}
        }}";

        return json;
    }

    public IEnumerator SendPostRequest(string name, string gender, int age, int progress)
    {
        string data = CreateJSON("Users", name, gender, age, progress);

        using (UnityWebRequest www = UnityWebRequest.Post(uri, data, contentType))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print("Error: " + www.error);
            }
            else
            {
                print("Respuesta: " + www.downloadHandler.text);
            }
        }
    }

    private void LoadCredentials()
    {
        string configPath = "Assets/Resources/config.json";

        if (File.Exists(configPath))
        {
            string configJson = File.ReadAllText(configPath);
            var config = JsonUtility.FromJson<Credentials>(configJson);

            username = config.username;
            password = config.password;
            uri = config.uri;
        }
        else
        {
            Debug.LogError("Config file not found!");
        }
    }

    [System.Serializable]
    private class Credentials
    {
        public string username;
        public string password;
        public string uri;
    }
}
