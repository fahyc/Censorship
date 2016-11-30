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
        text = "Select a large nodes whose idea is the same as yours and choose the \"place office\" action.\n\n" +
            "Placing an office will allow you to be able to purchase/hire other units.\n\n" +
            "The ideas of large nodes are influenced by the nodes surrounding it; it will take on the most prevalant idea.";
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
        text = "Select an office node whose idea is the same as yours and choose the \"place office\" action.\n";
        openPrompt();
    }
}
