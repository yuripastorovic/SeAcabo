using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;
using MongoDB.Bson.IO;
using Newtonsoft.Json;

[System.Serializable]
public class Holder
{
    public string name;
    public string category;
    public string desc;
}
[System.Serializable]
public class CardArrayHolder
{
    public Holder[] holder;
}
public class Connections : MonoBehaviour
{
    private static Connections _instance;
    private string mongodbURL = "mongodb+srv://jorgepastordev:65t5WivMWkukXMMV@cards.2us78zb.mongodb.net/?retryWrites=true&w=majority";
    private MongoClient client;
    private IMongoDatabase db;

    public List<Card> common = new List<Card>();
    public List<Card> anime = new List<Card>();
    public List<Card> futbol = new List<Card>();
    public List<Card> farandula = new List<Card>();
    public static Connections Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Connections>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<Connections>();
                    singletonObject.name = typeof(Connections).ToString() + " (Singleton)";

                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        InitializeMongoDBConnection();
    }
    private void InitializeMongoDBConnection()
    {
        client = new MongoClient(mongodbURL);
        db = client.GetDatabase("6689d87720187f14978cf0fe");
        Options.Instance.info.text = "Conexion inicializada";
    }

    public async Task GetCards()
    {
        try
        {
            var collection = db.GetCollection<BsonDocument>("common");
            var projection = Builders<BsonDocument>.Projection.Include("name").Include("category").Include("desc");
            var documents = await collection.Find(new BsonDocument()).Project(projection).ToListAsync();
            common.Clear();
            anime.Clear();
            futbol.Clear();
            farandula.Clear();
            foreach (var document in documents)
            {
                var card = new Card
                {
                    Name = document.Contains("name") ? document["name"].AsString : "unknown",
                    Category = document.Contains("category") ? document["category"].AsString : "unknown",
                    Winner = "unknown",
                    Desc = document.Contains("desc") ? document["desc"].AsString : "unknown"
                };

                //Debug.Log($"Document: Name = {card.Name}, Category = {card.Category}");

                switch (card.Category)
                {
                    case "common":
                        common.Add(card);
                        break;
                    case "anime":
                        anime.Add(card);
                        break;
                    case "futbol":
                        futbol.Add(card);
                        break;
                    case "farandula":
                        farandula.Add(card);
                        break;
                    default:
                        Debug.LogWarning($"Unknown category: {card.Name}");
                        break;
                }
                Options.Instance.defaultMaze.Add(card);
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            Options.Instance.info.text = "Imposible conectar con el repositorio, cargando version 1 del juego";
            LoadJson();
        }
    }

    private void LoadJson()
    {
        string filePath = "common";

        TextAsset jsonFile = Resources.Load<TextAsset>(filePath);

        if (jsonFile == null)
        {
            Debug.LogError("No se pudo cargar el archivo JSON.");
            return;
        }

        string jsonString = jsonFile.text;
        ProcessJson(jsonString);
    }

    private void ProcessJson(string jsonString)
    {
        CardArrayHolder wrapper = JsonUtility.FromJson<CardArrayHolder>(jsonString);
        Holder[] holders = wrapper.holder;

        common.Clear();
        anime.Clear();
        futbol.Clear();
        farandula.Clear();

        foreach (var document in holders)
        {
            Card card = new Card
            {
                Name = document.name,
                Category = document.category,
                Winner = "unknown",
                Desc = document.desc
            };

            //Debug.Log($"Document: Name = {card.Name}, Category = {card.Category}");

            switch (card.Category)
            {
                case "common":
                    common.Add(card);
                    break;
                case "anime":
                    anime.Add(card);
                    break;
                case "futbol":
                    futbol.Add(card);
                    break;
                case "farandula":
                    farandula.Add(card);
                    break;
                default:
                    Debug.LogWarning($"Unknown category: {card.Name}");
                    break;
            }
            Options.Instance.defaultMaze.Add(card);
        }
    }

    public bool CheckConnection() 
    {
        var settings = MongoClientSettings.FromConnectionString(mongodbURL);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        var client = new MongoClient(settings);
        try
        {
            var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            Debug.Log("Pinged your deployment. You successfully connected to MongoDB!");
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log("Pinged your deployment. You unable to connected to MongoDB!"+ex);
            return false;
        }
    }
}