using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpSpawner : MonoBehaviour
{
    //This is a Singleton of the BoidSpawner. there is only one instance 
    // of BoidSpawner, so we can store it in a static variable named s.
    static public UpSpawner U;
    static public List<UpBoid> upboids;

    // These fields allow you to adjust the spawning behavior of the boids
    [Header("Set in Inspector: Spawning")]
    public GameObject boidPrefab;
    public Transform boidAnchor;
    public int upnumBoids = 100;
    public float spawnRadius = 100f;
    public float spawnDelay = 0.1f;

    // These fields allow you to adjust the flocking behavior of the boids
    [Header("Set in Inspector: Boids")]
    public float velocity = 30f;
    public float neighborDist = 30f;
    public float collDist = 4f;
    public float velMatching = 0.25f;
    public float flockCentering = 0.2f;
    public float collAvoid = 2f;
    public float attractPull = 2f;
    public float attractPush = 2f;
    public float attractPushDist = 5f;

    void Awake()
    {
        //Set the Singleton S to be this instance of BoidSpawner
        U = this;
        //Start instantiation of the Boids
        upboids = new List<UpBoid>();
        upInstantiateBoid();
    }

    public void upInstantiateBoid()
    {
        GameObject go = Instantiate(boidPrefab);
        UpBoid c = go.GetComponent<UpBoid>();
        c.transform.SetParent(boidAnchor);
        upboids.Add(c);
        if (upboids.Count < upnumBoids)
        {
            Invoke("upInstantiateBoid", spawnDelay);
        }
    }
}
