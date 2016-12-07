using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {

    public Button promptButton;
    public Text promptText;
    string text;
    public int promptNum = 0;

	// Use this for initialization
	void Start () {
        //promptButton = GetComponent<Button>();
        //promptText = promptButton.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        //TODO: create buttons/text to explain the gui features
        //TODO: create central button/text listing mouse/keyboard controls
        //TODO: explain unit movement, firing, and fog of war
        promptText.text = text;
        text = "Click on a prompt or information box to close it.";
        //promptButton.GetComponent<RectTransform>().sizeDelta = new Vector2(promptText.preferredWidth, promptText.preferredHeight);
        switch (promptNum)
        {
            case 1:
                text = "Select an office slot whose idea is the same as yours and choose the \"place office\" action.\n\n" +
                    "Placing an office will allow you to be able to purchase/hire other units.\n\n" +
                    "The large node and the nodes surrounding it represents a cluster.\n" +
                    "The large node is influenced by the ideas in the cluster and will take on the most prevalent idea.";
                //openPrompt();
                break;
            case 2:
                introduceOffice();
                break;
            case 3:
                introduceLurkers();
                break;
            case 4:
                introduceShills();
                break;
            case 5:
                introduceWalls();
                break;
            case 6:
                introduceInvestigators();
                break;
            case 7:
                introduceHackers();
                break;
            case 8:
                introduceBotnets();
                break;
            case 9:
                introduceFirewalls();
                break;
            case 10:
                closeTutorial();
                break;
        }
        //introduce office
        //introduce lurkers
        //introduce shills
        //introduce walls
        //introduce investigators
        //introduce hackers
        //introduce botnet
        //introduce firewall
    }

    public void closePrompt()
    {
        promptButton.gameObject.SetActive(false);
        if(promptNum == 0)
        {
            promptNum++;
            openPrompt();
        }
        //promptNum++;
        if(promptNum >= 10)
        {
            TeamLobbyManager._singleton.StopHost();
        }
    }

    public void openPrompt()
    {
        //promptButton.GetComponent<RectTransform>().sizeDelta = new Vector2(promptText.preferredWidth, promptText.preferredHeight);
        promptButton.gameObject.SetActive(true);
    }

    void introduceOffice()
    {
        text = "Office\n" +
            "\tAllows you to purchase/hire other units.\n" +
            "\tHas a bar above it that indicates its health.\n" +
            "\tLoses health whenever a different idea becomes the majority its cluster.\n" +
            "\tOffices cannot move and does not require upkeep.";
        //openPrompt();
    }
    
    void introduceLurkers()
    {
        text = 
            "\tLurkers lift the fog of war and provide permanent vision.\n" +
            "\tThey can only be placed around the office hiring it.\n" +
            "\tLurkers cannot be fired nor destroyed by the enemey.\n" +
            "\tThey cannot move and do not require upkeep.\n\n" +
            "Place a lurker anywhere around your office.";
        //openPrompt();
    }

    void introduceShills()
    {
        text = "Shills:\n" +
            "\t * Periodically send out a specific idea to any nodes its connected to.\n" +
            "\t * Can only be placed around the office hiring it.\n" +
            "\t * Can be fired or destroyed by enemy hackers and botnets.\n" +
            "\t * Cannot move and does not require upkeep.\n\n" +
            "Place a shill in the spot marked by the box.";
        //openPrompt();
    }

    void introduceWalls()
    {
        text = "Walls:\n" +
            "\tBlock out ideas that are the same color as the wall.\n" +
            "\tCan only be placed around the office hiring it.\n" +
            "\tCan be fired or destroyed by enemy hackers and botnets.\n" +
            "\tCan move and requires upkeep.\n\n" +
            "Place a wall in the spot marked by the box.";
        //openPrompt();
    }

    void introduceInvestigators()
    {
        text = "Investigator:\n" +
            "\tProvides vision in a small area around it and reveals hidden enemy units.\n" +
            "\tCan only be placed around the office hiring it.\n" +
            "\tCan be fired or destroyed by enemy hackers and botnets.\n" +
            "\tCan move and requires upkeep.\n\n" +
            "Place an investigator in the spot marked by the box.";
        //openPrompt();
    }

    void introduceHackers()
    {
        text = "Hackers:\n" +
            "\tAttack and destroy visible enemy units.\n" +
            "\tCan only be placed around the office hiring them.\n" +
            "\tCan be fired or destroyed by enemy hackers and botnets.\n" +
            "\tCan move and requires upkeep.\n\n" +
            "Place an hacker in the spot marked by the box.";
        //openPrompt();
    }

    void introduceBotnets()
    {
        text = "Botnet:\n" +
            "\tAttacks and destroys visible enemy units.\n" +
            "\tCan only be placed around the office hiring it.\n" +
            "\tCan be fired or destroyed by enemy hackers and botnets.\n" +
            "\tCannot move and does not requires upkeep.\n\n" +
            "Place a botnet in the spot marked by the box.";
        //openPrompt();
    }

    void introduceFirewalls()
    {
        text = "Firewalls:\n" +
            "\tProvide vision in a large area and reveal hidden enemy units.\n" +
            "\tCan only be placed around the office hiring it.\n" +
            "\tCan be fired or destroyed by enemy hackers and botnets.\n" +
            "\tCannot move and do not requires upkeep.\n\n" +
            "Place a firewall in the spot marked by the box.";
        //openPrompt();
    }

    void closeTutorial()
    {
        text = "Click here to exit the tutorial.\n";
        //openPrompt();
    }
}
