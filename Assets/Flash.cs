using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour {
    private float alpha = 0;
    private Image myImage;
    bool red = false;
	// Use this for initialization
	void Start () {
        myImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (alpha > 0)
            alpha -= Time.deltaTime * 2;
        if (alpha < 0)
            alpha = 0;
        if (red)
            myImage.color = new Color(1, 0, 0, alpha);
        else
            myImage.color = new Color(1, 1, 1, alpha);
    }

    public void Play(bool warning)
    {
        red = warning;
        alpha = 1;
    }


}
