using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControlScript : MonoBehaviour
{
    #region Fileds
    //References(Other)
    public BallScript ballScript;
    public playersMovement PlayersMovement1Script;
    public playersMovement PlayersMovement2Script;

    //Ui Elements
    public Text scoreDisplayText;
    public Text victoryMessageText;
    public GameObject endGameOptionsPanel;
    public Text pointsToWin;
    public GameObject menuPanel;
    public Text numberOfPlayersText;
    public Text serveTimer;     

    //Functional fields
    public int numberOfPlayers=1;

    //Score Fields
    public bool IsGameWon=false;
    public int scoreToWin = 3;
    public int player1Score = 0;
    public int player2Score = 0;
    #endregion
    // Use this for initialization
    #region MonoMethods

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

     #region Class Methods
    /*Starts the game from the menu
     */
     public void StartGame()
    {
        //Disable the victory options panel
        endGameOptionsPanel.SetActive(false);


        //Disable the menu panel
        menuPanel.SetActive(false);

        //Serve the ball
        RestartGame();
    }

    /*Restart the game using the current settings
     */
    public void RestartGame()
    {
        //Reset the score & isgamewon bool
        player1Score = 0;
        player2Score = 0;
        IsGameWon = false;
        UpdateScoreDisplay();

        //Reposition paddles
        PlayersMovement2Script.transform.position = new Vector3( 0, -8.1f, 0);
        PlayersMovement1Script.transform.position = new Vector3( 0, 8.1f, 0);

        //Reset the victory message & update the score display
        victoryMessageText.text = "  ";
        victoryMessageText.enabled = false;
        //Hide the end game option panel
        endGameOptionsPanel.SetActive(false);
        //Serve the ball tp random player
        ballScript.HandleBallServe(0);
    }

    /*Handles logic for a player score
     */
    public void HandlePlayerScore(int playerScoredNumberIn)
    {
        //if p1 scored increase player1 score
        if (playerScoredNumberIn == 1)
        {
            player1Score++;
        }
        //Else increase p2 score
        else
        {
            player2Score++;
        }

        UpdateScoreDisplay();
        // Check if either players score is high enough for victory
        checkIfGameWon();
    }


    /*Check if either player's score >=scoreToWin;
     */
     public void checkIfGameWon()
    {
        // if p1 greater than scoretowin
        if(player1Score>=scoreToWin)
        {
            IsGameWon = true;
            HandleEndGame(1);
        }
        //if p2 greater than scoretowin
        else if (player2Score >= scoreToWin)
        {
            IsGameWon = true;
            HandleEndGame(2);
        }
    }

    /*Handles end of game logic
     */
        public void HandleEndGame(int playerNowWhoWonIn)
    {

        //Enable the end game option panel 
        endGameOptionsPanel.SetActive(true);


        //Display player number who won
        ShowVictoryMessge(playerNowWhoWonIn);
    }

    /*show message annocing which player has won
     */
     public void ShowVictoryMessge(int playerNowWhoWonIn)
    {
        //Declare string to hold the message
        string VictoryMessage = "Игрок ";

        //if p1 Won

        //else p1 Won
        if(playerNowWhoWonIn==1)
        {
            VictoryMessage += "1 Выигрывает";
        }
        else
        {
            VictoryMessage += "2 Выигрывает";
        }

        //Set the string in UI text element
        victoryMessageText.text = VictoryMessage;
    }
    /*Update Score Display
     */

    public void UpdateScoreDisplay()
    {
        //Update the text
        scoreDisplayText.text = player1Score + ":" + player2Score;
    }

    /*Update serve timer text
     */
     public void UpdateServeTimerText(string serveTimerTextIn)
    {
        serveTimer.text = serveTimerTextIn;
    } 

    #endregion

    #region Main Menu Methods
    /*Switch on menu panel
     */
     public void ReturnToMenu()
    {
        menuPanel.SetActive(true);
    }


    /*Increases the number of points required to win the game
     */
     public void IncreasePointsToWinGame()
    {
        //Increase score to win
        scoreToWin++;

        //Reduce score win to 1 ,if >10
        if(scoreToWin>10)
        {
            scoreToWin = 1;
        }

        //Set the points to win text
        pointsToWin.text = "Очки:" + scoreToWin;
    }


    /*Toggles number of human players
     */
     public void ToggleNumberOfHumanPlayers()
    {

        //if currently one human player,set both players as human
        if(numberOfPlayers==1)
        {
            //Set number if human players
            numberOfPlayers = 2;

            //set to player controlled 
            PlayersMovement1Script.isPlayerControlled = true;
        }

        //else both players are human,set player 2 to computer controlled
        else
        {

            //Set number if human players
            numberOfPlayers = 1;

            //set to player controlled 
            PlayersMovement1Script.isPlayerControlled = false;
        }
        //Update the text ont he button
        numberOfPlayersText.text = "Игроки: " + numberOfPlayers;
    }

    /*Quits the application
     */
        public void QuitApp()
        {
            Application.Quit();
        }
    #endregion
}
