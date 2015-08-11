using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThrownCardsDeck : MonoBehaviour {
	
	public static ThrownCardsDeck Ins;
	
	public Cards currentCard;
	
	public List<Cards> thrownCards = new List<Cards>();
	
	void Awake(){
		Ins = this;
	}
	
	public void AddCardToThrownCards(Cards card){
		
		if(thrownCards.Count == 0){
			thrownCards.Add(card);
			currentCard = card;
		}
		else{
			//Destroy(thrownCards[thrownCards.Count-1].gameObject);
			thrownCards[thrownCards.Count-1].cardType = CardType.pastDeck;
			thrownCards[thrownCards.Count-1].gameObject.SetActive(false);
			//thrownCards.RemoveAt((thrownCards.Count-1));
			thrownCards.Add(card);
			currentCard = card;
		}
		
	}
	
	public void RemoveCardFromThrownCards(){
		if(thrownCards.Count >= 2)
			thrownCards[thrownCards.Count-2].gameObject.SetActive(true);
		thrownCards.RemoveAt((thrownCards.Count-1));
	}
	
}
