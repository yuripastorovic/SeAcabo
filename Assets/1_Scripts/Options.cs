using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]private Sprite accept;
    [SerializeField] private Sprite cancel;

    private bool anime;
    private bool futbol;
    private bool farandula;
    // Start is called before the first frame update
    void Start()
    {
        anime = true;
        futbol = true;
        farandula = true;
    }

    // Update is called once per frame
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
        PlayerPrefs.SetInt("anime", anime ? 1 : 0);
        PlayerPrefs.SetInt("futbol", futbol ? 1 : 0);
        PlayerPrefs.SetInt("farandula", farandula ? 1 : 0);

        PlayerPrefs.SetString("team1", team1.text);
        PlayerPrefs.SetString("team2", team2.text);

        PlayerPrefs.Save();
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
}
