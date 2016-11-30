using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {

    Button promptButton;
    Text promptText;
    string text;

	// Use this for initialization
	void Start () {
        promptButton = GetComponent<Button>();
        promptText = promptButton.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        //TODO: create buttons/text to explain the gui features
        //TODO: create central button/text listing mouse/keyboard controls
        //TODO: explain unit movement, firing, and fog of war
        promptText.text = text;
        text = "Select a large node whose idea is the same as yours and choose the \"place office\" action.\n\n" +
            "Placing an office will allow you to be able to purchase/hire other units.\n\n" +
            "The large node and the nodes surrounding it represents a cluster.\n" +
            "The large node is influenced by the ideas in the cluster and will take on the most prevalant idea.";
        //introduce office
        //introduce lurkers
        //introduce shills
        //introduce walls
        //introduce investigators
        //introduce hackers
    }

    void closePrompt()
    {
        promptButton.gameObject.SetActive(false);
    }

    void openPrompt()
    {
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
            "\tCan only be placed around the office hiring it." +
            "\tCan be fired or destroyed by enemy hackers and botnets.\n" +
            "\tCannot move and does not require upkeep.";
        openPrompt();
    }
}
