using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MapsGenerator : MonoBehaviour
{

    public Transform[] points;
    public Transform playerPosition;

    public TextMeshProUGUI distanceDisplay;


    public int rockAmounts = 50;
    public int targetAmount = 2;
    public GameObject[] rocks;
    public GameObject target;

    private Boolean generated = false;

    

    // Start is called before the first frame update
    void Start()
    {
        GenerateMaps();
        GenerateTarget();
        generated = true;
        
    }

    private void Update()
    {
        if (generated)
        {
            GameObject target = GameObject.Find("Targets(Clone)");
            if(target != null)
            {

                float distance = (target.transform.position - playerPosition.position).magnitude;
                distanceDisplay.text = "Distance to a target: " + distance.ToString();
            }

        }
    }

    private void GenerateMaps()
    {
        // Determine the boundaries of the map from the points
        float minX = Mathf.Min(points[0].position.x, points[1].position.x, points[2].position.x);
        float maxX = Mathf.Max(points[0].position.x, points[1].position.x, points[2].position.x);
        float minZ = Mathf.Min(points[0].position.z, points[1].position.z, points[2].position.z);
        float maxZ = Mathf.Max(points[0].position.z, points[1].position.z, points[2].position.z);
        float minY = Mathf.Min(points[0].position.y, points[1].position.y, points[2].position.y);
        float maxY = Mathf.Max(points[0].position.y, points[1].position.y, points[2].position.y);



        // Loop through the number of rocks to generate
        for (int i = 0; i < rockAmounts; i++)
        {
            // Generate a random position within the map boundaries
            Vector3 randomPosition = new Vector3(
                UnityEngine.Random.Range(minX, maxX),      // Random x within limits
                UnityEngine.Random.Range(minY, maxY),      // Random y
                UnityEngine.Random.Range(minZ, maxZ)      // Random z within limits
            ); ;

           
            if(randomPosition.magnitude < 20)
            {
                continue;
            }

            // Randomly select a rock prefab
            GameObject randomRock = rocks[UnityEngine.Random.Range(0, rocks.Length)];

            // Instantiate the rock at the random position
            Instantiate(randomRock, randomPosition, Quaternion.identity);
        }
    }

    private void GenerateTarget()
    {
        float minX = Mathf.Min(points[0].position.x, points[1].position.x, points[2].position.x);
        float maxX = Mathf.Max(points[0].position.x, points[1].position.x, points[2].position.x);
        float minZ = Mathf.Min(points[0].position.z, points[1].position.z, points[2].position.z);
        float maxZ = Mathf.Max(points[0].position.z, points[1].position.z, points[2].position.z);
        float minY = Mathf.Min(points[0].position.y, points[1].position.y, points[2].position.y);
        float maxY = Mathf.Max(points[0].position.y, points[1].position.y, points[2].position.y);


        
        for (int i = 0; i < targetAmount; i++)
        {
            // Generate a random position within the map boundaries
            Vector3 randomPosition = new Vector3(
                UnityEngine.Random.Range(minX, maxX),      // Random x within limits
                UnityEngine.Random.Range(minY, maxY),      // Random y
                UnityEngine.Random.Range(minZ, maxZ)      // Random z within limits
            ); ;


            if (randomPosition.magnitude < 100)
            {
                i--;
                continue;
            }

            if(randomPosition.magnitude > 200)
            {
                i--;
                continue;
            }

            // Instantiate the rock at the random position
            Instantiate(target, randomPosition, Quaternion.identity);
        }

    }
}
