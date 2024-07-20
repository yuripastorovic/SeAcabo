using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
    // NO ESTAMOS CARGANDO LOS ITEMS EN LA LISTA U BLOQUEMAOS EL BOTON DE SALIR
    // ImprimirUna(LocalData.Instance.Load("altMaze")); esta funciona
    [SerializeField] private GameObject itemPrefab;                       // Prefab de los elementos de la lista
    [SerializeField] private Transform contentTransform;                  // Transform del Content en el Scroll Rect

    [SerializeField] private Sprite accept;
    [SerializeField] private Sprite cancel;

    private List<Card> currentMaze;
    private int turno;
    private string winner;
    private int stage;
    private string team1;
    private string team2;

    void Start()
    {
        stage = PlayerPrefs.GetInt("ronda");
        currentMaze = new List<Card>();
        currentMaze = LocalData.Instance.Load("altMaze");
        GetTeam();
        PopulateScrollView();
    }
    public void OnNext()
    {
        UpdateMaze();
        EndScene();
    }

    private void UpdateMaze()
    {
        List<Card> gameMaze = new List<Card>();
        gameMaze.Clear();
        gameMaze = LocalData.Instance.GetCurrentGameMaze();
        foreach (var currentCard in currentMaze)
        {
            for (int i = 0; i < gameMaze.Count; i++)
            {
                if (gameMaze[i].Name == currentCard.Name)
                {
                    gameMaze[i] = currentCard;
                    break;
                }
            }
        }
        LocalData.Instance.SaveCurrentGameMaze(gameMaze);
    }

    private void GetTeam()
    {
        turno = PlayerPrefs.GetInt("turno");
        if (turno % 2 == 0)
        {
            winner = PlayerPrefs.GetString("team2");
        }
        else
        {
            winner = PlayerPrefs.GetString("team1");
        }
    }
    private void GenerateItem(Card card, int index)
    {
        GameObject newItem = Instantiate(itemPrefab, contentTransform);

        RectTransform rectTransform = newItem.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, -index * 150);

        newItem.GetComponentInChildren<Text>().text = card.Name;
        Button button = newItem.GetComponentInChildren<Button>();

        if (card.Winner.Equals(winner))
        {
            button.GetComponent<Image>().sprite = accept;
        }
        else
        {
            button.GetComponent<Image>().sprite = cancel;
        }

        button.onClick.AddListener(() => OnItemClick(card, button));
    }

    private void OnItemClick(Card card, Button button)
    {
        if (card.Winner.Equals(winner))
        {
            card.Winner = "unknown";
            button.GetComponent<Image>().sprite = cancel;
        }
        else
        {
            card.Winner = winner;
            button.GetComponent<Image>().sprite = accept;
        }
    }

    private void PopulateScrollView()
    {
        for (int i = 0; i < currentMaze.Count; i++)
        {
            GenerateItem(currentMaze[i], i);
        }
    }

    private void EndScene()
    {
        turno++;
        PlayerPrefs.SetInt("turno", turno);
        PlayerPrefs.Save();
        Debug.Log(LocalData.Instance.Load("altMaze").Count);
        if (IsEndStage()) 
        {
            // la idea es guardar el que se usa en esta partida, verificar en la siguiente y sobre escribir cambios 
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
        
    }
    private bool IsEndStage()
    {
        if(stage == 1)
        {
            var compare = LocalData.Instance.GetGameMaze1();
            bool respuesta = compare.All(card => !card.Winner.Equals("unknown"));

            if (respuesta)
            {
                int countTeam1 = compare.Count(card => card.Winner.Equals(PlayerPrefs.GetString("team1")));
                int countTeam2 = compare.Count(card => card.Winner.Equals(PlayerPrefs.GetString("team2")));
                PlayerPrefs.SetInt("round1Team1", countTeam1);
                PlayerPrefs.SetInt("round1Team2", countTeam2);
                PlayerPrefs.Save();
            }

            return respuesta;
        }
        else if (stage == 2) 
        {
            var compare = LocalData.Instance.GetGameMaze2();
            bool respuesta = compare.All(card => !card.Winner.Equals("unknown"));

            if (respuesta)
            {
                int countTeam1 = compare.Count(card => card.Winner.Equals(PlayerPrefs.GetString("team1")));
                int countTeam2 = compare.Count(card => card.Winner.Equals(PlayerPrefs.GetString("team2")));
                PlayerPrefs.SetInt("round2Team1", countTeam1);
                PlayerPrefs.SetInt("round2Team2", countTeam2);
                PlayerPrefs.Save();
            }

            return respuesta;
        }
        else
        {
            var compare = LocalData.Instance.GetGameMaze3();
            bool respuesta = compare.All(card => !card.Winner.Equals("unknown"));

            if (respuesta)
            {
                int countTeam1 = compare.Count(card => card.Winner.Equals(PlayerPrefs.GetString("team1")));
                int countTeam2 = compare.Count(card => card.Winner.Equals(PlayerPrefs.GetString("team2")));
                PlayerPrefs.SetInt("round3Team1", countTeam1);
                PlayerPrefs.SetInt("round3Team2", countTeam2);
                PlayerPrefs.Save();
            }

            return respuesta;
        }
    }
}
