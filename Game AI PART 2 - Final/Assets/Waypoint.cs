using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class Waypoint : MonoBehaviour
{


    public Terrain terrain; 
    public TerrainData terrainData;
    GameObject[] unwalkable; // Array with game object tagged as "Unwalkable" in scene
    GameObject[] walkable; // Array with game object tagged as "Walkable" in scene
    List<GameObject> cubePositions = new List<GameObject>(); // Stores a list of cube positions


    public int maxYPos; // User selectable Y position
    int end; // Used for modulo 
   
    
    
    

    // Use this for initialization
    void Awake() // Places the game objects before being accessed  by the 'start' functions of other classes

    {
        
        unwalkable = GameObject.FindGameObjectsWithTag("Unwalkable"); // Defines the user selectable, walkable and unwalkable areas
        walkable = GameObject.FindGameObjectsWithTag("Walkable");


        if (!terrain) // If there is no inputted, uses the terrain which the script has been assigned to 
        {
            terrain = Terrain.activeTerrain;
        }

        terrainData = terrain.terrainData; 
        cubeGeneration();
     
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) // Responsible for toggling the waypoints on and off
        {
            foreach(GameObject index in cubePositions)
            {
                index.GetComponent<MeshRenderer>().enabled = true; // Renders Object if left arrow is pressed
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            foreach (GameObject index in cubePositions)
            {
                index.GetComponent<MeshRenderer>().enabled = false;
            }
        }

    }


    void cubeGeneration()
    {

        //List<Collider> hitColliders = new List<Collider>();


        float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight); // Sets the sample parameters of the terrain

        for (int y = 0; y < terrainData.heightmapHeight; y++) // Nested loop to address all points in scene

        {
            for (int x = 0; x < terrainData.heightmapHeight; x++)
            {
                if (x % 5 == 0 && y % 5 == 0) // Calculates every 5 Units
                {

                    float actualHeight = heights[y, x] * terrainData.size.y + 1; // Gets the height from a specific X,Z position on the terrain

                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);// Creates a cube Game Object
                    cube.transform.position = new Vector3(x, actualHeight, y); // Sets each position of the cube game object. 
                                                                               // Calculates and places cube on the specified X,Y,Z position 

                    cubePositions.Add(cube); // Add the cube to a list 
                    cube.GetComponent<MeshRenderer>().enabled = false; // Disables Mesh renderer of cubes so they aren't rendered when the game starts
                    cube.GetComponent<BoxCollider>().isTrigger = true; // Sets the Box collider to 'isTrigger' so that the waypoints aren't physical objects

                    // hitColliders.Add(Physics.OverlapSphere(cube.transform.position, 3));

                }
            }
        }

        end = cubePositions.Count - 3; // Prevents Array OUt of Bounds Error

        for (int i = 0; i < cubePositions.Count; i++) // Loops through the cube Gameobject array
        {
            int a = i + 1; // index + 1
            int b = i + 2; // index +2


            cubePositions[i].transform.GetComponent<Renderer>().material.color = Color.green;

            if (cubePositions[a % end].transform.position.y + 2 < cubePositions[i].transform.position.y) // If the current instance position was 2 units higher than the next Instance's Y value
            {
                cubePositions[i].transform.GetComponent<Renderer>().material.color = Color.red; // Set it as unwalkable
            }

            if (cubePositions[a % end].transform.position.y > cubePositions[i].transform.position.y + 2) // If the next instance is 2 units higher than the current Y value
            {
                cubePositions[i].transform.GetComponent<Renderer>().material.color = Color.red;


            }
            
            if (cubePositions[b % end].transform.position.y + 2 < cubePositions[i].transform.position.y) // If the current instance position was 2 units higher than the Instance after next's Y value
            {
                cubePositions[i].transform.GetComponent<Renderer>().material.color = Color.red;
            }

            if (cubePositions[b % end].transform.position.y > cubePositions[i].transform.position.y + 2) // If the instance after next is 2 units higher than the current Y value
            {
                cubePositions[i].transform.GetComponent<Renderer>().material.color = Color.red;

            }

            

            /// HIT COLLIDER BOXES
            Collider[] hitColliders = Physics.OverlapBox(cubePositions[i].transform.position, new Vector3(10f, 0.1f, 10f), cubePositions[i].transform.rotation); // Creates an overlap box on each waypoint 

            //cubePositions[i].transform.GetComponent<Renderer>().material.color = Color.green;

            foreach (Collider hit in hitColliders)//Nested for loop to compare bounding boxes within the same array
            {

                foreach (Collider hits in hitColliders)
                {
                    if (hit.bounds.Intersects(hits.bounds) && hit != hits)// If overlap box instance intersect with another overlap box and not with with itself
                    {
                        if (cubePositions[i].transform.position.y < maxYPos)// If cube instances Y position is less than the user specified position
                        {
                            cubePositions[i].transform.GetComponent<Renderer>().material.color = Color.green; // set cube colour to green

                        }

                        else
                        {
                            cubePositions[i].transform.GetComponent<Renderer>().material.color = Color.red; // if not set cube colour to red
                        }
                    }

                }


                if (cubePositions[i].transform.position.y < Terrain.activeTerrain.SampleHeight(cubePositions[i].transform.position)) // If cube y position is below that of the terrain Y position
                                                                                                                                     
                {
                    cubePositions[i].transform.GetComponent<Renderer>().material.color = Color.red; // Set it to unwalkable. Means that all waypoints under the terrain are unwalkable
                }


            }


            for (int j = 0; j < unwalkable.Length; j++) // User defined unwalkable area under the same Y position as the bounding box
            {
                //if (unwalkable[j].GetComponent<Collider>().bounds.Contains(cubePositions[i].transform.position)
                if (unwalkable[j].GetComponent<Collider>().bounds.max.x > cubePositions[i].transform.position.x && unwalkable[j].GetComponent<Collider>().bounds.min.x < cubePositions[i].transform.position.x && unwalkable[j].GetComponent<Collider>().bounds.max.z > cubePositions[i].transform.position.z && unwalkable[j].GetComponent<Collider>().bounds.min.z < cubePositions[i].transform.position.z && unwalkable[j].GetComponent<Collider>().bounds.max.y > cubePositions[i].transform.position.y) // If distance from one waypoint to the other 
                {
                    cubePositions[i].transform.GetComponent<Renderer>().material.color = Color.red;
                }
            }

            for (int j = 0; j < walkable.Length; j++) // User defined walkable area under the same Y position as the bounding box
            {
                //if (unwalkable[j].GetComponent<Collider>().bounds.Contains(cubePositions[i].transform.position)
                if (walkable[j].GetComponent<Collider>().bounds.max.x > cubePositions[i].transform.position.x && walkable[j].GetComponent<Collider>().bounds.min.x < cubePositions[i].transform.position.x && walkable[j].GetComponent<Collider>().bounds.max.z > cubePositions[i].transform.position.z && walkable[j].GetComponent<Collider>().bounds.min.z < cubePositions[i].transform.position.z && walkable[j].GetComponent<Collider>().bounds.max.y > cubePositions[i].transform.position.y&& cubePositions[i].transform.position.y > Terrain.activeTerrain.SampleHeight(cubePositions[i].transform.position)) // If distance from one waypoint to the other 
                {
                    cubePositions[i].transform.GetComponent<Renderer>().material.color = Color.green; // If waypoint instance is under walkable tagged object, then set waypoint colour to green
                }
            }

            
            if (cubePositions[i].GetComponent<Renderer>().material.color == Color.green)
            {
                cubePositions[i].gameObject.tag = "Waypoint"; // Tag all waypoints with colour green as "Waypoint" in scene
                
            }




            

        }
       
    }


   /* void Toggle()
    {
        if (VisibleNodes == false)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }*/
        void OnDrawGizmosSelected()
    {
            for (int i = 0; i < cubePositions.Count; i++)
            {
                if (cubePositions[i].transform.position.y < maxYPos)
                {

                Gizmos.color = new Color(0, 1, 1, 0.5F);
                Gizmos.DrawCube(cubePositions[i].transform.position, new Vector3(4.5f, 0.5f, 4.5f));




                }
            }
        }
    }



