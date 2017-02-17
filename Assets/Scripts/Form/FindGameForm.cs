using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindGameForm : MonoBehaviour {

    DateTime start = DateTime.Now;
    public Text lblTimer;

	// Use this for initialization
	void Start () {
        if (ServerConnection.session == null)
        {
            FindObjectOfType<LevelManager>().LoadLevel("Login");
            return;
        }
        ServerConnection.FindGame(this, (error, data) =>
        {
            if (error != null)
            {
                FindObjectOfType<LevelManager>().LoadLevel("Start");
                return;
            }
            if (data != null)
            {

            }
        });
    }
	
	// Update is called once per frame
	void Update () {
        lblTimer.text = ((int)(DateTime.Now - start).TotalSeconds).ToString();
	}

    public void CancelQueue()
    {
        ServerConnection.CancelQueue(this, (error, data) =>
        {
            if (error != null)
            {
                Debug.LogError(error);
            }

            FindObjectOfType<LevelManager>().LoadLevel("Start");

        });
    }
}
