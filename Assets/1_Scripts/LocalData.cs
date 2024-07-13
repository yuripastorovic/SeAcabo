using System.Collections.Generic;
using UnityEngine;
using NewtonsoftJson = Newtonsoft.Json.JsonConvert;
using NewtonsoftFormatting = Newtonsoft.Json.Formatting;

public class LocalData : MonoBehaviour
{
    private static LocalData _instance;
    [HideInInspector] public List<Card> gameMaze;
    [HideInInspector] public List<Card> alterMaze;
    [HideInInspector] public List<Card> newAlterMaze;
    #region SINGLETON
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
    private void Start()
    {
        gameMaze = new List<Card>();
        alterMaze = new List<Card>();       // Almacena lo que se juega en una partida, las que se ven
    }
    #region SAVE
    public void Save(List<Card> maze, string mazeName)
    {
        string json = NewtonsoftJson.SerializeObject(maze, NewtonsoftFormatting.Indented);

        PlayerPrefs.SetString(mazeName, json);
        PlayerPrefs.Save();
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
        PlayerPrefs.SetInt("help", Options.Instance.help ? 1 : 0);

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
    public List<Card> GetGameMaze1()
    {
        return Load("maze1");
    }
    public List<Card> GetGameMaze2()
    {
        return Load("maze2");
    }
    public List<Card> GetGameMaze3()
    {
        return Load("maze3");
    }
    public List<Card> GetDefaultMaze()
    {
        return Load("maze0");
    }
    public void LoadDefaultMaze()
    {
        Options.Instance.defaultMaze = Load("maze0");
    }
    public void LoadPersonalOptions()
    {
        // Cargar selección de opciones de juego
        Options.Instance.anime      = PlayerPrefs.GetInt("anime")     == 1;
        Options.Instance.futbol     = PlayerPrefs.GetInt("futbol")    == 1;
        Options.Instance.farandula  = PlayerPrefs.GetInt("farandula") == 1;
        Options.Instance.help       = PlayerPrefs.GetInt("help")      == 1;

        // Cargar nombres de equipo
        Options.Instance.team1.text = PlayerPrefs.GetString("team1", "");
        Options.Instance.team2.text = PlayerPrefs.GetString("team2", "");
    }
    #endregion
    public List<Card> GetCurrentGameMaze()
    {
        Options.Instance.ImprimirBarajas();
        int stage = PlayerPrefs.GetInt("ronda");
        if(stage == 1)
        {
            
            return Options.Instance.gameMaze1;
        }
        else if (stage == 2) 
        {
            return Options.Instance.gameMaze2;
        }
        else if (stage == 3)
        {
            return Options.Instance.gameMaze3;
        }
        else
        {
            return null;
        }
    }
}