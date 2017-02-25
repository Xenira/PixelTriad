using Assets.Scripts.Networking.Modules.Login;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuForm : MonoBehaviour {

    public Text usernameLabel;

	// Use this for initialization
	void Start () {
        var u = LoginModule.Instance.user;
        if (u == null)
        {
            FindObjectOfType<LevelManager>().LoadLevel("Login");
            return;
        }
        usernameLabel.text = u.name;
    }
}
