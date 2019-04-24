using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    public string path;
    public void ElegirArchivo()
    {
        path = EditorUtility.OpenFilePanel("Select Level", Application.persistentDataPath, "txt");
        PlayerPrefs.SetString("path", path);
        if (path != "")
        {
            SceneManager.LoadScene("Editor", LoadSceneMode.Single);
        }

    }
}
