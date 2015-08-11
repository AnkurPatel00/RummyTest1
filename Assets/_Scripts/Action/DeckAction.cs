using UnityEngine;
using System.Collections;

public class DeckAction : Action {

	public override void TackAction (){

		GameManager.Ins.DistributeCards();
	}
}
