using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIEventHandler : MonoBehaviour {

	public PlayerController playerController;
	public static UIEventHandler Ins;
	public GameObject[] menus;

	public Text pickNotificationText;
	public Text gameoverNotificationText;
	public Text arrangementIssueText;

	public GameObject changeTurnButton;

	// Use this for initialization
	void Awake () {
		Ins = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CloseMenu(string s){
		playerController.canTouch = true;
		GameObject menuObj = GetMenu(s);
		menuObj.SetActive(false);
	}

	public void OpenMenu(string s){
		playerController.canTouch = false;
		GameObject menuObj = GetMenu(s);
		menuObj.SetActive(true);
	}

	private GameObject GetMenu(string s){

		GameObject menuObj = new GameObject();

		foreach(GameObject g in menus)
			if(g.name == s)
				menuObj = g;

		return menuObj;
	}

	public void PlayAgain(){
		Application.LoadLevel (0);
	}
}
