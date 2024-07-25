using System.Collections.Generic;
using UnityEngine;
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
    private bool isrunning = false;
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
        SetInitialScene();
        SetScene();
    }
    void Update()
    {
        CountDownTimmer();
    }
    #region ON CLICK
    public void OnCorrect()
    {
        AudioManager.Instance.Play(0);
        currentMaze[index].Winner = winner;
        altMaze.Add(currentMaze[index]);
        UpdateNameDesc();
    }
    public void OnFail()
    {
        AudioManager.Instance.Play(1);
        if (stage == 1)
        {
            ReduceTime();
        }
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
            background.GetComponent<Image>().color = new Color32(70, 130, 180, 255);
        }
        else
        {
            winner = PlayerPrefs.GetString("team1");
            background.GetComponent<Image>().color = new Color32(255, 165, 0, 255);
        }
        UpdateNameDesc();
    }
    /// <summary>
    /// Actualiza las cartas en pantalla
    /// </summary>
    private void UpdateNameDesc()
    {
        index = currentMaze.FindIndex(card => card.Winner == "unknown");
        if (index < 0)
        {
            AudioManager.Instance.Play(3);
            LocalData.Instance.Save(altMaze, "altMaze");
            //Debug.Log(LocalData.Instance.Load("altMaze").Count);
            AudioManager.Instance.Stop();
            SceneManager.LoadScene(3);
        }
        else
        {
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
    }
    #endregion
    #region TIME
    /// <summary>
    /// En funcion de la "ronda" prepara una tiempo u otro
    /// </summary>
    public void SetInitialScene()
    {
        stage = PlayerPrefs.GetInt("ronda");
        float time;
        if (stage == 1)
        {
            currentMaze = LocalData.Instance.GetGameMaze1();
            time = 30f; 
        }
        else if (stage == 2)
        {
            currentMaze = LocalData.Instance.GetGameMaze2();
            time = 40f;
        }
        else if (stage == 3)
        {
            currentMaze = LocalData.Instance.GetGameMaze3();
            time = 60f;
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
        if (timeRemaining<0)
        {
            seconds = 0;
        }
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
                AudioManager.Instance.Play(3);
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimmerText();
                EndScene();
            }
            if(timeRemaining <= 10 && !isrunning)
            {
                AudioManager.Instance.Play(2);
                isrunning= true;
            }
        }
    }
    #endregion
    #region DISPARADORES
    private void EndScene()
    {
        if (index >= 0)
        {
            currentMaze[index].Winner = "fail";
            altMaze.Add(currentMaze[index]);
        }
        
        LocalData.Instance.Save(altMaze, "altMaze");
        //Debug.Log(LocalData.Instance.Load("altMaze").Count);
        AudioManager.Instance.Stop();
        SceneManager.LoadScene(3);
    }
    #endregion
}
