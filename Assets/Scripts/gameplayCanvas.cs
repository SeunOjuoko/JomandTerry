using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameplayCanvas : MonoBehaviour
   
{
    public static gameplayCanvas instance;  //The publics static makes other scripts be reached with linking them together
    public GameObject directionalLight;
    public Monster[] monsters;              //Multiple Joms
    public Text txtCheese;
    public string cheeseString;
    public int cheeseTotal = 4;
    private int cheeseFound = 0;
    public Animator anim;

    //Enables other scripts to be accessed
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        updateCanvas();
        anim = GetComponent<Animator>();
    }

    //This updates the texts based the actions made
    public void updateCanvas()
    {
        //String is created to be added to the text 
        cheeseString = "Cheese bites " + cheeseFound.ToString() + "/" + cheeseTotal.ToString();
        txtCheese.text = cheeseString;
    }

    //When Terry finds the cheese 
    public void findCheese()
    {
        //Adds one to the cheese counter
        cheeseFound++;
        //Runs the new number through update Canvas
        updateCanvas();

        //If all te chesses are found and the amount found is equal to the Total
        if(cheeseFound >= cheeseTotal)
        {
            directionalLight.SetActive(true);
            anim.SetBool("win", true);
            //Goes through all the Joms within the scene 
            for(int n=0; n < monsters.GetLength(0); n++)
            {
                //Kills the monster throught the conditional loop 
                monsters[n].death();
            }
        }
    }

    public void win()
    {
        if (cheeseFound >= cheeseTotal)
        {
            //PlayerController.instance.dance();
            
        }
    }
}
