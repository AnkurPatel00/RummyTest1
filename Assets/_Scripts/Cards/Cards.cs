using UnityEngine;
using System.Collections;

public class Cards : MonoBehaviour {
	public CardType cardType;
	public string cardName;
	public int number;
	public PackInitiator packInitiator;
	public CardsPositionPoints cardsPositionPoints;
	public DashBoardCardHolder dashBoardCardHolder; 
	private Texture originalTexture;

	void Start(){
		originalTexture = renderer.material.mainTexture;
	}

	public void StartAnimation(Transform endPoint){
		StartCoroutine (AnimateCard(endPoint));
	}

	public void StartDeckToHandAnimation(Transform endPoint){
		if(packInitiator)
			if(packInitiator.isAiInitiator)
				renderer.material.mainTexture = packInitiator.cardBack;
		StartCoroutine(DisableCardsForAI());
		StartCoroutine (DeckToHandAnimation(endPoint));
	}

	IEnumerator DisableCardsForAI(){
		yield return new WaitForSeconds(0.5f);
		if (packInitiator)
		if (packInitiator.isAiInitiator) {
			renderer.enabled = false;
			renderer.material.mainTexture = originalTexture;
		}
	}

	public void ResetToOriginalHandPosition(){
		transform.position = new Vector3(cardsPositionPoints.transform.position.x,cardsPositionPoints.transform.position.y+0.16f,cardsPositionPoints.transform.position.z);
		transform.rotation = Quaternion.Euler(packInitiator.cardRotation);
	}

	public void ResetToOriginalDashboard(){

		if(!dashBoardCardHolder.IsCardAtIndexZero(cardsPositionPoints))
			transform.position = new Vector3(cardsPositionPoints.transform.position.x,cardsPositionPoints.transform.position.y,cardsPositionPoints.transform.position.z);
		else{
			dashBoardCardHolder.transform.position = transform.position;
			transform.position = cardsPositionPoints.transform.position;
		}
	}

	public void InstantiateDashBoardCardHolder(PlayerController playerController){
	
		cardType = CardType.dashboard;
		GameObject tempDashboardObject = Instantiate (GameManager.Ins.dashBoardCardHolder,transform.position,Quaternion.identity) as GameObject;
		tempDashboardObject.name = tempDashboardObject.name+playerController.dashBoardCardHolder.Count;
		GameObject tempPositionObject = Instantiate (GameManager.Ins.cardsPositionPointsObject,transform.position,Quaternion.identity) as GameObject;
		tempDashboardObject.GetComponent<DashBoardCardHolder>().cardsPositionPoints.Add(tempPositionObject.GetComponent<CardsPositionPoints>());
		dashBoardCardHolder = tempDashboardObject.GetComponent<DashBoardCardHolder> ();
		playerController.dashBoardCardHolder.Add(dashBoardCardHolder);
		tempPositionObject.transform.parent = tempDashboardObject.transform;
		transform.parent = tempPositionObject.transform;
		tempPositionObject.GetComponent<CardsPositionPoints> ().cards = this;
		tempDashboardObject.GetComponent<DashBoardCardHolder>().id = tempDashboardObject.GetComponent<DashBoardCardHolder>().id+1;
		tempPositionObject.GetComponent<CardsPositionPoints> ().id = tempDashboardObject.GetComponent<DashBoardCardHolder>().id;
		packInitiator.DestroyCardPosition (cardsPositionPoints);
		cardsPositionPoints = tempPositionObject.GetComponent<CardsPositionPoints>() as CardsPositionPoints;
	}


	public void InstantiateSeparateDashBoardCardHolder(PlayerController playerController){
		
		cardType = CardType.dashboard;
		GameObject tempDashboardObject = Instantiate (GameManager.Ins.dashBoardCardHolder,transform.position,Quaternion.identity) as GameObject;
		tempDashboardObject.name = tempDashboardObject.name+playerController.dashBoardCardHolder.Count;
		GameObject tempPositionObject = Instantiate (GameManager.Ins.cardsPositionPointsObject,transform.position,Quaternion.identity) as GameObject;
		tempDashboardObject.GetComponent<DashBoardCardHolder>().cardsPositionPoints.Add(tempPositionObject.GetComponent<CardsPositionPoints>());

		tempPositionObject.transform.parent = tempDashboardObject.transform;
		transform.parent = tempPositionObject.transform;
		DashboardPositionData dashboardPositionData = dashBoardCardHolder.GetPosition(gameObject);
		//dashboardPositionData.index =0;
		//print ("dashBoardCardHolder "+dashBoardCardHolder.name);
		dashBoardCardHolder.DestroyCardsPositionPoints (cardsPositionPoints);

		if (dashBoardCardHolder.cardsPositionPoints.Count <= 0)
			playerController.DestroyDashboard (dashBoardCardHolder);
		dashBoardCardHolder = tempDashboardObject.GetComponent<DashBoardCardHolder> ();
		playerController.dashBoardCardHolder.Add(dashBoardCardHolder);
		tempPositionObject.GetComponent<CardsPositionPoints> ().cards = this;
		tempDashboardObject.GetComponent<DashBoardCardHolder>().id = tempDashboardObject.GetComponent<DashBoardCardHolder>().id+1;
		tempPositionObject.GetComponent<CardsPositionPoints> ().id = tempDashboardObject.GetComponent<DashBoardCardHolder>().id;

		cardsPositionPoints = tempPositionObject.GetComponent<CardsPositionPoints>() as CardsPositionPoints;


	}



