using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using SFB;

public class SelectLevel : MonoBehaviour
{
    public string path;
    public void ElegirArchivo()
    {
        path = StandaloneFileBrowser.OpenFolderPanel("Select Folder", UnityEngine.Application.persistentDataPath, false)[0];

        PlayerPrefs.SetString("path", path);
        if (path != "")
        {
            SceneManager.LoadScene("Editor", LoadSceneMode.Single);
        }

    }
}
