using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpNeighbor : MonoBehaviour
{
    [Header("Set Dynamically")]
    public List<UpBoid> upneighbors;
    private SphereCollider coll;

    void Start()
    {
        upneighbors = new List<UpBoid>();
        coll = GetComponent<SphereCollider>();
        coll.radius = UpSpawner.U.neighborDist / 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (coll.radius != UpSpawner.U.neighborDist / 2)
        {
            coll.radius = UpSpawner.U.neighborDist / 2;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        UpBoid b = other.GetComponent<UpBoid>();
        if (b != null)
        {
            if (upneighbors.IndexOf(b) == -1)
            {
                upneighbors.Add(b);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        UpBoid b = other.GetComponent<UpBoid>();
        if (b != null)
        {
            if (upneighbors.IndexOf(b) != -1)
            {
                upneighbors.Remove(b);
            }
        }
    }


    public Vector3 upavgPos
    {
        get
        {
            Vector3 upavg = Vector3.zero;
            if (upneighbors.Count == 0) return upavg;

            for (int i = 0; i < upneighbors.Count; i++)
            {
                upavg += upneighbors[i].uppos;
            }
            upavg /= upneighbors.Count;

            return upavg;

        }
    }

    public Vector3 upavgVel
    {
        get
        {
            Vector3 upavg = Vector3.zero;
            if (upneighbors.Count == 0) return upavg;

            for (int i = 0; i < upneighbors.Count; i++)
            {
                upavg += upneighbors[i].rigid.velocity;
            }
            upavg /= upneighbors.Count;

            return upavg;
        }
    }

    public Vector3 upavgClosePos
    {
        get
        {
            Vector3 upavg = Vector3.zero;
            Vector3 delta;
            int upnearCount = 0;
            for (int i = 0; i < upneighbors.Count; i++)
            {
                delta = upneighbors[i].uppos - transform.position;
                if (delta.magnitude <= UpSpawner.U.collDist)
                {
                    upavg += upneighbors[i].uppos;
                    upnearCount++;
                }
            }

            // If there were no neighbors too close, return Vector3.zero
            if (upnearCount == 0) return upavg;

            // Otherwise, averge their locations
            upavg /= upnearCount;
            return upavg;
        }
    }
}
