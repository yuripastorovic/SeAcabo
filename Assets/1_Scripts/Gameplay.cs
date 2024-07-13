using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    #region COMPONENTS
    [SerializeField] private Text nameCard;
    [SerializeField] private Text descCard;
    [SerializeField] private Text timmer;
    [SerializeField] private GameObject background;
    #endregion
    #region VARIABLES
    private float timeRemaining;
    private bool timerIsRunning = false;
    private int stage;
    private List<Card> currentMaze;
    private int index = 0;
    private int turno;
    private string winner;
    #endregion
    void Start()
    {
        currentMaze = new List<Card>();
        currentMaze = LocalData.Instance.GetCurrentGameMaze();
        LocalData.Instance.alterMaze.Clear();
        SetInitialTime();
        SetScene();
        GetTeam();
        
    }
    // Update is called once per frame
    void Update()
    {
        CountDownTimmer();
        UpdateTimmerText();
    }
    #region ON CLICK
    public void OnCorrect()
    {
        //UpdateNameDesc();
        currentMaze[index].Winner = winner;
        LocalData.Instance.alterMaze.Add(currentMaze[index]);
    }
    public void OnFail()
    {
        //UpdateNameDesc();
        ReduceTime();
        currentMaze[index].Winner = "fail";
        LocalData.Instance.alterMaze.Add(currentMaze[index]);
    }
    #endregion
    #region GUI UPDATE
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
    public void GetTeam()
    {
        if (turno % 2 == 0)
        {
            winner = PlayerPrefs.GetString("team2");
        }
        else
        {
            winner = PlayerPrefs.GetString("team1");
        }
    }
    private void FindNextCard()
    {
        //Options.Instance.ImprimirUna(currentMaze);
        //Debug.Log(currentMaze.FindIndex(card => card.Winner == "unknown")+"------"+currentMaze.Count);
        index = currentMaze.FindIndex(card => card.Winner == "unknown");
    }
    private void UpdateNameDesc()
    {
        FindNextCard();
        nameCard.text = currentMaze[index].Name;
        if (PlayerPrefs.GetInt("help") == 1)
        {
            descCard.text = currentMaze[index].Desc;
        }
        else
        {
            descCard.text = "";
        }
    }
    private void UpdateTimmerText()
    {
        int seconds = Mathf.FloorToInt(timeRemaining);
        timmer.text = seconds.ToString();
    }

    public void SetInitialTime()
    {
        stage = PlayerPrefs.GetInt("ronda");
        float time;
        if (stage == 1)
        {
            time = 30f;
        }
        else if (stage == 2) 
        {
            time = 40f;
        }
        else
        {
            time = 60f;
        }
        timeRemaining = time;
        timerIsRunning = true;
        UpdateTimmerText();
    }
    private void SetScene()
    {
        turno = PlayerPrefs.GetInt("turno");
        if (turno % 2 == 0)
        {
            background.GetComponent<Image>().color = Color.blue;
        }
        else
        {
            background.GetComponent<Image>().color = Color.green;
        }
        //UpdateNameDesc();
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
    private void EndScene()
    {
        currentMaze[index].Winner = "fail";
        LocalData.Instance.alterMaze.Add(currentMaze[index]);
        turno++;
        PlayerPrefs.SetInt("turno", turno);
        PlayerPrefs.Save();
        LocalData.Instance.Save(LocalData.Instance.alterMaze, "alterMaze");
        // la idea es guardar el que se usa en esta partida, verificar en la siguiente y sobre escribir cambios 
        SceneManager.LoadScene(3);
    }
    #endregion    
}