	public void InstantiateOnDashBoardCardHolder(Cards tempCard){

		DashboardPositionData dashboardPositionData = tempCard.dashBoardCardHolder.GetPosition(gameObject);

		if(!dashboardPositionData.sameObject){

			GameObject tempPositionObject = Instantiate (GameManager.Ins.cardsPositionPointsObject,dashboardPositionData.position,Quaternion.identity) as GameObject;
			transform.position = tempPositionObject.transform.position;
			transform.parent = tempPositionObject.transform;
			tempPositionObject.transform.parent = tempCard.dashBoardCardHolder.transform;
			tempCard.dashBoardCardHolder.cardsPositionPoints.Add(tempPositionObject.GetComponent<CardsPositionPoints>());

			if (dashboardPositionData.index < tempCard.dashBoardCardHolder.cardsPositionPoints.Count && !dashboardPositionData.isLast) {

				CardsPositionPoints temppositionpoint = tempCard.dashBoardCardHolder.cardsPositionPoints [dashboardPositionData.index];
				tempCard.dashBoardCardHolder.cardsPositionPoints [dashboardPositionData.index] = tempCard.dashBoardCardHolder.cardsPositionPoints [tempCard.dashBoardCardHolder.cardsPositionPoints.Count - 1];
					
				for (int i = (dashboardPositionData.index+1); i < tempCard.dashBoardCardHolder.cardsPositionPoints.Count; i++) {
					
					CardsPositionPoints temppositionpoint2 = tempCard.dashBoardCardHolder.cardsPositionPoints [i];
					tempCard.dashBoardCardHolder.cardsPositionPoints [i] = temppositionpoint;
					temppositionpoint = temppositionpoint2; 
				}
			}
			else if(dashboardPositionData.isLast){
				print("added to last postiion");
				transform.position = dashboardPositionData.position;
			}

			if(cardType == CardType.dashboard){
				dashBoardCardHolder.DestroyCardsPositionPoints (cardsPositionPoints);
			}
			else{
				packInitiator.DestroyCardPosition (cardsPositionPoints);
			}
			cardType = CardType.dashboard;
			cardsPositionPoints = tempPositionObject.GetComponent<CardsPositionPoints>() as CardsPositionPoints;
			this.dashBoardCardHolder = tempCard.dashBoardCardHolder;
			dashBoardCardHolder.id = dashBoardCardHolder.id+1;
			tempPositionObject.GetComponent<CardsPositionPoints> ().id = dashBoardCardHolder.id;
			tempPositionObject.GetComponent<CardsPositionPoints> ().cards = this;
		}
		else{
			//transform.position = dashboardPositionData.position;
			transform.position = cardsPositionPoints.transform.position;
		}
	}

	IEnumerator AnimateCard(Transform endPoint){

		StopCoroutine("DeckToHandAnimation");

		transform.parent = endPoint;
		packInitiator.DestroyCardPosition (cardsPositionPoints);
		transform.rotation = Quaternion.identity;
		while (Vector3.Distance(transform.position,new Vector3(endPoint.position.x,endPoint.position.y+0.16f,endPoint.position.z)) > 0.1f) {
			transform.position = Vector3.Slerp(transform.position,new Vector3(endPoint.position.x,endPoint.position.y+0.16f,endPoint.position.z),Time.deltaTime*2);
			yield return null;
		}
		transform.position = new Vector3(endPoint.position.x,endPoint.position.y+0.16f,endPoint.position.z);

		ThrownCardsDeck.Ins.AddCardToThrownCards(this);
		//print ("thrown");
		yield return null;
	}

	IEnumerator DeckToHandAnimation(Transform endPoint){
		StopCoroutine("AnimateCard");
		while (Vector3.Distance(transform.position,new Vector3(endPoint.position.x,endPoint.position.y+0.16f,endPoint.position.z)) > 0.1f) {
			transform.position = Vector3.Slerp(transform.position,new Vector3(endPoint.position.x,endPoint.position.y+0.16f,endPoint.position.z),Time.deltaTime*3);
			yield return null;
		}
		yield return null;
		transform.position = new Vector3(endPoint.position.x,endPoint.position.y+0.16f,endPoint.position.z);

	}
}
