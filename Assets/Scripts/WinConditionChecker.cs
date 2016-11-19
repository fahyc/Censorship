using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class WinConditionChecker : NetworkBehaviour
{

    public List<int> activePlayerIdeas = new List<int>();
    public static WinConditionChecker instance;
    public bool gameOver = false;
    public int winningIdea = -1;
    public float winCondition = 0.3f;

    // Use this for initialization
    void Awake()
    {
        instance = this;
	}
	
	// Update is called once per frame
    [Server]
	void Update () {
        if (!gameOver && activePlayerIdeas.Count > 0)
        {
            
            for (int i = 0; i < activePlayerIdeas.Count; i++)
            {
                float num = IdeaList.instance.Prevalence[activePlayerIdeas[i]];
                float total = IdeaList.instance.nodeCount;
                //print("PERCENTAGE!! " + num / total);
                if (num / total > winCondition)
                {
                    //print("OVER!!!!!!!!!!!!!!!!");
                    winningIdea = activePlayerIdeas[i];
                    gameOver = true;
                }
            }
        }
	    
	}
}
