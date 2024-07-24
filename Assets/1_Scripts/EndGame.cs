using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    #region HEADER
    [SerializeField] private Text team1;
    [SerializeField] private Text team2;
    #endregion
    #region ROUND 1
    [SerializeField] private Text team1Round1;
    [SerializeField] private Text team2Round1;
    #endregion
    #region ROUND 2
    [SerializeField] private Text team1Round2;
    [SerializeField] private Text team2Round2;
    #endregion
    #region ROUND 3
    [SerializeField] private Text team1Round3;
    [SerializeField] private Text team2Round3;
    #endregion
    #region TOTAL
    [SerializeField] private Text team1RoundT;
    [SerializeField] private Text team2RoundT;
    [SerializeField] private Text winner;
    #endregion
    private int stage;
    private string nameTeam1;
    private string nameTeam2;

    void Start()
    {
        InitializeScene();
    }
    void Update()
    {
        
    }
    public void OnClick()
    {
        ButtonAction();
    }
    private void InitializeScene()
    {
        stage = PlayerPrefs.GetInt("ronda");
        nameTeam1 = PlayerPrefs.GetString("team1");
        nameTeam2 = PlayerPrefs.GetString("team2");

        team1.text = nameTeam1;
        team2.text = nameTeam2;
        

        PaintScores();
    }
    private void PaintScores()
    {
        // Ronda 1
        int teamARonda1 = PlayerPrefs.GetInt("round1Team1");
        int teamBRonda1 = PlayerPrefs.GetInt("round1Team2");
        team1Round1.text = teamARonda1.ToString();
        team2Round1.text = teamBRonda1.ToString();

        if (stage >= 2)
        {
            // Ronda 2
            int teamARonda2 = PlayerPrefs.GetInt("round2Team1");
            int teamBRonda2 = PlayerPrefs.GetInt("round2Team2");
            Debug.Log("TEAM1+" + teamARonda2 + "||||||TEAM2+" + teamBRonda2);
            team1Round2.text = teamARonda2.ToString();
            team2Round2.text = teamBRonda2.ToString();

            if (stage >= 3)
            {
                // Ronda 3
                int teamARonda3 = PlayerPrefs.GetInt("round3Team1");
                int teamBRonda3 = PlayerPrefs.GetInt("round3Team2");
                Debug.Log("TEAM1+" + teamARonda3 + "||||||TEAM2+" + teamBRonda3);
                team1Round3.text = teamARonda3.ToString();
                team2Round3.text = teamBRonda3.ToString();

                // Totales
                int totalTeam1 = teamARonda1 + teamARonda2 + teamARonda3;
                int totalTeam2 = teamBRonda1 + teamBRonda2 + teamBRonda3;
                team1RoundT.text = totalTeam1.ToString();
                team2RoundT.text = totalTeam2.ToString();
                if (totalTeam1 > totalTeam2)
                {
                    winner.text = nameTeam1;
                }
                else if (totalTeam1 > totalTeam2)
                {
                    winner.text = nameTeam2;
                }
                else 
                {
                    winner.text = "Empate";
                }
            }
        }
    }
    private void ButtonAction()
    {
        if (stage == 1)
        {
            stage++;
            PlayerPrefs.SetInt("ronda", stage);
            PlayerPrefs.Save();
            SceneManager.LoadScene(1);
        }
        else if (stage == 2)
        {
            stage++;
            PlayerPrefs.SetInt("ronda", stage);
            PlayerPrefs.Save();
            SceneManager.LoadScene(1);
        }
        else
        {
            PlayerPrefs.SetInt("ronda", 0);
            PlayerPrefs.Save();
            SceneManager.LoadScene(0);
        }
    }
}
