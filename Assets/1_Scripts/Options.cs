using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
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

    private List<Card> maze;
    private List<Card> gameMaze;

    void Start()
    {
        anime = true;
        futbol = true;
        farandula = true;

        maze = new List<Card>();
        gameMaze = new List<Card>();
    }

    void Update()
    {
        Refesh();
    }

    public void OnAnime()
    {
        anime = !anime;
    }
    public void OnFutbol()
    {
        futbol = !futbol;
    }
    public void OnFarandula()
    {
        farandula = !farandula;
    }
    public void OnPlay()
    {
        GameConfig();
        SelectCards();
    }
    public async void OnUpdate()
    {
        await Connections.Instance.GetCommon();
        //System.Threading.Tasks.Task task = Connections.Instance.GetCommon();
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

    private void GameConfig()
    {
        PlayerPrefs.SetInt("anime", anime ? 1 : 0);
        PlayerPrefs.SetInt("futbol", futbol ? 1 : 0);
        PlayerPrefs.SetInt("farandula", farandula ? 1 : 0);

        PlayerPrefs.SetString("team1", team1.text);
        PlayerPrefs.SetString("team2", team2.text);

        PlayerPrefs.Save();
    }

    private void SelectCards()
    {
        maze.Clear();
        gameMaze.Clear();

        maze = Connections.Instance.common;
        if (anime) 
        {
            maze = maze.Concat(Connections.Instance.anime).ToList();
        }
        if (futbol)
        {
            maze = maze.Concat(Connections.Instance.futbol).ToList();
        }
        if (farandula) 
        {
            maze = maze.Concat(Connections.Instance.farandula).ToList();
        }
        var random = new System.Random();
        gameMaze = maze.OrderBy(x => random.Next()).Take(40).ToList();
        SaveMaze();
    }

    private void SaveMaze()
    {
        Debug.Log("Baraja guardada");
        int lol = 0;
        foreach (var maze in gameMaze) 
        {
            lol++;
            Debug.Log($"{lol} --Name = {maze.Name}, Category = {maze.Category}, Winner = {maze.Winner}, desc = {maze.Desc}");
        }
    }
}
