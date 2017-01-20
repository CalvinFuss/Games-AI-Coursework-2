using UnityEngine;
using System.Collections;

public class characterController : MonoBehaviour
{

    public float speed = 10.0f;
    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Keeps cursor in middle of screen
    }

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed; // For forward and back movement
        float straffe = Input.GetAxis("Horizontal") * speed; // For left and right movement 
        translation *= Time.deltaTime;
        straffe *= Time.deltaTime;

        transform.Translate(straffe, 0, translation);

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None; // Allows cursor to move freely
        }
    }
}
