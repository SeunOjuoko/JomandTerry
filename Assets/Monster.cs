using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Monster : MonoBehaviour
{
    public GameObject player;
    public Transform eyes;
    public GameObject deathCam;
    public Transform camPos;

    private UnityEngine.AI.NavMeshAgent nav;
    private Animator anim;
    private string state = "idle";              //initalising Idle behaviour
    private bool alive = true;
    private float wait = 0f;
    private bool highAlert = false;             //Boolean for determining whether Jom is alert or not 
    private float alertness = 20f;              //Radius of alertness for Jom

    // Use this for initialization
    void Start()
    {
        //References the navMeshAgent from the inspector in Unity
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //sound = GetComponent<AudioSource>();
        //Uses animator to animation
        anim = GetComponent<Animator>();
        //Speed of Jom's navigation
        nav.speed = 1.2f;
        //Speed of Jom's animation
        anim.speed = 1.2f;
    }

    //Checks whether they can see the player
    public void checkSight()
    {
        if(alive)
        {
            RaycastHit rayHit;
            //Checks whether the Terry (Player) crosses the Jom's line of sight 
            if(Physics.Linecast(eyes.position,player.transform.position, out rayHit))
            {
                //If the player reaches the line of sight of Jom...
                if(rayHit.collider.gameObject.name == "player")
                {
                    //..and Jom hasn't killed Terry 
                    if (state != "kill")
                    {
                        //Then Jom will continue to chase Jom 
                        state = "chase";
                        //The naviation speed become:
                        nav.speed = 3.5f;
                        //The naviation speed become:
                        anim.speed = 3.5f;
                    }
                }
            }
        }
    }
 
    // Update is called once per frame
    void Update()
    { 
        //Checks wether Jom is alive
        if (alive)
        {
            //This tells the navigator how fast Jom is navigating
            anim.SetFloat("velocity", nav.velocity.magnitude);

            //Jom's state will be idle when Jerry is not around
            if (state== "idle")
            {
                //It would go to a random position within the plane 
                Vector3 randomPos = Random.insideUnitSphere * alertness;
                UnityEngine.AI.NavMeshHit navHit;
                //This finds a random spot based on where Jerry currently is anywhere within the plane
                UnityEngine.AI.NavMesh.SamplePosition(transform.position + randomPos, out navHit, 20f, UnityEngine.AI.NavMesh.AllAreas);
                //Navigates towards Jerry's position whenever close in range
                nav.SetDestination(navHit.position);
                //If the 
                state = "walk";

                //High alert becomes true when Jom is close to the player
                if (highAlert)
                {
                    UnityEngine.AI.NavMesh.SamplePosition(player.transform.position + randomPos, out navHit, 20f, UnityEngine.AI.NavMesh.AllAreas);
                    alertness += 5f;

                    //Anything further than the radius would cause...
                    if(alertness > 20f)
                    {
                        //High alert to become false and sets the navigation and speed to normal
                        highAlert = false;
                        nav.speed = 1.2f;
                        anim.speed = 1.2f;
                    }
                }
            }

            //While Jom is walking
            if (state == "walk")
            {
                //If Jom reaches edge that means it can't keep on walking
                if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
                {
                    state = "idle";
                }
            }
            

            if(state == "chase")
            {
                //Follows Jerrr with the new statistics
                nav.destination = player.transform.position;

                //loses sight of Terry
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance > 10f)
                {
                    state = "hunt";
                }
                //Kill Terry
                else if (nav.remainingDistance <= nav.stoppingDistance + 1f && !nav.pathPending)
                {
                    if(player.GetComponent<player>().alive)
                    {
                        state = "kill";
                        player.GetComponent<player>().alive = false;
                        player.GetComponent<PlayerController>().enabled = false;
                        deathCam.transform.position = Camera.main.transform.position;
                        deathCam.transform.rotation = Camera.main.transform.rotation;
                        Invoke("reset", 1f);
                    }
                }
                if (state == "hunt")
                {
                    //Still travels around in the area where Terry was seen
                    if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
                    {
                        //
                        state = "idle";
                        wait = 5f;
                        highAlert = true;
                        alertness = 5f;
                        //Checks whether the Terry is seen within proximity
                        checkSight();
                    }
                }
                //Kill
                if(state == "kill")
                {
                    deathCam.transform.position = Vector3.Slerp(deathCam.transform.position, camPos.position, 10f * Time.deltaTime);
                    deathCam.transform.rotation = Quaternion.Slerp(deathCam.transform.rotation, camPos.rotation, 10f * Time.deltaTime);
                    anim.speed = 1f;
                    nav.SetDestination(deathCam.transform.position);
                }
            }
        }
        
    }
    //Reset
    void reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Monster dies
    public void death()
    {
        anim.SetTrigger("dead"); 
        anim.speed = 1f;
        alive = false;
        nav.Stop();
    }
}
