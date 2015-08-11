using UnityEngine;
using System.Collections;

public class CardsPositionPoints : MonoBehaviour {

	public Cards cards;

	private bool zUp;

	public int id;

	public void UpdatePosition(Vector3 position){
		cards.transform.position = position;
		//cards.transform.rotation = Quaternion.identity;
	}
}
