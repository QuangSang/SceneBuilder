using System.IO;
using UnityEngine;

/// <summary>
/// Responsible for saving/loading the scene
/// </summary>
public class SceneLoader
{
    private const string FILE_NAME = "scene.json";
    private string _savePath;
    public SceneLoader()
    {
        _savePath = Path.Combine(Application.persistentDataPath, FILE_NAME);
    }
    public void SaveScene(SceneData sceneData)
    {
        var json = JsonUtility.ToJson(sceneData);
        File.WriteAllText(_savePath, json);
        Debug.Log("Scene data saved to: " + _savePath);
    }

    public SceneData LoadScene()
    {
        if (!File.Exists(_savePath))
        {
            Debug.LogWarning("No save file found at: " + _savePath);
            return null;
        }

        string json = File.ReadAllText(_savePath);
        var data = JsonUtility.FromJson<SceneData>(json);
        return data;
    }

    public bool HasSavedScene()
    {
        return File.Exists(_savePath);
    }
}
