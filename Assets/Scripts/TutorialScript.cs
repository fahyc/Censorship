using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {

    public Button promptButton;
    public Text promptText;
    string text;
    int promptNum = 0;

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
        promptButton.GetComponent<RectTransform>().sizeDelta = new Vector2(promptText.preferredWidth, promptText.preferredHeight);
        switch (promptNum)
        {
            case 1:
                text = "Select a large node whose idea is the same as yours and choose the \"place office\" action.\n\n" +
                    "Placing an office will allow you to be able to purchase/hire other units.\n\n" +
                    "The large node and the nodes surrounding it represents a cluster.\n" +
                    "The large node is influenced by the ideas in the cluster and will take on the most prevalant idea.";
                openPrompt();
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
        promptNum++;
        if(promptNum > 10)
        {
            TeamLobbyManager._singleton.StopHost();
        }
    }

    void openPrompt()
    {
        promptButton.GetComponent<RectTransform>().sizeDelta = new Vector2(promptText.preferredWidth, promptText.preferredHeight);
        promptButton.gameObject.SetActive(true);
    }

    void introduceOffice()
    {
        text = "Office\n" +
            "\tAllows you to purchase/hire other units.\n" +
            "\tHas a bar above it that indicates its health.\n" +
            "\tLooses health when ever a different idea becomes the majority it's cluster.\n" +
            "\tCannot move and does not require upkeep.";
        openPrompt();
    }
    
    void introduceLurkers()
    {
        text = "Lurkers\n" +
            "\tLift the fog of war and provide permanent vision.\n" +
            "\tCan only be placed around the office hiring it.\n" +
            "\tCannot be fired nor destroyed by the enemey.\n" +
            "\tCannot move and does not require upkeep.";
        openPrompt();
    }

    void introduceShills()
    {
        text = "Shills\n" +
            "\tPeriodically send out a specific idea to any nodes its connected to.\n" +
            "\tCan only be placed around the office hiring it.\n" +
            "\tCan be fired or destroyed by enemy hackers and botnets.\n" +
            "\tCannot move and does not require upkeep.";
        openPrompt();
    }

    void introduceWalls()
    {
        text = "Walls\n" +
            "\tBlock out ideas that are the same color as the wall.\n" +
            "\tCan only be placed around the office hiring it.\n" +
            "\tCan be fired or destroyed by enemy hackers and botnets.\n" +
            "\tCan move and requires upkeep.";
        openPrompt();
    }

    void introduceInvestigators()
    {
        text = "Investigators\n" +
            "\tProvides vision in a small area around it and reveals hidden enemy units.\n" +
            "\tCan only be placed around the office hiring it.\n" +
            "\tCan be fired or destroyed by enemy hackers and botnets.\n" +
            "\tCan move and requires upkeep.";
        openPrompt();
    }

    void introduceHackers()
    {
        text = "Hackers\n" +
            "\tAttacks and destroys visible enemy units.\n" +
            "\tCan only be placed around the office hiring it.\n" +
            "\tCan be fired or destroyed by enemy hackers and botnets.\n" +
            "\tCan move and requires upkeep.";
        openPrompt();
    }

    void introduceBotnets()
    {
        text = "Botnet\n" +
            "\tAttacks and destroys visible enemy units.\n" +
            "\tCan only be placed around the office hiring it.\n" +
            "\tCan be fired or destroyed by enemy hackers and botnets.\n" +
            "\tCannot move and does not requires upkeep.";
        openPrompt();
    }

    void introduceFirewalls()
    {
        text = "Firewalls\n" +
            "\tProvides vision in a large area around it and reveals hidden enemy units.\n" +
            "\tCan only be placed around the office hiring it.\n" +
            "\tCan be fired or destroyed by enemy hackers and botnets.\n" +
            "\tCannot move and does not requires upkeep.";
        openPrompt();
    }

    void closeTutorial()
    {
        text = "Click here to exit the tutorial.\n";
        openPrompt();
    }
}
