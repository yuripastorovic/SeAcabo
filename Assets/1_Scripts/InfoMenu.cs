using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InfoMenu : MonoBehaviour
{
    [SerializeField] public GameObject atras;

    [SerializeField] public Text title;
    [SerializeField] public Text body;
    private int stage;
    void Start()
    {
        // Desactivamos el boton "Atras"
        atras.SetActive(false);
        // Encontramos en que ronda estamos y creamos la escena
        CreateScene();

    }
    public void OnBack()
    {
        SceneManager.LoadScene(0);
    }
    public void OnPlay()
    {
        // Pasamos a la siguiente escena
        EndScene();
    }
    private void CreateScene()
    {
        stage = PlayerPrefs.GetInt("ronda");
        if (stage == 0)
        {
            atras.SetActive(true);
            title.text = PlayerPrefs.GetString("team1") + "\nVS\n" + PlayerPrefs.GetString("team2");
            body.text  = "Vamos a jugar con las mismas 40 cartas 3 rondas.\n\nTrata de recordar las palabras de rondas anteriores, ya que las mismas cartas se usan en todas las rondas.\n\n\n¿Estás listo?";
        }
        else if (stage == 1)
        {
            title.text = "Ronda 1\nDescripción";
            body.text  = "Un miembro del equipo tiene 30 segundos para describir los nombres de las cartas a su equipo sin decir el nombre exacto, traducirlo, usar abreviaturas, ni hacer gestos.\n\nEl jugador podrá pasar de carta, pero perderá 15 segundos.";
        }
        else if (stage == 2)
        {
            title.text = "Ronda 2\nPassword";
            body.text  = "El jugador debe describir los nombres de las cartas durante 40 con una sola palabra.\n\nRecuerda utilizar una sola palabra\nPuedes pasar de carta sin penalizacion.";
        }
        else if (stage == 3)
        {
            title.text = "Ronda 3\nMímica";
            body.text  = "El jugador debe describir los nombres de las cartas durante 60 solo haciendo gestos.\n\n\nPoneros deacuerdo sobre cantar o tararear, puede hacer el juego mas divertido.\n\nPuedes pasar de carta sin penalizacion.";
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
    private void EndScene()
    {
        // stage = ronda
        // scene = escena
        AudioManager.Instance.Stop();
        int scene;
        if (stage == 0)
        {
            scene = 1;
            stage++;
        }
        else
        {
            scene = 2;
        }
        PlayerPrefs.SetInt("ronda", stage);
        PlayerPrefs.SetInt("turno", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(scene);
    }
}
