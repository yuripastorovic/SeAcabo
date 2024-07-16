using System;
using System.Collections.Generic;
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

    void Start()
    {
        currentMaze = new List<Card>();
        currentMaze = LocalData.Instance.Load("alterMaze");
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
    private void GenerateItem(Card card)
    {
        GameObject newItem = Instantiate(itemPrefab, contentTransform);
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
        foreach (var item in currentMaze)
        {
            GenerateItem(item);
        }
    }

    private void EndScene()
    {
        turno++;
        PlayerPrefs.SetInt("turno", turno);
        PlayerPrefs.Save();
        Debug.Log(LocalData.Instance.Load("altMaze").Count);
        // la idea es guardar el que se usa en esta partida, verificar en la siguiente y sobre escribir cambios 
        SceneManager.LoadScene(0);
    }
}
