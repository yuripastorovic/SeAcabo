using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    private static Options _instance;
    #region COMPONENTS
    [SerializeField] public GameObject jugar;
    [SerializeField] public GameObject actualizar;

    [SerializeField] private Image imageAnime;
    [SerializeField] private Image imageFutbol;
    [SerializeField] private Image imageFarandula;

    [SerializeField] public InputField team1;
    [SerializeField] public InputField team2;

    [SerializeField] private Sprite accept;
    [SerializeField] private Sprite cancel;
    #endregion
    #region VARIABLES
    [HideInInspector] public bool anime;
    [HideInInspector] public bool futbol;
    [HideInInspector] public bool farandula;

    [HideInInspector] public List<Card> defaultMaze;                      // Representa al mazo por defecto
    [HideInInspector] public List<Card> personalMaze;                     // Representa el mazo personal del jugador +40 cards
    [HideInInspector] public List<Card> gameMaze1;                        // Representa el mazo del primer juego
    [HideInInspector] public List<Card> gameMaze2;                        // Representa el mazo del segundo juego
    [HideInInspector] public List<Card> gameMaze3;                        // Representa el mazo del tercer juego

    [HideInInspector] private bool saved         = false;
    [HideInInspector] private bool hasConnection = false;
    #endregion
    #region SINGLETON
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
    #endregion
    void Start()
    {
        InitializeMazes();
        CheckConnectivity();
        CheckLocalData();
    }

    void Update()
    {
        Refesh();
    }
    #region ON CLICK
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
        if (hasConnection)
        {
            await Connections.Instance.GetCards();
            LocalData.Instance.SaveDefaultMaze();
            UpdateLocalData();
        }
        ImprimirBarajas();
    }

    public void OnPlay()
    {
        SavePersonalOptions();
        UpdatePersonalMaze();
        ImprimirUna(defaultMaze);
        Debug.Log("----------------------------------");
        ImprimirUna(gameMaze2);
    }
    #endregion
    #region LOGIC
    private void InitializeMazes()
    {
        personalMaze = new List<Card>();
        gameMaze1    = new List<Card>();
        gameMaze2    = new List<Card>();
        gameMaze3    = new List<Card>();
    }
    private void SavePersonalOptions()
    {
        LocalData.Instance.SavePersonalOptions();
    }
    private void SaveMaze()
    {
        LocalData.Instance.SaveAll();
        LocalData.Instance.LoadAll();
    }
    private void UpdatePersonalMaze()
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
        SaveMaze();
    }
    #endregion
    #region OTHERS
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

    private void CheckConnectivity()
    {
        hasConnection = Application.internetReachability != NetworkReachability.NotReachable;
        actualizar.SetActive(hasConnection);
    }
    private void LoadFromLocal()
    {
        // Cragamos opciones del usuario de sesion anterior
        LocalData.Instance.LoadPersonalOptions();

        // Cargamos mazo por defecto si existe
        LocalData.Instance.LoadDefaultMaze();
    }
    private void CheckLocalData()
    {
        LoadFromLocal();
        saved = defaultMaze.Count != 0;
        jugar.SetActive(saved);
    }
    private void UpdateLocalData()
    {
        SaveMaze();
        SavePersonalOptions();
        CheckLocalData();
    }
   
    private void ImprimirUna(List<Card> cardList)
    {
        int cont = 0;
        foreach (var maze in cardList)
        {
            cont++;
            Debug.Log($"{cont} --Name = {maze.Name}, Category = {maze.Category}, Winner = {maze.Winner}, desc = {maze.Desc}");
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
    #endregion  
}