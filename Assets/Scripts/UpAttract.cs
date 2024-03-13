using UnityEngine;

public class UpAttract : MonoBehaviour
{
    static public Vector3 POS = Vector3.zero;

    [Header("Set in Inspector")]
    public float radius = 10.0f;
    public float xPhase = 0.5f;
    public float yPhase = 0.4f;
    public float zPhase = 0.1f;

    // Define the boundaries for the attractor's movement
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = 50f;
    public float maxY = 70f;
    public float minZ = -10f;
    public float maxZ = 10f;

    // FixedUpdate is called once per physics update (i.e. 50x/second)
    void FixedUpdate()
    {
        // Calculate the target position based on time and phases
        Vector3 tPos = Vector3.zero;
        Vector3 scale = transform.localScale;
        tPos.x = Mathf.Sin(xPhase + Time.time) * radius * scale.x;
        tPos.y = Mathf.Sin(yPhase + Time.time) * radius * scale.y;
        tPos.z = Mathf.Sin(zPhase + Time.time) * radius * scale.z;

        // Clamp the position within the defined boundaries
        tPos.x = Mathf.Clamp(tPos.x, minX, maxX);
        tPos.y = Mathf.Clamp(tPos.y, minY, maxY);
        tPos.z = Mathf.Clamp(tPos.z, minZ, maxZ);

        // Set the attractor's position
        transform.position = tPos;
        POS = tPos;
    }
}
