using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataBase : MonoBehaviour {
	
}

public enum TurnType{pick,through,none,show,throughtorshow};
public enum CardType{hand,dashboard,deck,pastDeck};

public class DashboardPositionData{
	public Vector3 position;
	public int index;
	public bool isLast;
	public bool sameObject;
	public int foundAtIndex;
	public bool foundAtIndexLessThani;
	public bool foundAtIndexGreaterThani;
}
