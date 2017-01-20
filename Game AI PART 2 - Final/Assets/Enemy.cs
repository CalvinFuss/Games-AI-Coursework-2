using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{

    
    Vector3 current; // Current transform
    Vector3 target; // Target to move to
    GameObject[] waypoints; // Stores array of scene game objects with tag "Waypoints"
    List<GameObject> storeClosest = new List<GameObject>();

    public Transform enemy;
    public Transform player;
   
    
    float distances;
    public float speed;
     
    GameObject storeClosest0;
   

    //Vector3 closest;
    // Use this for initialization

    void Start()
    {
        distances = 100;
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint"); // Stores all Waypoint game objects with tag 'Waypoint'
        current = enemy.transform.position;
        // Created a variable which would take a random index value from the objects array

        foreach (GameObject points in waypoints)
        {
            float distanceSqr = (transform.position - points.transform.position).sqrMagnitude; // Gets the waypoint distances from transform 
            if (distanceSqr < 150) 
            {
                storeClosest.Add(points);// Adds all waypoints within a distance of 150 units to list
            }
        }
        foreach (GameObject point in storeClosest) // Calculates distance of each waypoint to the player
        {

            float dist = Vector3.Distance(player.transform.position, point.transform.position);

            if (dist < distances)
            {
                storeClosest0 = point;
                distances = dist;
            }
        }
        //  MyIndex = Random.Range(0, (storeClosest.Count - 1));
    }

    // Update is called once per frame
    void Update()
    {
        
        current = enemy.transform.position; // Refine variables 
        target = storeClosest0.transform.position; // Refine variables - target wyapoint position
        enemy.transform.LookAt(player);// Rotate gaameobject to look at the player
        enemy.transform.Rotate(0, 35, 0); // Correct the rotation 
        float step = speed * Time.deltaTime; // Sets the speed

       enemy.transform.position = Vector3.MoveTowards(current, target, step); // Moves enemy towards waypoints

       // current = Vector3.Lerp(current, target , Time.deltaTime * 0.5f);
        

        if (Vector3.Distance(enemy.transform.position, target) < 4f) // If on the waypoint
        {
            distances = 100;
            // Remove target game object
            storeClosest.Clear(); // Clear List

            foreach (GameObject points in waypoints)//Loops through waypoints to find the closest waypoints
            {
                float distanceSqr = (transform.position - points.transform.position).sqrMagnitude; // Gets distance values
                if (distanceSqr < 150) // Is waypoints are within a spcific distance 
                {
                    storeClosest.Add(points);// Add waypoints which are close

                }
            }

            foreach (GameObject point in storeClosest)
            {

                float dist = Vector3.Distance(player.transform.position, point.transform.position); // Calculates distances of each instance to the player

                if (dist < distances) // If distance of current instance to the player is less than that of the previous instance
                {
                    storeClosest0 = point; // target game object is updated 
                    distances = dist;       // Update minimum distance              
                }               
            }
        }
    }

  
    void OnDrawGizmosSelected()
    {
        if (storeClosest0 != null)
        {
            if (target != null)
            {
                foreach (GameObject points in storeClosest)
                {
                    Gizmos.color = new Color(1, 1, 0, 0.5F);
                    Gizmos.DrawCube(points.transform.position, new Vector3(1, 1, 1));
                }


            }


            if (target != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(storeClosest0.transform.position, enemy.transform.position);

            }

        }
    }
    


}



