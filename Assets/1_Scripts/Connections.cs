using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System;

public class Connections : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick() 
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
