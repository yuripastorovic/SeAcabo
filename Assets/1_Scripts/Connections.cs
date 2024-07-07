using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

public class Connections : MonoBehaviour
{        // Campo estático para almacenar la instancia única
    private static Connections _instance;

    // Propiedad para acceder a la instancia única
    public static Connections Instance
    {
        get
        {
            // Crear la instancia si aún no existe
            if (_instance == null)
            {
                // Buscar una instancia existente en la escena
                _instance = FindObjectOfType<Connections>();

                // Crear una nueva instancia si no se encontró una en la escena
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<Connections>();
                    singletonObject.name = typeof(Connections).ToString() + " (Singleton)";

                    // Asegurar que el objeto no se destruya al cambiar de escena
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }
    // Conexión a la base de datos
    private MongoClient client;
    private IMongoDatabase database;

    // Métodos para la configuración y el acceso a la base de datos
    private void Awake()
    {
        // Verificar si ya existe una instancia y destruir esta si es necesario
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Inicializar la conexión
        InitializeMongoDBConnection();
    }
    private void InitializeMongoDBConnection()
    {
        var connectionUri = "mongodb+srv://jorgepastordev:65t5WivMWkukXMMV@cards.2us78zb.mongodb.net/?retryWrites=true&w=majority&appName=Cards";
        var settings = MongoClientSettings.FromConnectionString(connectionUri);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);

        client = new MongoClient(settings);

        // Confirmar la conexión con un ping
        try
        {
            var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            Debug.Log("Pinged your deployment. You successfully connected to MongoDB!");

            // Conectar a la base de datos
            database = client.GetDatabase("Cards"); // Reemplaza 'Cards' con el nombre real de tu base de datos
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error connecting to MongoDB: {ex.Message}");
        }
    }

    public async Task GetCommon()
    {
        try
        {
            if (database == null)
            {
                Debug.LogError("Database is not initialized.");
                return;
            }

            Debug.Log("Attempting to get collection 'common'.");
            var collection = database.GetCollection<BsonDocument>("common");
            if (collection == null)
            {
                Debug.LogError("Collection 'common' not found.");
                return;
            }

            var documents = await collection.Find(new BsonDocument()).ToListAsync();

            if (documents.Count == 0)
            {
                Debug.Log("No documents found in the 'common' collection.");
            }
            else
            {
                Debug.Log($"Found {documents.Count} documents in the 'common' collection.");
                foreach (var document in documents)
                {
                    Debug.Log(document.GetValue("name").AsString);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error fetching documents from MongoDB: {ex.Message}");
        }
    }
    public void CheckConnection() 
    {
        var connectionUri = "mongodb+srv://jorgepastordev:65t5WivMWkukXMMV@cards.2us78zb.mongodb.net/?retryWrites=true&w=majority&appName=Cards";

        var settings = MongoClientSettings.FromConnectionString(connectionUri);

        // Set the ServerApi field of the settings object to set the version of the Stable API on the client
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);

        // Create a new client and connect to the server
        var client = new MongoClient(settings);

        // Send a ping to confirm a successful connection
        try
        {
            var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            Debug.Log("Pinged your deployment. You successfully connected to MongoDB!");
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
}
/*
     public void GetCommon()
    {
        var connectionUri = "mongodb+srv://jorgepastordev:65t5WivMWkukXMMV@cards.2us78zb.mongodb.net/?retryWrites=true&w=majority&appName=Cards";

        var settings = MongoClientSettings.FromConnectionString(connectionUri);

        // Set the ServerApi field of the settings object to set the version of the Stable API on the client
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);

        // Create a new client and connect to the server
        var client = new MongoClient(settings);

        // Send a ping to confirm a successful connection
        try
        {
            var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            Debug.Log("Pinged your deployment. You successfully connected to MongoDB!");

            // Conectar a la base de datos que contiene la colección 'common'
            var database = client.GetDatabase("yourDatabaseName"); // Reemplaza 'yourDatabaseName' con el nombre real de tu base de datos
            var collection = database.GetCollection<BsonDocument>("common");

            // Leer todos los registros de la colección
            var documents = collection.Find(new BsonDocument()).ToList();

            // Iterar y mostrar los documentos
            foreach (var document in documents)
            {
                Debug.Log(document.ToString());
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
 */