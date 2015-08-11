using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DashBoardCardHolder : MonoBehaviour {

	public GameObject cardsPositionPointsObject;

	public List<Cards> cards = new List<Cards>();

	public List<CardsPositionPoints> cardsPositionPoints = new List<CardsPositionPoints>();

	private Vector3 cardIncrement;

	public int id;

	public bool doNotCheckforsequence;

	// Use this for initialization
	void Start () {
		cardIncrement = new Vector3 (0.16f,0.001f,0);
	}


	public DashboardPositionData GetPosition(GameObject cardObj){

		DashboardPositionData dashboardPositionData = new DashboardPositionData();
		Vector3 cardPosition = cardObj.transform.position;
		int n = -1;
		int foundAtIndex = -1;
		bool flag = false;

		for (int i = 0; i < cardsPositionPoints.Count; i++) {

			if(cardPosition.x == cardsPositionPoints[i].transform.position.x){
				n = i;
				print (" equal at "+i);
			}

			if(cardObj == cardsPositionPoints[i].cards.gameObject){
				//print ("card already exist, fount at index "+i);
				foundAtIndex = i;
			}

		}

		if (n >= 0) {
			if(cardObj != cardsPositionPoints[n].cards.gameObject){
				//print("current card "+cardObj.name+" is equal in position to card "+cardsPositionPoints [n].cards.name+" at index "+n);
				dashboardPositionData.index = n;
				dashboardPositionData.position = cardsPositionPoints [n].transform.position;
				for (int i = n; i < cardsPositionPoints.Count; i++) {

					cardsPositionPoints [i].transform.position += cardIncrement;
				}
			}else{
				//print ("card is equal in position with itself");
				dashboardPositionData.sameObject = true;
			}

		}else{

			if(cardsPositionPoints.Count <= 1){
				//print ("only one card");
				if(cardPosition.x > cardsPositionPoints[0].transform.position.x){
					//print ("card is at end point, card name is "+cardObj.name+" first card is "+cardsPositionPoints[0].cards.gameObject.name);
					dashboardPositionData.index = 0;
					dashboardPositionData.position = cardsPositionPoints [0].transform.position+cardIncrement;
					dashboardPositionData.isLast = true;
				}else {
					//print ("card is at first point, card name is "+cardObj.name+" first card is "+cardsPositionPoints[0].cards.gameObject.name);
					dashboardPositionData.index = 0;
					dashboardPositionData.position = cardsPositionPoints [0].transform.position;
					cardsPositionPoints [0].transform.position += cardIncrement;
				}
			}else{

				for (int i = 0; i < cardsPositionPoints.Count; i++) {

					if(cardPosition.x < cardsPositionPoints[i].transform.position.x && !flag){
						if(cardObj != cardsPositionPoints[i].cards.gameObject){
							//print ("card "+cardObj.name+" position is less than "+cardsPositionPoints[i].cards.gameObject.name);



							if(foundAtIndex >= 0 && foundAtIndex < i){
								if(cardObj != cardsPositionPoints[i-1].cards.gameObject){
									dashboardPositionData.index = i;
									dashboardPositionData.position = cardsPositionPoints [i-1].transform.position;
									//print ("card "+cardObj.name+" found at index "+foundAtIndex+" is less than i "+i+" where card at i is "+cardsPositionPoints[i].cards.gameObject.name);
									for(int j = foundAtIndex+1; j < i; j++)
										cardsPositionPoints[j].transform.position-=cardIncrement;
								}
								else{
									//print ("card is greater than in position with itself");
									dashboardPositionData.sameObject = true;
								}
							}
							else if(foundAtIndex >= 0 && foundAtIndex > i){
								int ivalue = 0;
								if((i-1) < 0)
									ivalue = 0;
								else
									ivalue = i-1;
								dashboardPositionData.index = ivalue;
								dashboardPositionData.position = cardsPositionPoints [ivalue].transform.position;
								//print ("card "+cardObj.name+" found at index "+foundAtIndex+" is greater than i "+i+" where card at i is "+cardsPositionPoints[i].cards.gameObject.name);
								for(int j = ivalue; j < foundAtIndex; j++)
									cardsPositionPoints[j].transform.position+=cardIncrement;

							}
							else{

								int ivalue = 0;
								if((i-1) < 0)
									ivalue = 0;
								else
									ivalue = i-1;
								dashboardPositionData.index = ivalue;
								dashboardPositionData.position = cardsPositionPoints [ivalue].transform.position;
								//print ("card "+cardObj.name+" found at index "+foundAtIndex+" is greater than i "+i+" where card at i is "+cardsPositionPoints[i].cards.gameObject.name);
								for(int j = ivalue; j < cardsPositionPoints.Count; j++)
									cardsPositionPoints[j].transform.position+=cardIncrement;

							}

							flag = true;
						}else{
							//print ("card is less than in position with itself");
							dashboardPositionData.sameObject = true;
						}
					}
					else if(i == (cardsPositionPoints.Count-1) && !flag){
						if(cardObj != cardsPositionPoints[i].cards.gameObject){
							//print ("card "+cardObj.name+" position is last to card "+cardsPositionPoints[i].cards.gameObject.name);
							dashboardPositionData.index = i;
							dashboardPositionData.position = cardsPositionPoints [i].transform.position+cardIncrement;
							dashboardPositionData.isLast = true;
							flag = true;
						}else{
							//print ("card is last in position with itself");
							dashboardPositionData.sameObject = true;
						}
					}
				}
			}

		}

		return dashboardPositionData;
	}


	public void DestroyCardsPositionPoints(CardsPositionPoints point){
		print(gameObject.name);
		int index = 0;
		bool flag = false;
		
		for (int i =0; i < cardsPositionPoints.Count; i++) {

			//if(cardsPositionPoints[i].id == point.id){
			if(cardsPositionPoints[i].transform.childCount <= 0 && !flag){
				flag = true;
				index = i;
			}


		}

		if (flag) {
			
			Destroy (cardsPositionPoints [index].gameObject);
			cardsPositionPoints.RemoveAt (index);

		} else {
			print ("point on dashboard not found ");
		}
	}

	public bool IsCardAtIndexZero(CardsPositionPoints point){
		bool flag = false;
		if(cardsPositionPoints[0].gameObject == point.gameObject){
			flag = true;
		}
		return flag;
	}
}
