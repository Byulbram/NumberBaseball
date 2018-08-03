using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultText : MonoBehaviour {
    main myMain;
    Text myText;
	// Use this for initialization
	void Start () {
        myMain = GameObject.Find("Main").GetComponent<main>();
        myText = GetComponent<Text>();
        myText.text = "#" + myMain.tryNumber + ": " +  myMain.PrevNumber.text + " | " + myMain.Strike + "S " + myMain.Ball + "B";
    }

    void Update()
    {
        if (myMain.gameMode == main.eGameMode.Clear)
        {
            Destroy(gameObject);
        }
    }


}
