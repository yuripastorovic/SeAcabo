using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;                       // Prefab de los elementos de la lista
    [SerializeField] private Transform contentTransform;                  // Transform del Content en el Scroll Rect

    [SerializeField] private Sprite accept;
    [SerializeField] private Sprite cancel;

    private List<Card> alterMaze;
    private List<Card> currentMaze;
    private int turno;
    private string winner;
    private int stage;
    private string team1;
    private string team2;

    void Start()
    {
        AudioManager.Instance.Play(3);
        stage = PlayerPrefs.GetInt("ronda");
        alterMaze = new List<Card>();
        alterMaze.Clear();
        alterMaze = LocalData.Instance.Load("altMaze");
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
        // esto huele sospechoso
        currentMaze = new List<Card>();
        currentMaze.Clear();
        currentMaze = LocalData.Instance.GetCurrentGameMaze();

        for (int i = 0; i < currentMaze.Count; i++) 
        {
            for (int j =0; j< alterMaze.Count; j++)
            {
                if (currentMaze[i].Name== alterMaze[j].Name)
                {
                    if (alterMaze[j].Winner == "fail")
                    {
                        alterMaze[j].Winner = "unknown";
                    }
                    currentMaze[i] = alterMaze[j];
                }
            }
        }
        LocalData.Instance.SaveCurrentGameMaze(currentMaze);
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
            card.Winner = "fail";
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
        for (int i = 0; i < alterMaze.Count; i++)
        {
            GenerateItem(alterMaze[i], i);
        }
    }

    private void EndScene()
    {
        turno++;
        PlayerPrefs.SetInt("turno", turno);
        PlayerPrefs.Save();
        if (IsEndStage()) 
        {
            // la idea es guardar el que se usa en esta partida, verificar en la siguiente y sobre escribir cambios 
            SceneManager.LoadScene(4);
        }
        else
        {
            SceneManager.LoadScene(2);
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
    public void ImprimirUna(List<Card> cardList)
    {
        if (cardList.Count != 0)
        {
            int cont = 0;
            foreach (var maze in cardList)
            {
                cont++;
                Debug.Log($"{cont} --Name = {maze.Name}, Category = {maze.Category}, Winner = {maze.Winner}, desc = {maze.Desc}");
            }
        }
        else
        {
            Debug.Log("Fiera la baraja esta vacia:");
        }
    }
}
