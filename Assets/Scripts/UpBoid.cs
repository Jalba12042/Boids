using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpBoid : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Rigidbody rigid;
    private UpNeighbor upneighborhood;

    // Use this for initialization
    void Awake()
    {
        upneighborhood = GetComponent<UpNeighbor>();
        rigid = GetComponent<Rigidbody>();

        //Set a random initial position
        uppos = Random.insideUnitSphere * UpSpawner.U.spawnRadius;

        //Set a random initial velocity
        Vector3 vel = Random.onUnitSphere * UpSpawner.U.velocity;

        rigid.velocity = vel;

        LookAhead();

        //Give the Boid a random color, but make sure it's not too dark
        Color redColor = Color.red;
        Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
        {
            r.material.color = redColor;
        }
        TrailRenderer tRend = GetComponent<TrailRenderer>();
        tRend.material.SetColor("_TintColor", redColor);
    }


    void LookAhead()
    {
        //Orients the Boid to look at the direction it's flying
        transform.LookAt(uppos + rigid.velocity);
    }

    public Vector3 uppos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    //FixedUpdate is called one per physics update (i.e. 50x/second)
    private void FixedUpdate()
    {
        Vector3 vel = rigid.velocity;
        UpSpawner uspn = UpSpawner.U;

        //Collision Avoidance - avoid neigbors who are too close
        Vector3 upvelAvoid = Vector3.zero;
        Vector3 UptooClosePos = upneighborhood.upavgClosePos;
        // If the response is Vector3.zero, then no need to react
        if (UptooClosePos != Vector3.zero)
        {
            upvelAvoid = uppos - UptooClosePos;
            upvelAvoid.Normalize();
            upvelAvoid *= uspn.velocity;
        }

        //Velocity matching - Try to match velocity with neigbors
        Vector3 upvelAlign = upneighborhood.upavgVel;
        // Only do more if the velAlign is not Vector3.zero
        if (upvelAlign != Vector3.zero)
        {
            // we're really interested in direction, so normalize the velocity
            upvelAlign.Normalize();
            // and then set it to the speeed we chose
            upvelAlign *= uspn.velocity;
        }

        //Flock centering - move towards the center of local neighbors
        Vector3 velCenter = upneighborhood.upavgPos;
        if (velCenter != Vector3.zero)
        {
            velCenter -= transform.position;
            velCenter.Normalize();
            velCenter *= uspn.velocity;
        }

        //ATTRACTION - Move towards the Atttractor
        Vector3 delta = UpAttract.POS - uppos;
        //Check whether we're attracted or avoiding the Attractor
        bool attracted = (delta.magnitude > uspn.attractPushDist);
        Vector3 velAttract = delta.normalized * uspn.velocity;

        //Apply all the velocities
        float fdt = Time.fixedDeltaTime;
        if (upvelAvoid != Vector3.zero)
        {
            vel = Vector3.Lerp(vel, upvelAvoid, uspn.collAvoid);
        }
        else
        {
            if (upvelAlign != Vector3.zero)
            {
                vel = Vector3.Lerp(vel, upvelAlign, uspn.velMatching * fdt);
            }
            if (velCenter != Vector3.zero)
            {
                vel = Vector3.Lerp(vel, upvelAlign, uspn.flockCentering * fdt);
            }
            if (velAttract != Vector3.zero)
            {
                if (attracted)
                {
                    vel = Vector3.Lerp(vel, velAttract, uspn.attractPull * fdt);
                }
                else
                {
                    vel = Vector3.Lerp(vel, -velAttract, uspn.attractPush * fdt);
                }
            }
        }

        //set vel to the velocity set on the spawner singleton
        vel = vel.normalized * uspn.velocity;
        // Finally assign this to the Rigidbody
        rigid.velocity = vel;
        //Lock in the direction of the new velocity
        LookAhead();
    }
}