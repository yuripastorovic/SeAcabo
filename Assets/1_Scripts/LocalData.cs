using System.Collections.Generic;
using UnityEngine;
using NewtonsoftJson = Newtonsoft.Json.JsonConvert;
using NewtonsoftFormatting = Newtonsoft.Json.Formatting;

public class LocalData : MonoBehaviour
{
    #region SINGLETON
    private static LocalData _instance;
    public static LocalData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LocalData>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<LocalData>();
                    singletonObject.name = typeof(LocalData).ToString() + " (Singleton)";

                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }
    #endregion
    #region SAVE
    public void Save(List<Card> maze, string mazeName)
    {
        string json = NewtonsoftJson.SerializeObject(maze, NewtonsoftFormatting.Indented);
        //Debug.Log("Serialized JSON: " + json);

        PlayerPrefs.SetString(mazeName, json);
    }

    public void SaveAll()
    {
        Save(Options.Instance.gameMaze1, "maze1");
        Save(Options.Instance.gameMaze2, "maze2");
        Save(Options.Instance.gameMaze3, "maze3");
    }
    
    public void SaveDefaultMaze()
    {
        Save(Options.Instance.defaultMaze, "maze0");
    }
    public void SavePersonalOptions()
    {
        // Seleccion de opciones de juego
        PlayerPrefs.SetInt("anime", Options.Instance.anime ? 1 : 0);
        PlayerPrefs.SetInt("futbol", Options.Instance.futbol ? 1 : 0);
        PlayerPrefs.SetInt("farandula", Options.Instance.farandula ? 1 : 0);

        // Seleccion de nombres de equipo
        PlayerPrefs.SetString("team1", Options.Instance.team1.text);
        PlayerPrefs.SetString("team2", Options.Instance.team2.text);

        // Confirmación de guardado
        PlayerPrefs.Save();
    }
    #endregion
    #region LOAD
    private List<Card> Load(string mazeName)
    {
        List<Card> maze = new List<Card>();
        string jsonFromPrefs = PlayerPrefs.GetString(mazeName);
        if (!string.IsNullOrEmpty(jsonFromPrefs))
        {
            maze = NewtonsoftJson.DeserializeObject<List<Card>>(jsonFromPrefs);
            //Debug.Log("Deserialized List: " + maze.Count + " items");
        }
        else
        {
            //Debug.Log("No JSON data found in PlayerPrefs.");
        }
        return maze;
    }

    public void LoadAll()
    {
        Options.Instance.gameMaze1 = Load("maze1");
        Options.Instance.gameMaze2 = Load("maze2");
        Options.Instance.gameMaze3 = Load("maze3");
    }

    public void LoadDefaultMaze()
    {
        Options.Instance.defaultMaze = Load("maze0");
    }
    public void LoadPersonalOptions()
    {
        // Cargar selección de opciones de juego
        Options.Instance.anime      = PlayerPrefs.GetInt("anime") == 1;
        Options.Instance.futbol     = PlayerPrefs.GetInt("futbol") == 1;
        Options.Instance.farandula  = PlayerPrefs.GetInt("farandula") == 1;

        // Cargar nombres de equipo
        Options.Instance.team1.text = PlayerPrefs.GetString("team1", "");
        Options.Instance.team2.text = PlayerPrefs.GetString("team2", "");
    }
    #endregion


}