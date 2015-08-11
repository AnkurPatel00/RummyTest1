using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPlayerController : MonoBehaviour {

	public TurnType turnType;

	public bool isMyTurn = false;
	public PackInitiator packInitiator;
	public Transform tempCardHolder;

	// Use this for initialization
	void Start () {
	
	}

	public void InitiatePack(List<GameObject> cards){


		if(turnType == TurnType.none || turnType == TurnType.pick){
			if(cards.Count > 0){
				packInitiator.InitiatePack (cards);
			}
		}
	}

	public void TakeTurn(){

		StartCoroutine("Move");
	}

	IEnumerator Move(){
		yield return new WaitForSeconds(0.5f);
		if(turnType == TurnType.pick){
			GameManager.Ins.AddCardToPlayerPack("1");
		}
		yield return new WaitForSeconds(2);
		GetSingleCard();

		//change turn
		yield return new WaitForSeconds(1.5f);
		GameManager.Ins.ChangeTurn();
	}

	public class AI_SingleCards{
		public string cardName;
		public int cardNumber;
		public int numberOfTimes;
		public int packInitiatorIndex;
	}

//	void GetSingleCard(){
//
//		int[] 		cardnumber 	= new int[]{1,2,3,4,5,6,7,8,9,10,11,12,13};
//		int 		indexCount 	= 0;
//		List<AI_SingleCards> ai_SingleCards = new List<AI_SingleCards>();
//
//		for(int a = 0; a < cardnumber.Length; a++){
//
//			int cardNumber = cardnumber[a];
//
//			for(int i = 0; i < packInitiator.cardsPositionPoints.Count; i++){
//
//				if(packInitiator.cardsPositionPoints[i].cards.number == cardNumber){
//					int n = IsNumberPresentInai_CardTracker(ai_SingleCards,cardNumber);
//					if(n == -1){
//						AI_SingleCards ai_SingleCardsTemp = new AI_SingleCards();
//						ai_SingleCardsTemp.cardName = packInitiator.cardsPositionPoints[i].cards.cardName;
//						ai_SingleCardsTemp.cardNumber = cardNumber;
//						ai_SingleCardsTemp.numberOfTimes+=1;
//						ai_SingleCardsTemp.packInitiatorIndex = i;
//						ai_SingleCards.Add(ai_SingleCardsTemp);
//
//					}
//					else{
//						ai_SingleCards[n].numberOfTimes+=1;
//					}
//
//					indexCount++;
//				}
//
//			}
//		}
//
//
//		List<AI_SingleCards> ai_SingleCardsWithMinimumNumberOfTimes = new List<AI_SingleCards>();
//
//		for(int i = 1; i < ai_SingleCards.Count; i++){
//			if(ai_SingleCards[i].numberOfTimes <= 1){
//				ai_SingleCardsWithMinimumNumberOfTimes.Add(ai_SingleCards[i]);
//			}
//		}
//
//		if(ai_SingleCardsWithMinimumNumberOfTimes.Count >=1 ){
//			packInitiator.cardsPositionPoints[ai_SingleCardsWithMinimumNumberOfTimes[0].packInitiatorIndex].cards.renderer.enabled = true;
//			packInitiator.cardsPositionPoints[ai_SingleCardsWithMinimumNumberOfTimes[0].packInitiatorIndex].cards.cardType = CardType.deck;
//			packInitiator.cardsPositionPoints[ai_SingleCardsWithMinimumNumberOfTimes[0].packInitiatorIndex].cards.StartAnimation(tempCardHolder);
//
//		}
//		else{
//
//			int highestOrderCardIndex = 0;
//			for(int i = 1; i < packInitiator.cardsPositionPoints.Count; i++){
//				if(packInitiator.cardsPositionPoints[highestOrderCardIndex].cards.number < packInitiator.cardsPositionPoints[i].cards.number)
//					highestOrderCardIndex = i;
//			}
//			packInitiator.cardsPositionPoints[highestOrderCardIndex].cards.renderer.enabled = true;
//			packInitiator.cardsPositionPoints[highestOrderCardIndex].cards.cardType = CardType.deck;
//			packInitiator.cardsPositionPoints[highestOrderCardIndex].cards.StartAnimation(tempCardHolder);
//
//		}
//
//
//	}

	void GetSingleCard(){
		
		int[] 		cardnumber 	= new int[]{1,2,3,4,5,6,7,8,9,10,11,12,13};
		int 		indexCount 	= 0;
		List<AI_SingleCards> ai_SingleCards = new List<AI_SingleCards>();
		
		for(int a = 0; a < cardnumber.Length; a++){
			
			int cardNumber = cardnumber[a];
			
			for(int i = 0; i < packInitiator.cardsPositionPoints.Count; i++){
				
				if(packInitiator.cardsPositionPoints[i].cards.number == cardNumber){
					int n = IsNumberPresentInai_CardTracker(ai_SingleCards,cardNumber);
					if(n == -1){
						AI_SingleCards ai_SingleCardsTemp = new AI_SingleCards();
						ai_SingleCardsTemp.cardName = packInitiator.cardsPositionPoints[i].cards.cardName;
						ai_SingleCardsTemp.cardNumber = cardNumber;
						ai_SingleCardsTemp.numberOfTimes+=1;
						ai_SingleCardsTemp.packInitiatorIndex = i;
						ai_SingleCards.Add(ai_SingleCardsTemp);
						
					}
					else{
						ai_SingleCards[n].numberOfTimes+=1;
					}
					
					indexCount++;
				}
				
			}
		}

		bool singleCardFound = false;

		for(int i = 0; i < ai_SingleCards.Count; i++){
			if(ai_SingleCards[i].numberOfTimes <= 1){
				packInitiator.cardsPositionPoints[ai_SingleCards[i].packInitiatorIndex].cards.renderer.enabled = true;
				packInitiator.cardsPositionPoints[ai_SingleCards[i].packInitiatorIndex].cards.cardType = CardType.deck;
				packInitiator.cardsPositionPoints[ai_SingleCards[i].packInitiatorIndex].cards.StartAnimation(tempCardHolder);
				singleCardFound = true;
				break;
			}
		}

		if(!singleCardFound){
			print("singleCardFound "+singleCardFound.ToString());


		}
//		else{
//
//			int highestOrderCardIndex = 0;
//			for(int i = 1; i < packInitiator.cardsPositionPoints.Count; i++){
//				if(packInitiator.cardsPositionPoints[highestOrderCardIndex].cards.number < packInitiator.cardsPositionPoints[i].cards.number)
//					highestOrderCardIndex = i;
//			}
//			packInitiator.cardsPositionPoints[highestOrderCardIndex].cards.renderer.enabled = true;
//			packInitiator.cardsPositionPoints[highestOrderCardIndex].cards.cardType = CardType.deck;
//			packInitiator.cardsPositionPoints[highestOrderCardIndex].cards.StartAnimation(tempCardHolder);
//
//		}

		
		
	}



	int IsNumberPresentInai_CardTracker(List<AI_SingleCards> ai_SingleCards, int number){

		int index = -1;
		for(int i = 0; i < ai_SingleCards.Count; i++){
			if(ai_SingleCards[i].cardNumber == number){
				index = i;
			}
		}
		return index;
	}

}
