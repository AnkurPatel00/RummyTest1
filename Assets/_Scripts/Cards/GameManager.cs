using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	public static GameManager Ins;
	
	public GameObject[] players;
	public GameObject[] cards;
	
	public GameObject dashBoardCardHolder;
	public GameObject cardsPositionPointsObject;
	
	public GameObject handsPlaceCard;
	public GameObject handsDeckCard;
	
	public GameObject turnOverButton;
	
	public List<int> cardNumList = new List<int>();
	
	private bool showDeckCardFinish = false;
	public bool showGameFinish = false;
	
	void Awake(){
		Ins = this;
	}
	
	// Use this for initialization
	void Start(){
		//turnOverButton.SetActive(false);
		DistributeCards ();
	}
	
	public void DistributeCards () {
		
		int i = 0;//Toss();
		
		InitialiseCardNumList();
		players[i].GetComponent<PlayerController>().isMyTurn = true;
		players[i].SendMessage("InitiatePack",GetRandomCardsFromPack(15));
		players[i].GetComponent<PlayerController>().turnType = TurnType.throughtorshow;
		SetHandPlaceCard(true);
		
		// if i = 0 than assign i = 1 or if i = 1 than assign i = 0;
		i = (i == 0) ? 1 : 0;
		players[i].SendMessage("InitiatePack",GetRandomCardsFromPack(14));
		players[i].GetComponent<AIPlayerController>().turnType = TurnType.pick;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R)){
			Application.LoadLevel(0);
		}
	}
	
	
	public void ShowGameOverGUI(){
		UIEventHandler.Ins.OpenMenu("GameOverNotification");
		UIEventHandler.Ins.gameoverNotificationText.text = "You won Gameover \n Thanks for playing";
	}
	
	public void ShowDeckCardFinishGUI(){
		UIEventHandler.Ins.OpenMenu("GameOverNotification");
		UIEventHandler.Ins.gameoverNotificationText.text = "Deck Card are finished \n Thanks for playing";
	}
	
	void InitialiseCardNumList(){
		
		cardNumList.Clear();
		
		for(int i = 0; i < 104; i++){
			cardNumList.Add(i);
		}
	}
	
	int Toss(){
		return Random.Range(0,2);
	}
	
	public void ChangeTurn(){
		
		if(players[0].GetComponent<PlayerController>().isMyTurn){
			
			if((players[0].GetComponent<PlayerController>().turnType == TurnType.show || players[0].GetComponent<PlayerController>().turnType == TurnType.throughtorshow) && players[0].GetComponent<PlayerController>().dashBoardCardHolder.Count > 0){
				players[0].GetComponent<PlayerController>().CheckDashboard();
				//turnOverButton.SetActive(false);
			}
			else{
				SetHandPlaceCard(false);
				
				players[0].GetComponent<PlayerController>().isMyTurn = false;
				
				players[0].GetComponent<PlayerController>().turnType = TurnType.pick;
				players[0].GetComponent<PlayerController>().placedDeckCards = null;
				players[1].GetComponent<AIPlayerController>().isMyTurn = true;
				players[1].GetComponent<AIPlayerController>().TakeTurn();
			}
		}
		else{
			players[0].GetComponent<PlayerController>().isMyTurn = true;
			//SetHandDeckCard(true);
			players[1].GetComponent<AIPlayerController>().isMyTurn = false;
			players[1].GetComponent<AIPlayerController>().turnType = TurnType.pick;
		}
		
	}
	
	List<GameObject> GetRandomCardsFromPack(int value){
		
		List<int> randomCardNumberList = new List<int>();
		List<GameObject> randomCardList =  new List<GameObject>();
		
		if(cardNumList.Count > 0){
			for(int i = 0 ; i < value; i++){
				int num = Random.Range(0,cardNumList.Count);
				randomCardNumberList.Add(cardNumList[num]);
				cardNumList.RemoveAt(num);
			}
			
			for(int i = 0 ; i < randomCardNumberList.Count; i++){
				randomCardList.Add(cards[randomCardNumberList[i]]);
			}
		}
		else{
			showDeckCardFinish = true;
		}
		
		return randomCardList;
	}
	
	public void AddCardToPlayerPack(string args){
		int num = int.Parse (args);
		//		if(num == 0){
		//			SetHandDeckCard(false);
		//		}
		players[num].SendMessage("InitiatePack",GetRandomCardsFromPack(1));
	}
	
	public void SetHandDeckCard(bool value){
		handsDeckCard.SetActive(value);
	}
	
	public void SetHandPlaceCard(bool value){
		handsPlaceCard.SetActive(value);
	}
}
