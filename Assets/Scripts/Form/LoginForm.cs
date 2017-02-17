using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LoginForm : MonoBehaviour
{

    public InputField tbEmail;
    public InputField tbPassword;
    public Button btnLogin;

    public Text lblError;

    public void Login()
    {
        btnLogin.enabled = false;
        ServerConnection.Login(this, tbEmail.text, tbPassword.text, (error, session) =>
        {
            btnLogin.enabled = true;
            if (error != null)
            {
                lblError.text = error;
            }
            else
            {
                FindObjectOfType<LevelManager>().LoadLevel("Start");
            }
        });
    }
}
