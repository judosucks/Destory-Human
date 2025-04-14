// using System.Linq;
// using System;
// using System.Collections.Generic;
// using NUnit.Framework;
// using UnityEngine;
//
// public class SaveManager : MonoBehaviour
// {
//     public static SaveManager instance;
//     private GameData gameData;
//     private List<ISaveManager> saveManagers;
//     [SerializeField] private string fileName;
//     private FileDataHandler dataHandler;
//     private void Awake()
//     {
//         if (instance != null)
//         {
//             Destroy(instance.gameObject);
//         }
//         else
//         {
//             instance = this;
//         }
//     }
//
//     private void Start()
//     {
//         dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
//         saveManagers = FindAllSaveManagers();
//         LoadGame();
//     }
//
//     public void NewGame()
//     {
//         gameData = new GameData();
//     }
//
//     public void LoadGame()
//     {
//         gameData = dataHandler.Load();
//         if (this.gameData == null)
//         {
//             Debug.Log("no gamedada found");
//             NewGame();
//         }
//
//         foreach (ISaveManager saveManager in saveManagers)
//         {
//             saveManager.LoadData(gameData);
//         }
//         
//     }
//
//     public void SaveGame()
//     {
//         Debug.Log("save game");
//         foreach (ISaveManager saveManager in saveManagers)
//         {
//          saveManager.SaveData(ref gameData);   
//         }
//         dataHandler.Save(gameData);
//     }
//
//     private void OnApplicationQuit()
//     {
//         SaveGame();
//     }
//
//     private List<ISaveManager> FindAllSaveManagers()
//     {
//         //findobjectoftype has been depreated using findobjectbytype
//          saveManagers = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).
//              OfType<ISaveManager>().ToList();
//          return new List<ISaveManager>(saveManagers);
//
//     }
// }
using System.Linq;
using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private GameData gameData;
    private List<ISaveManager> saveManagers;
    [SerializeField] private string fileName;
    private FileDataHandler dataHandler;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
            
        }
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        saveManagers = FindAllSaveManagers();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }
    [ContextMenu("Delete Save")]
    public void DeleteSavedGame()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataHandler.Delete();
    }
    public void LoadGame()
    {
        gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("no gamedada found");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
        
    }

    public void SaveGame()
    {
        Debug.Log("save game");
        if (saveManagers == null) return; // Add this line
        foreach (ISaveManager saveManager in saveManagers) // Line 57
        {
            saveManager.SaveData(ref gameData);   
        }
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame(); // Line 66
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        //findobjectoftype has been depreated using findobjectbytype
        IEnumerable<ISaveManager> saveManagers = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).
            OfType<ISaveManager>().ToList();
        return new List<ISaveManager>(saveManagers);
    

    }

    public bool HasSavedData()
    {
        if (dataHandler.Load() != null)
        {
            return true;
        }

        return false;
    }
}