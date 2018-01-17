using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BallScript : MonoBehaviour {
    #region Fileds
    //References (Other)
    public GameControlScript gameControlScript;
    //References (This)
    public Rigidbody2D ballRB;
    public Transform ballTransform;
    public AudioSource audioSource;
    public AudioClip beep;

    //Functional Fields
    private float TimeSinceLastPaddleContact=0;
    private float TimeSinceLastBoundaryContact = 0;
    private bool isInPlay=false;
    private Vector2 serveDirectionV2;
    private float timeUntilBallServe=1;
    private bool isCountingDowmTimeServeBall = false;
    #endregion



    #region monoMethods
    // Use this for initialization
    void Start () {
        ballTransform = this.transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (isInPlay)
        {
            CheckIfInBounds();
            CountDownTimeSinceLastBoundaryContact();
            CountDownTimeSinceLastPaddleContact();
        }
        else if(isCountingDowmTimeServeBall)
        {
            CountDownTimeUntillBallServe();
        }
        }
    //Hadles collisions logic

    void OnCollisionEnter2D(Collision2D collisionIn)
    {
        audioSource.PlayOneShot(beep);

        //if collision is with paddle,reset counter
        if(collisionIn.gameObject.tag=="Paddle")
        {
            TimeSinceLastPaddleContact = 0;
        }

        //else if ,collision is with boundary ,reset counter
        if (collisionIn.gameObject.tag == "Boundary")
        {
            TimeSinceLastBoundaryContact = 0;
        }

    }
    #endregion

    #region classMethods
    /*Counts dowm the time since the last paddle contact
     */
     public void CountDownTimeSinceLastPaddleContact()
    {
        //Increment time since last contact
        TimeSinceLastPaddleContact += Time.deltaTime;

        //if over a certain value reset the ball
        if (TimeSinceLastPaddleContact>10)
        {
            StopAndRepositionBall();
            ServeBall();
        }
    }

    /*Counts dowm the time since the last boundary contact
     */
    public void CountDownTimeSinceLastBoundaryContact()
    {
        //Increment time since last contact
        TimeSinceLastBoundaryContact += Time.deltaTime;

        //if over a certain value reset the ball
        if (TimeSinceLastBoundaryContact > 10)
        {
            StopAndRepositionBall();
            ServeBall();
        }
    }

    /*Determines direction to serve ball
     */
    public void DetermineServeDirection(int playerWhoScoredIn)
    {
        //if player 1 scored,serve to player 2
        if(playerWhoScoredIn ==1)
        {
            HandleBallServe(1);
        }
        //Else,if player 2 scored,serve to player 1
        else if(playerWhoScoredIn==2)
        {
            HandleBallServe(-1);
        }
        //Else,if first serve randomize serve direction
        else
        {
            HandleBallServe(0);
        }
    }

    /*Handle ball serve
     */
     public void HandleBallServe(int serveDirectionIn)
    {
        float serveDitectionY = 0;

        //(if serve direction == 0)- Randomize
        if(serveDirectionIn==0)
        {
            serveDitectionY = GetRandomServeDirection();
        }

        //if a value !=0 has been passed,that is the serve direction

        else
        {
            serveDitectionY = serveDirectionIn;
        }

        //Declare a float to store serve angle

        float serveAngleX = GetRandomServeAngle();

        //Create a vector 2 containing data on serve  direction and angle
        serveDirectionV2 = new Vector2(serveAngleX,serveDitectionY);

        //Start counting down time until ball is serve 
        isCountingDowmTimeServeBall = true;
        gameControlScript.serveTimer.gameObject.SetActive(true);
    }

    /*Gets randomize serve angle
     */
     public float GetRandomServeAngle()
    {
        return UnityEngine.Random.Range(0.5f, 1.5f);
    }

    /*Gets randomize serve direction
     */
     public int  GetRandomServeDirection()
    {
        int randomServeDirection=0;
        //Get number 0 or 1,serve up (P1)
        if(UnityEngine.Random.Range(0,2)==0)
        {
            randomServeDirection = 1;
        }
        //Else,number is 1,serve down (P2)
        else
        {
            randomServeDirection = -1;
        }
        return randomServeDirection;
    }

    /*Count down the time untill the ball is severd ,update UI timer and serve ball when time  has elapsed 
     */
     public  void CountDownTimeUntillBallServe()
    {
        //Reduce time until ball serve by the time since last update 
        timeUntilBallServe -= Time.deltaTime;

        //if <0,stop counting down,reset time,hide UI tiemr,serve the ball
        if(timeUntilBallServe<=0)
        {
            //stop counting down
            isCountingDowmTimeServeBall = false;
            //Reset timer
            timeUntilBallServe = 1;
            //hide UI text
            gameControlScript.serveTimer.gameObject.SetActive(false);
            //Serve the ball
            ServeBall();

        }

        //if >0,update timer
        else
        {
            gameControlScript.UpdateServeTimerText(timeUntilBallServe.ToString("F1"));
        }

    }

    /*  Serves the ball
     */
    public void ServeBall()
    {
        ballRB.AddForce(serveDirectionV2, ForceMode2D.Impulse );
        isInPlay = true;

        //Reset the time since last paddle and boundary contact
        TimeSinceLastBoundaryContact = 0;
        TimeSinceLastPaddleContact = 0;
    } 
    /*Check once per frame if the is still within the play ares
     */
    public void CheckIfInBounds()
    {
        // if ball is < / > 5 is out of play
        if(ballTransform.position.y<-8.2||ballTransform.position.y>8.2)
        {
            //Set ball out of play
            isInPlay = false;

            //Var to record whick player is has scored
            int playerNumberWhoScored = 0;

            // if transform < 0 player2 is scored
            if(ballTransform.position.y<0)
            {
                playerNumberWhoScored = 1;
            }
            //Else player1 has scored
            else
            {
                playerNumberWhoScored = 2;
            }
            //Increase score in game control

            gameControlScript.HandlePlayerScore(playerNumberWhoScored);
            //stop and reposition the ball
            StopAndRepositionBall();

            //Only serve the ball if game has not been won
            if(gameControlScript.IsGameWon==false)
            {
                DetermineServeDirection(playerNumberWhoScored);
            }
           
        }
    }


    /*Stops the ball,returns it to origin,serves
     */
     public void StopAndRepositionBall()
    {
        ballRB.velocity = Vector2.zero;// same as new Vector2(0,0)
        ballTransform.position = Vector2.zero;

    }
    #endregion
}
