using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playersMovement : MonoBehaviour {
    #region Fields
    //References(This)
    public Rigidbody2D thisRigidBody;
    public Transform thisTransform;
    //References(Other)
    public Transform ballTransform;
    //Players Fields
    public int playerNum;
    public bool isPlayerControlled;
    public float playersSpeed=3;
#endregion

    #region monoMethods
    // Use this for initialization
    void Start () {
        ballTransform = GameObject.FindGameObjectWithTag("Ball").GetComponent<Transform>();
        thisTransform = this.gameObject.GetComponent<Transform>();   
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void FixedUpdate()
    {
        if (isPlayerControlled)
        {
            checkForPlayerInput();
        }
        else
        {
            computerMovementSettings();
        }
    }
    #endregion
    #region methodsOfClass
    /*
     */
   public void computerMovementSettings()
    {//if ball x position is greater than topPlayer x position move topPlayer right 
        if (ballTransform.position.x > thisTransform.position.x)
        {
            movePlayerRight();
        }
        //if ball x position is less than topPlayer x position move topPlayer left 
        if (ballTransform.position.x < thisTransform.position.x)
        {
            movePlayerLeft ();
        }
    }
    /*
     */
     public void checkForPlayerInput()
    {

        if(playerNum==1)
        {
            //if this paddle is controlled by player 1
            if (Input.GetKey(KeyCode.A))
            {
                movePlayerLeft();
            }
            if (Input.GetKey(KeyCode.S))
            {
                movePlayerRight();
            }

        }
        //else this paddle is controlled by player 2
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                movePlayerLeft();
            }
            if (Input.GetKey(KeyCode.RightArrow ))
            {
                movePlayerRight();
            }

        }
    }
    /*
     */
     public void movePlayerLeft()
    {
        thisRigidBody.AddForce(Vector2.left * playersSpeed);
    }
    public void movePlayerRight()
    {
        thisRigidBody.AddForce(Vector2.right * playersSpeed);
    }
    #endregion
}
