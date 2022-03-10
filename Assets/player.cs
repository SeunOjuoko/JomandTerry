using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public static player instance;
    public bool alive = true;
    private Animator anim;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "eyes")
        {
            //Goes through the parent of eyes in Monster to access the checkSight method
            other.transform.parent.GetComponent<Monster>().checkSight();
        }
        else if(other.CompareTag("Cheese"))
        {
            //Cheese will disappar once player touches
            Destroy(other.gameObject);
            //Triggers the find page method in gameplayCanvas 
            gameplayCanvas.instance.findCheese();
            //anim.speed = 1f;
            //anim.SetTrigger("win");
        }
    }
}
