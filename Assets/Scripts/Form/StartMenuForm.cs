using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuForm : MonoBehaviour {

    public Text usernameLabel;

	// Use this for initialization
	void Start () {
        if (ServerConnection.session == null)
        {
            FindObjectOfType<LevelManager>().LoadLevel("Login");
            return;
        }
        usernameLabel.text = ServerConnection.session.user.name;
	}
}
