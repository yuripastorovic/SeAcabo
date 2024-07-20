using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField] private Text title;
    [SerializeField] private Text descr;
    [SerializeField] private Text timmer;

    [SerializeField] private GameObject background;

    private float timeRemaining;
    private bool timerIsRunning = false;
    private int stage;
    private List<Card> currentMaze;
    private List<Card> altMaze;
    private int index = 0;
    private int turno;
    private string winner;

    void Start()
    {
        currentMaze = new List<Card>();
        altMaze     = new List<Card>();
        altMaze.Clear();
        SetInitialTime();
        SetScene();
        //ImprimirUna(currentMaze);
        //ImprimirUna(altMaze);

    }
    void Update()
    {
        CountDownTimmer();
    }
    #region ON CLICK
    public void OnCorrect()
    {
        currentMaze[index].Winner = winner;
        altMaze.Add(currentMaze[index]);
        UpdateNameDesc();
    }
    public void OnFail()
    {
        ReduceTime();
        currentMaze[index].Winner = "fail";
        altMaze.Add(currentMaze[index]);
        UpdateNameDesc();
    }
    #endregion
    #region SCENE
    /// <summary>
    /// Prepara una Scena en funcion del turno fondo y nombre de equipo.
    /// </summary>
    private void SetScene()
    {
        turno = PlayerPrefs.GetInt("turno");
        if (turno % 2 == 0)
        {
            winner = PlayerPrefs.GetString("team2");
            background.GetComponent<Image>().color = Color.blue;
        }
        else
        {
            winner = PlayerPrefs.GetString("team1");
            background.GetComponent<Image>().color = Color.green;
        }
        UpdateNameDesc();
    }
    /// <summary>
    /// Actualiza las cartas en pantalla
    /// </summary>
    private void UpdateNameDesc()
    {
        FindNextCard();
        title.text = currentMaze[index].Name;
        if (PlayerPrefs.GetInt("help") == 1)
        {
            descr.text = currentMaze[index].Desc;
        }
        else
        {
            descr.text = "";
        }
    }
    /// <summary>
    /// Busca la siguiente carta para seguir jugando
    /// </summary>
    private void FindNextCard()
    {
        index = currentMaze.FindIndex(card => card.Winner == "unknown");
        // nos quedamos sin cartas
        if (index < 0)
        {
            LocalData.Instance.Save(altMaze, "altMaze");
            Debug.Log(LocalData.Instance.Load("altMaze").Count);
            //ImprimirUna(LocalData.Instance.Load("altMaze"));
            // la idea es guardar el que se usa en esta partida, verificar en la siguiente y sobre escribir cambios 
            SceneManager.LoadScene(3);
        }
    }
    #endregion
    #region TIME
    /// <summary>
    /// En funcion de la "ronda" prepara una tiempo u otro
    /// </summary>
    public void SetInitialTime()
    {
        stage = PlayerPrefs.GetInt("ronda");
        Debug.Log("Stage" + stage);
        float time;
        if (stage == 2)
        {
            time = 30f;
            currentMaze = LocalData.Instance.GetGameMaze1();
        }
        else if (stage == 3)
        {
            time = 40f;
            currentMaze = LocalData.Instance.GetGameMaze2();
        }
        else if (stage == 5)
        {
            time = 60f;
            currentMaze = LocalData.Instance.GetGameMaze3();
        }
        else
        {
            time = 0f;
        }
        timeRemaining = time;
        timerIsRunning = true;
        UpdateTimmerText();
    }
    private void UpdateTimmerText()
    {
        int seconds = Mathf.FloorToInt(timeRemaining);
        timmer.text = seconds.ToString();
    }
    public void ReduceTime()
    {
        if (timerIsRunning && timeRemaining > 15)
        {
            timeRemaining -= 15;
            UpdateTimmerText();
        }
        else if (timerIsRunning)
        {
            timeRemaining = 0;
            timerIsRunning = false;
            UpdateTimmerText();
            EndScene();
        }
    }
    private void CountDownTimmer()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimmerText();
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimmerText();
                EndScene();
            }
        }
    }
    #endregion
    #region DISPARADORES
    private void EndScene()
    {
        currentMaze[index].Winner = "fail";
        altMaze.Add(currentMaze[index]);
        LocalData.Instance.Save(altMaze, "altMaze");
        Debug.Log(LocalData.Instance.Load("altMaze").Count);
        //ImprimirUna(LocalData.Instance.Load("altMaze"));
        // la idea es guardar el que se usa en esta partida, verificar en la siguiente y sobre escribir cambios 
        SceneManager.LoadScene(3);
    }
    #endregion

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
