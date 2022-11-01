using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A script to run a boot player
/// </summary>
public class BootPlayer : MonoBehaviour
{
    private static bool moved = false;
    [SerializeField] private Box[] box;
    public static Action MakeMoveAction;

    private void Start()
    {
        MakeMoveAction += MakeMove;
    }

    private void OnDestroy()
    {
        MakeMoveAction -= MakeMove;
    }
    public void MakeMove()
    {
        moved = false;
        Board.instance.SwitchPlayer();

        //Pattern
        //  0   1   2
        //  3   4   5
        //  6   7   8

        //First check if we are in danger form the enemy, by checking to each possible move
        TeamMark[] marks = Board.instance.marks;
        TeamMark mark = Board.instance.currentMark;

        //First check if we can win
        sbyte pos = Are2BoxesMatched(0, 1, 2, mark);
        //Rows
        if (pos >= 0 && !moved) //means we have got the current move to be taken
            MakeMove(pos);
        pos = Are2BoxesMatched(3, 4, 5, mark);
        if (pos >= 0 && !moved) //means we have got the current move to be taken
            MakeMove(pos);
        pos = Are2BoxesMatched(6, 7, 8, mark);
        if (pos >= 0 && !moved) //means we have got the current move to be taken
            MakeMove(pos);
        //Cols
        pos = Are2BoxesMatched(0, 3, 6, mark);
        if (pos >= 0 && !moved) //means we have got the current move to be taken
            MakeMove(pos);
        pos = Are2BoxesMatched(1, 4, 7, mark);
        if (pos >= 0 && !moved) //means we have got the current move to be taken
            MakeMove(pos);
        pos = Are2BoxesMatched(2, 5, 8, mark);
        if (pos >= 0 && !moved) //means we have got the current move to be taken
            MakeMove(pos);
        //Others
        pos = Are2BoxesMatched(0, 4, 8, mark);
        if (pos >= 0 && !moved) //means we have got the current move to be taken
            MakeMove(pos);
        pos = Are2BoxesMatched(2, 4, 6, mark);
        if (pos >= 0 && !moved) //means we have got the current move to be taken
            MakeMove(pos);


        //Now don't let others win
        mark = (mark == TeamMark.O) ? TeamMark.X : TeamMark.O;
        pos = Are2BoxesMatched(0, 1, 2, mark);
        //Rows
        if (pos >= 0 && !moved) 
            MakeMove(pos);
        pos = Are2BoxesMatched(3, 4, 5, mark);
        if (pos >= 0 && !moved) 
            MakeMove(pos);
        pos = Are2BoxesMatched(6, 7, 8, mark);
        if (pos >= 0 && !moved) 
            MakeMove(pos);
        //Cols
        pos = Are2BoxesMatched(0, 3, 6, mark);
        if (pos >= 0 && !moved) 
            MakeMove(pos);
        pos = Are2BoxesMatched(1, 4, 7, mark);
        if (pos >= 0 && !moved) 
            MakeMove(pos);
        pos = Are2BoxesMatched(2, 5, 8, mark);
        if (pos >= 0 && !moved) 
            MakeMove(pos);
        //Others
        pos = Are2BoxesMatched(0, 4, 8, mark);
        if (pos >= 0 && !moved) 
            MakeMove(pos);
        pos = Are2BoxesMatched(2, 4, 6, mark);
        if (pos >= 0 && !moved) 
            MakeMove(pos);

        while (!moved)
        {
            if(marks[4] == TeamMark.None)
            {
                MakeMove(4);
                return;
            }
            sbyte random = (sbyte)UnityEngine.Random.Range(0, 9);
            if (marks[random] == TeamMark.None)
                MakeMove(random);
        }
    }
    private void MakeMove(sbyte position)
    {
        moved = true;
        TeamMark[] marks = Board.instance.marks;
        var currentMark = Board.instance.currentMark;
        marks[position] = currentMark;
        box[position].SetAsMarked(Board.instance.GetSprite(), currentMark, Board.instance.GetColor());
        Board.instance.CheckIfWin();
    }

    /// <summary>
    /// This method check if is there any chance to win on the next move.
    /// </summary>
    /// <param name="i">The first el to be checked</param>
    /// <param name="j">The second el to be checked</param>
    /// <param name="k">The third el to be checked</param>
    /// <returns>Returns the position of the el that makes the user winer</returns>
    private static sbyte Are2BoxesMatched(sbyte i, sbyte j, sbyte k, TeamMark mark)
    {
        TeamMark[] marks = Board.instance.marks;
        var currentMark = Board.instance.currentMark;
        sbyte p = -1;
        byte empty = 0;
        //First we get the enemy team

        //Second We cont the matches and mark the empty one 
        int matched = 0;
        if (marks[i] == mark)
        {
            matched++;

        }
        else if (marks[i] == TeamMark.None)
        {
            empty++;
            if (empty >= 2)
                return -1; //return there is no match
            p = i;
        }

        if (marks[j] == mark)
        {
            matched++;
        }
        else if (marks[j] == TeamMark.None)
        {
            empty++;
            if (empty >= 2)
                return -1; //return there is no match
            p = j;
        }
        if (marks[k] == mark)
        {
            matched++;
        }
        else if (marks[k] == TeamMark.None)
        {
            empty++;
            if (empty >= 2)
                return -1; //return there is no match
            p = k;
        }
        //If there are 2 matches we return the empty one, else return -1
        if (matched >= 2)
            return p;
        return -1;

    }
}
