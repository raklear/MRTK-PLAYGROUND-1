using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum GameStatus
{
    RED_TURN,
    BLUE_TURN,
    RED_REPEAT,// this status is for when the red player has eaten and MUST eat again
    BLUE_REPEAT,
    RED_WON,
    BLUE_WON

}
public class CheckersManager : MonoBehaviour
{
    public GameObject pawnRedPrefab;
    public GameObject pawnBluePrefab;
    public GameObject visualBoard;
    public TMP_Text winText;

    public GameStatus GameStatus { get => gameStatus; 
        set 
        { 
            gameStatus = value; 
            if(gameStatus == GameStatus.BLUE_WON)
            {
                winText.gameObject.SetActive(true);
                winText.text = "BLUE WON!";
            }
            else if (gameStatus == GameStatus.RED_WON)
            {
                winText.gameObject.SetActive(true);
                winText.text = "RED WON!";
            }
        }
    }
    public GameObject[,] BoardMatrix { get => boardMatrix; set => boardMatrix = value; }
    public PawnScript Repeater { get => repeater; set => repeater = value; }
    public int RedCounter { get => redCounter; set => redCounter = value; }
    public int BlueCounter { get => blueCounter; set => blueCounter = value; }

    private GameStatus gameStatus;
    private GameObject[,] boardMatrix;
    private int redCounter = 12;
    private int blueCounter = 12;
    private PawnScript repeater; // this is a reference to the gameobject that is allowed to repeat eat
    // Start is called before the first frame update
    void Start()
    {
        gameStatus = GameStatus.BLUE_TURN;
        BoardMatrix = new GameObject[8, 8];
        InitBoard();
    }

    private void InitBoard()
    {
        InitBlue();
        InitRed();
    }



    private void InitRed()
    {
        // position of the red left bottom corner
        float xPos = 0.15f;
        float zPos = 0.55f;

        // initialize the first line
        InitLine(xPos, zPos, pawnRedPrefab, PawnType.RED_PAWN);

        // initialize the first line
        InitLine(xPos - 0.1f, zPos + 0.1f, pawnRedPrefab, PawnType.RED_PAWN);

        // initialize the first line
        InitLine(xPos, zPos + 0.2f, pawnRedPrefab, PawnType.RED_PAWN);
    }

    private void InitBlue()
    {
        // position of the bottom left corner
        float xPos = 0.05f;
        float zPos = 0.05f;

        // initialize the first line
        InitLine(xPos, zPos, pawnBluePrefab, PawnType.BLUE_PAWN);

        // initialize the first line
        InitLine(xPos + 0.1f, zPos + 0.1f, pawnBluePrefab, PawnType.BLUE_PAWN);

        // initialize the first line
        InitLine(xPos, zPos + 0.2f, pawnBluePrefab, PawnType.BLUE_PAWN);

    }

    private void InitLine(float xPos, float zPos, GameObject prefab, PawnType type)
    {
        GameObject temp;

        for (int i = 0; i < 4; i++)
        {
            // create the pawn
            temp = Instantiate(prefab, visualBoard.transform);
            // update pawn fields with  useful links
            PawnScript pawnScript = temp.GetComponent<PawnScript>();
            // give the pawn ref to this script
            pawnScript.CheckersManager = this;
            // give ref to board matrix
            pawnScript.BoardMatrix = boardMatrix;
            // set the type of the pawn
            pawnScript.PawnType = type;
            // set pawn index 
            int xIndex = convertPositionToIndex(xPos);
            pawnScript.XIndex = xIndex;
            // set pawn index 
            int zIndex = convertPositionToIndex(zPos);
            pawnScript.ZIndex = zIndex;
            // set the pawn position
            temp.transform.localPosition = new Vector3(xPos, 0.07f, zPos);
            // save pawn to board matrix
            boardMatrix[zIndex, xIndex] = temp;
            xPos += 0.2f;
        }
    }

    private int convertPositionToIndex(float pos)
    {
        pos *= 10; // convert 0.15 -> 1.5
        return (int)pos;
    }

    // decrease the value of red pawns
    public void decRed()
    {
        RedCounter--;
    }
    // decrease the value of blue pawns
    public void decBlue()
    {
        BlueCounter--;
    }
    public void SetRepeater(PawnScript pawnScript)
    {
        Repeater = pawnScript;

    }
    // if blue was playing switch to red, and vice versa
    public void SwitchPlayer()
    {
        gameStatus = (gameStatus == GameStatus.BLUE_REPEAT || gameStatus == GameStatus.BLUE_TURN) ? GameStatus.RED_TURN : GameStatus.BLUE_TURN;
    }
    public void EnableRepeat()
    {
        if (gameStatus == GameStatus.BLUE_TURN)
            gameStatus = GameStatus.BLUE_REPEAT;
        else if (gameStatus == GameStatus.RED_TURN)
            gameStatus = GameStatus.RED_REPEAT;
    }
}
