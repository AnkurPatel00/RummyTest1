using UnityEngine;
using System.Collections;

public class CardMovemenManager : MonoBehaviour {
	
	public PlayerController playerController;
	public Camera cam;
	
	public GameObject cube;
	
	public Transform tempCardHolder;
	
	private RaycastHit hit;
	private Ray ray;
	public Transform card;
	
	
	private Vector3 firstMousePosition;
	private Vector3 secondMousePosition;
	
	private bool isDevice;
	
	private GameObject tempColorCard;
	
	// Use this for initialization
	void Start () {
		if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer){
			isDevice = true;
		}
	}
	
	void DoOnFingerDown(Vector3 position){
		
		ray = cam.ScreenPointToRay (position);
		
		if (Physics.Raycast (ray, out hit)) {
			
			if (hit.collider.tag == "card") {
				
				if(hit.collider.GetComponent<Cards>().cardType != CardType.deck){
					if(playerController.turnType != TurnType.pick){
						card = hit.collider.transform;
						card.gameObject.GetComponent<BoxCollider>().enabled = false;
					}
					else{
						playerController.dialog = playerController.turnType.ToString()+" one card \n From Deck";
						UIEventHandler.Ins.OpenMenu("PickCardNotification");
						UIEventHandler.Ins.pickNotificationText.text = playerController.dialog;
						//playerController.showDialogBox = true;
					}
				}
				else if(hit.collider.GetComponent<Cards>().cardType == CardType.deck  && playerController.turnType != TurnType.through && playerController.turnType != TurnType.throughtorshow){
					
					//GameManager.Ins.turnOverButton.SetActive(true);
					playerController.showChangeTurn = true;
					playerController.turnType = TurnType.show;
					hit.collider.GetComponent<Cards>().cardType = CardType.hand;
					playerController.packInitiator.PlaceDeckCardOnPlayerHands(hit.collider.GetComponent<Cards>());
					ThrownCardsDeck.Ins.RemoveCardFromThrownCards();
					playerController.pickedDeckCards = hit.collider.GetComponent<Cards>() as Cards;
				}
			}
		}
	}

	bool IsValidDashboardPosition(){
		bool flag = false;
		if(ThrownCardsDeck.Ins.thrownCards.Count > 0){
			if(Vector3.Distance(card.transform.position,ThrownCardsDeck.Ins.thrownCards[ThrownCardsDeck.Ins.thrownCards.Count-1].transform.position) > 0.9f){
				flag = true;
			}
			else {
				flag = false;
			}
		}
		return flag;
	}
	
	
	void DoOnFingerUp(Vector3 position){
		
		if(tempColorCard){
			tempColorCard.renderer.material.color = Color.white;
			tempColorCard = null;
		}
		
		ray = cam.ScreenPointToRay (position);
		
		if (Physics.Raycast (ray, out hit)) {
			
			if (hit.collider.tag == "PlaceCard") {
				if(playerController.turnType != TurnType.pick && playerController.turnType != TurnType.show){
					
					if(!playerController.showChangeTurn){
						card.GetComponent<Cards>().StartAnimation(tempCardHolder);
						card.GetComponent<Cards>().cardType = CardType.deck;
						playerController.showChangeTurn = true;
						UIEventHandler.Ins.changeTurnButton.animation.Play();
						playerController.placedDeckCards = card.GetComponent<Cards>() as Cards;
						//GameManager.Ins.ChangeTurn();
					}
				}
				else{
					if(card.GetComponent<Cards>().cardType != CardType.dashboard){
						card.GetComponent<Cards>().ResetToOriginalHandPosition();
					}
					else if(card.GetComponent<Cards>().cardType == CardType.dashboard){
						card.GetComponent<Cards>().ResetToOriginalDashboard();
					}
				}
			}
			
			if (hit.collider.tag == "Deck" || hit.collider.tag == "ArrangeCard" || hit.collider.tag == "Floor" || hit.collider.tag == "TableBorder") {
				if(playerController.turnType == TurnType.through || playerController.turnType == TurnType.show || playerController.turnType == TurnType.throughtorshow){
					if(card.GetComponent<Cards>().cardType != CardType.dashboard){
						card.GetComponent<Cards>().ResetToOriginalHandPosition();
					}
					else if(card.GetComponent<Cards>().cardType == CardType.dashboard){
						card.GetComponent<Cards>().ResetToOriginalDashboard();
					}
				}
			}
			
			if (hit.collider.tag == "DashBoard") {
				if((playerController.turnType == TurnType.show || playerController.turnType == TurnType.throughtorshow) && card.GetComponent<Cards>().cardType == CardType.hand && IsValidDashboardPosition()){
					if(card.GetComponent<Cards>().cardType != CardType.dashboard){
						
						if(playerController.turnType == TurnType.throughtorshow){
							//GameManager.Ins.turnOverButton.SetActive(true);
							playerController.showChangeTurn = true;
							UIEventHandler.Ins.changeTurnButton.animation.Play();
						}
						
						card.GetComponent<Cards>().InstantiateDashBoardCardHolder(playerController);
						card.GetComponent<Cards>().cardType = CardType.dashboard;
					}
					else{
						print ("card.GetComponent<Cards>().cardType == CardType.dashboard");
						card.GetComponent<Cards>().InstantiateOnDashBoardCardHolder(hit.collider.GetComponent<Cards>());
					}
				}
				else{
					if(card.GetComponent<Cards>().cardType == CardType.dashboard){
						//card.GetComponent<Cards>().ResetToOriginalDashboard();
						print ("InstantiateSeparateDashBoardCardHolder");
						card.GetComponent<Cards>().InstantiateSeparateDashBoardCardHolder(playerController);
					}
					else
						card.GetComponent<Cards>().ResetToOriginalHandPosition();
				}
			}
			if (hit.collider.tag == "card") {
				
				if(hit.collider.GetComponent<Cards>().cardType == CardType.dashboard){
					playerController.showChangeTurn = true;
					card.GetComponent<Cards>().InstantiateOnDashBoardCardHolder(hit.collider.GetComponent<Cards>());
				}
				else if(hit.collider.GetComponent<Cards>().cardType == CardType.deck){
					if(!playerController.showChangeTurn){
						card.GetComponent<Cards>().StartAnimation(tempCardHolder);
						card.GetComponent<Cards>().cardType = CardType.deck;
						playerController.showChangeTurn = true;
						UIEventHandler.Ins.changeTurnButton.animation.Play();
						playerController.placedDeckCards = card.GetComponent<Cards>() as Cards;
					}
					else{
						card.GetComponent<Cards>().ResetToOriginalHandPosition();
					}
					//GameManager.Ins.ChangeTurn();
				}
				else if(hit.collider.GetComponent<Cards>().cardType == CardType.hand){
					
					card.GetComponent<Cards>().ResetToOriginalHandPosition();
				}
				else if(hit.collider.GetComponent<Cards>().cardType == CardType.pastDeck){
					print ("past dest");
					//if(playerController.showChangeTurn){
					print ("past dest true");
					card.GetComponent<Cards>().StartAnimation(tempCardHolder);
					card.GetComponent<Cards>().cardType = CardType.deck;
					playerController.placedDeckCards = card.GetComponent<Cards>() as Cards;
					if(playerController.pickedDeckCards){
						if(playerController.placedDeckCards.gameObject != playerController.pickedDeckCards.gameObject){
							playerController.showChangeTurn = true;
							UIEventHandler.Ins.changeTurnButton.animation.Play();
						}
						else{
							if(playerController.dashBoardCardHolder.Count <= 0){
								print ("playerController.RevertFromWrongOrder()");
								playerController.turnType = TurnType.pick;
							}
							playerController.showChangeTurn = false;
						}
					}
					else{
						playerController.showChangeTurn = true;
						UIEventHandler.Ins.changeTurnButton.animation.Play();
					}
					//}
					//else{
						//card.GetComponent<Cards>().ResetToOriginalHandPosition();
					//}
				}
			}
		}
		
		card.gameObject.GetComponent<BoxCollider>().enabled = true;
		card = null;
	}
	
	void HandleCardMovement(Vector3 position){
		if(card){
//			if(ThrownCardsDeck.Ins.thrownCards.Count > 0)
//				print (Vector3.Distance(card.transform.position,ThrownCardsDeck.Ins.thrownCards[ThrownCardsDeck.Ins.thrownCards.Count-1].transform.position));
			ray = cam.ScreenPointToRay (position);
			
			if (Physics.Raycast (ray, out hit)) {
				
				Vector3	posision = hit.point;
				posision.y = posision.y+0.1f;
				posision.z = posision.z+0.25f;
				
				
				if (hit.collider.tag == "card") {
					
					if(tempColorCard == null){
						tempColorCard = hit.collider.gameObject;
						tempColorCard.renderer.material.color = Color.yellow;
					}else if(tempColorCard != hit.collider.gameObject){
						tempColorCard.renderer.material.color = Color.white;
						tempColorCard = hit.collider.gameObject;
						tempColorCard.renderer.material.color = Color.yellow;
					}
					
					if(hit.collider.GetComponent<Cards>().cardType == CardType.hand){
						card.rotation = Quaternion.Euler(card.GetComponent<Cards>().packInitiator.cardRotation);
						
						posision.y = posision.y+0.2f;
						
					}
					else if(hit.collider.GetComponent<Cards>().cardType == CardType.dashboard){
						card.rotation = Quaternion.identity;
					}
				}else{
					if(tempColorCard){
						tempColorCard.renderer.material.color = Color.white;
						tempColorCard = null;
					}
				}
				
				if (hit.collider.tag == "DashBoard" || hit.collider.tag == "Deck" || hit.collider.tag == "ArrangeCard" || hit.collider.tag == "Floor" || hit.collider.tag == "PlaceCard" || hit.collider.tag == "TableBorder") {
					
					card.rotation = Quaternion.identity;
				}
				
				card.GetComponent<Cards>().cardsPositionPoints.UpdatePosition(posision);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if(playerController.isMyTurn && playerController.canTouch && !playerController.showFalseCombinationMenu && (playerController.turnType == TurnType.through || playerController.turnType == TurnType.pick || playerController.turnType == TurnType.show || playerController.turnType == TurnType.throughtorshow)){
			
			if(!isDevice){
				if (Input.GetMouseButtonDown (0)) {
					DoOnFingerDown(Input.mousePosition);
				}
				
				HandleCardMovement(Input.mousePosition);
				
				if (Input.GetMouseButtonUp (0)){
					if(card){
						DoOnFingerUp(Input.mousePosition);
					}
				}
			}
			else{
				for(int i = 0; i < Input.touchCount; i++){
					
					if(Input.GetTouch(i).phase == TouchPhase.Began){
						DoOnFingerDown(Input.GetTouch(i).position);
					}
					
					HandleCardMovement(Input.GetTouch(i).position);
					
					if(Input.GetTouch(i).phase == TouchPhase.Ended){
						DoOnFingerUp(Input.GetTouch(i).position);
					}
				}
			}
			
			
		}
		else if(playerController.isMyTurn && playerController.turnType == TurnType.pick){
			
		}
	}
}
