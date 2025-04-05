using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string _dataDirPath, string _dataFileName)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
           Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
           string dataToStore = JsonUtility.ToJson(_data,true);
           using (FileStream stream = new FileStream(fullPath,FileMode.Create))
           {
               using (StreamWriter writer = new StreamWriter(stream))
               {
                   writer.Write(dataToStore);
               }
               
           }
        }
        catch(Exception e)
        {
            Debug.LogError("error on trying to save data to file"+fullPath+"\n"+e);
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stram = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stram))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("fuck this shit"+e);
            }
            
        }

        return loadData;
    }

    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if(File.Exists(fullPath))
            File.Delete(fullPath);
    }
}
