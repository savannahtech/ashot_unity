using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[Serializable]
public class User
{
    //these variables are case sensitive and must match the strings "firstName" and "lastName" in the JSON.
    public string name;
    public string email;
}

[Serializable]
public class Users
{
    public User[] users;
}

public class ARObj_Controller : MonoBehaviour
{
    // Start is called before the first frame update

    public Canvas Cnvs;

    public Text TxtFromServer;

    private string url = "https://10xfilesbucket.s3.amazonaws.com/JsonFiles/TestJsonFile.json";

    void Start()
    {
        Cnvs.worldCamera = Camera.main;
        DisableAR();
        StartCoroutine(LoadTextFromServer());
    }

    void DisableAR() {
        GameObject ARSystem = GameObject.Find("AR Session Origin");
        ARSystem.GetComponent<ARAnchorManager>().enabled = false;
    }

    IEnumerator LoadTextFromServer() {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                TxtFromServer.text = "Internet problem";
            }
            else
            {
                var Users = JsonUtility.FromJson<Users>(webRequest.downloadHandler.text);

                TxtFromServer.text = "This text comes from my AWS Server";

                foreach (var item in Users.users)
                {
                    TxtFromServer.text += "\n" + item.name + " : " + item.email;
                }
                
            }
        }
    }
}
