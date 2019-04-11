using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    public static string path;
    public static void ElegirArchivo()
    {
        path = EditorUtility.OpenFilePanel("Select Level", Application.persistentDataPath, "txt");
        SceneManager.LoadScene("Editor", LoadSceneMode.Single);
    }
}
