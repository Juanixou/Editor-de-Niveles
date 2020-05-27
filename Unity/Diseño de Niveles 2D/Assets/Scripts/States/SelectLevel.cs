using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using SFB;

public class SelectLevel : MonoBehaviour
{
    public string path;
    private bool clicked;
    private float time;

    private void Start()
    {
        path = "";
        clicked = false;
        time = 1;
    }
    public void ElegirArchivo()
    {

        if (path == "")
        {
            path = StandaloneFileBrowser.OpenFolderPanel("Select Folder", UnityEngine.Application.persistentDataPath, false)[0];
        }

        PlayerPrefs.SetString("path", path);
        if (path != "")
        {

            SceneManager.LoadScene("Editor", LoadSceneMode.Single);
        }
    }

    public void JugarNivel()
    {
        if (path == "")
        {
            path = StandaloneFileBrowser.OpenFolderPanel("Select Folder", UnityEngine.Application.persistentDataPath, false)[0];
        }
        
        if (path != "")
        {
            PlayerPrefs.SetString("playPath", path);
            SceneManager.LoadScene("Play", LoadSceneMode.Single);
        }
    }
}
