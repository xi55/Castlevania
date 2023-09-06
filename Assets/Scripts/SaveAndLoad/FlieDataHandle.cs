using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FlieDataHandle
{
    private string dataDirPath = "";
    private string dataFileName = "";

    private bool encrypt = false;
    private string codeWord = "daxi";

    public FlieDataHandle(string _dataDirPath, string _dataFileName, bool _encrypt)
    {
        this.dataDirPath = _dataDirPath;
        this.dataFileName = _dataFileName;
        this.encrypt = _encrypt;
    }

    public void Save(GameData _gameData)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(_gameData, true);
            if(encrypt)
                dataToStore = EncryptData(dataToStore);
            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(dataToStore);
                }
            }    
        }
        catch(Exception e)
        {
            Debug.LogError("Error on trying to save data to flie: " + fullPath + "\n" + e);
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData data = null;
        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream fs = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader sr = new StreamReader(fs))
                        dataToLoad = sr.ReadToEnd();
                }
                if (encrypt)
                    dataToLoad = EncryptData(dataToLoad);
                data = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error on trying to load data from flie: " + fullPath + "\n" + e);
            }
        }
        return data;
    }

    public void DeleteData()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if(File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    private string EncryptData(string _data)
    {
        string res = "";
        for(int i = 0; i < _data.Length; i++)
        {
            res += (char)(_data[i] ^ codeWord[i % codeWord.Length]);
        }

        return res;
    }
}
