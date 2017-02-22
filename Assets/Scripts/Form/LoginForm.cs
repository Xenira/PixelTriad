using Assets.Scripts.Networking.Modules.Login;
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
        LoginModule.Instance.Login(tbEmail.text, tbPassword.text, (success, user) =>
        {
            btnLogin.enabled = true;
            if (!success)
            {
                lblError.text = "Could not log in with given credentials";
                return;
            }

            FindObjectOfType<LevelManager>().LoadLevel("Start");
        });
        //ServerConnection.Login(this, tbEmail.text, tbPassword.text, (error, session) =>
        //{
        //    btnLogin.enabled = true;
        //    if (error != null)
        //    {
        //        lblError.text = error;
        //    }
        //    else
        //    {
        //        FindObjectOfType<LevelManager>().LoadLevel("Start");
        //    }
        //});
    }
}
