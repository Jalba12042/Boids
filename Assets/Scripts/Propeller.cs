using UnityEngine;

public class Propeller : MonoBehaviour
{
    // Adjust the rotation speed as needed
    public float rotationSpeed = 500f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the propeller around its local up axis
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }
}
