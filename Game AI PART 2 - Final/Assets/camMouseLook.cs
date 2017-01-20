using UnityEngine;
using System.Collections;

public class camMouseLook : MonoBehaviour
{
    Vector2 mouseLook;
    Vector2 smoothV;

    public float sensitivity = 5.0f; // Sets the sensitivity of the mouse
    public float smoothing = 2.0f; // Ensures the looking isn't jittery 

    GameObject character;

    // Use this for initialization
    void Start()
    {
        character = this.transform.parent.gameObject; // Defines game object variable (Camera)
    }

    // Update is called once per frame
    void Update()
    {
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")); // Gets the position of the XY axis of the mouse

        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing)); // defines movement speed on the XY axis

        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing); // Implements the smoothing of the mouse
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);

        mouseLook += smoothV;


        mouseLook.y = Mathf.Clamp(mouseLook.y, -70f, 75f); // Sets limits of movement
        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right); // Defines the left/right movement
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up); // Defines up/ down movement

    }
}
