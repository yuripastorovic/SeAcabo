using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    private static Options _instance;

    [SerializeField] public GameObject jugar;

    [SerializeField] private Image imageAnime;
    [SerializeField] private Image imageFutbol;
    [SerializeField] private Image imageFarandula;

    [SerializeField] private InputField team1;
    [SerializeField] private InputField team2;

    [SerializeField] private Sprite accept;
    [SerializeField] private Sprite cancel;

    private bool anime;
    private bool futbol;
    private bool farandula;

    public List<Card> defaultMaze;                      // Representa al mazo por defecto
    public List<Card> personalMaze;                     // Representa el mazo personal del jugador +40 cards
    public List<Card> gameMaze1;                        // Representa el mazo del primer juego
    public List<Card> gameMaze2;                        // Representa el mazo del segundo juego
    public List<Card> gameMaze3;                        // Representa el mazo del tercer juego

    private bool saved = false;

    public static Options Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Options>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<Options>();
                    singletonObject.name = typeof(Options).ToString() + " (Singleton)";

                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }
    void Start()
    {
        LocalData.Instance.LoadDefaultMaze();
        jugar.SetActive(saved);

        LoadOptions();

        personalMaze = new List<Card>();
        gameMaze1    = new List<Card>();
        gameMaze2    = new List<Card>();
        gameMaze3    = new List<Card>();
    }

    void Update()
    {
        Refesh();
        if (defaultMaze.Count != 0)
        {
            saved = true;
        }
        jugar.SetActive(saved);
    }

    public void OnAnime()
    {
        anime = !anime;
        PlayerPrefs.SetInt("anime", anime ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnFutbol()
    {
        futbol = !futbol;
        PlayerPrefs.SetInt("futbol", futbol ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnFarandula()
    {
        farandula = !farandula;
        PlayerPrefs.SetInt("farandula", farandula ? 1 : 0);
        PlayerPrefs.Save();
    }

    public async void OnUpdate()
    {
        await Connections.Instance.GetCards();
        //System.Threading.Tasks.Task task = Connections.Instance.GetCommon();
        LocalData.Instance.SaveDefaultMaze();
        LocalData.Instance.LoadDefaultMaze();
    }

    public void OnPlay()
    {
        GameConfig();
        SelectCards();
        SaveMaze();
    }

   

    

    private void GameConfig()
    {
        // Seleccion de opciones de juego
        PlayerPrefs.SetInt("anime", anime ? 1 : 0);
        PlayerPrefs.SetInt("futbol", futbol ? 1 : 0);
        PlayerPrefs.SetInt("farandula", farandula ? 1 : 0);

        // Seleccion de nombres de equipo
        PlayerPrefs.SetString("team1", team1.text);
        PlayerPrefs.SetString("team2", team2.text);

        // Confirmación de guardado
        PlayerPrefs.Save();
    }

    private void SelectCards()
    {
        // Vaciamos barajas
        personalMaze.Clear();
        gameMaze1.Clear();
        gameMaze2.Clear();
        gameMaze3.Clear();

        // Creamos una nueva baraja de juego en funcion de las opciones de usuario
        if (saved)
        {
            personalMaze = defaultMaze;
        }
        else
        {
            personalMaze = Connections.Instance.common;
        }
        
        if (anime) 
        {
            personalMaze = personalMaze.Concat(Connections.Instance.anime).ToList();
        }
        if (futbol)
        {
            personalMaze = personalMaze.Concat(Connections.Instance.futbol).ToList();
        }
        if (farandula) 
        {
            personalMaze = personalMaze.Concat(Connections.Instance.farandula).ToList();
        }

        // Creamos tres barajas, una para cada nivel
        var random = new System.Random();
        gameMaze1 = personalMaze.OrderBy(x => random.Next()).Take(40).ToList();
        gameMaze2 = gameMaze1.OrderBy(x => random.Next()).Take(40).ToList();
        gameMaze3 = gameMaze1.OrderBy(x => random.Next()).Take(40).ToList();
    }

    private void SaveMaze()
    {
        // Guardamos en local cada una de las barajas
        LocalData.Instance.SaveAll();
        LocalData.Instance.LoadAll();
    }

    private void LoadOptions()
    {
        // Cargar selección de opciones de juego
        anime     = PlayerPrefs.GetInt("anime") == 1;
        futbol    = PlayerPrefs.GetInt("futbol") == 1;
        farandula = PlayerPrefs.GetInt("farandula") == 1;

        // Cargar nombres de equipo
        team1.text = PlayerPrefs.GetString("team1", "");
        team2.text = PlayerPrefs.GetString("team2", "");
    }

    private void Refesh()
    {
        if (anime)
        {
            imageAnime.sprite = accept;
        }
        else
        {
            imageAnime.sprite = cancel;
        }

        if (futbol)
        {
            imageFutbol.sprite = accept;
        }
        else
        {
            imageFutbol.sprite = cancel;
        }

        if (farandula)
        {
            imageFarandula.sprite = accept;
        }
        else
        {
            imageFarandula.sprite = cancel;
        }
    }

    private void ImprimirBarajas()
    {
        int cont = 0;
        foreach (var maze in gameMaze1)
        {
            cont++;
            Debug.Log($"{cont} --Name = {maze.Name}, Category = {maze.Category}, Winner = {maze.Winner}, desc = {maze.Desc}");
        }
        cont = 0;
        foreach (var maze in gameMaze2)
        {
            cont++;
            Debug.Log($"{cont} --Name = {maze.Name}, Category = {maze.Category}, Winner = {maze.Winner}, desc = {maze.Desc}");
        }
        cont = 0;
        foreach (var maze in gameMaze3)
        {
            cont++;
            Debug.Log($"{cont} --Name = {maze.Name}, Category = {maze.Category}, Winner = {maze.Winner}, desc = {maze.Desc}");
        }
    }
}