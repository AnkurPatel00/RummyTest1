using UnityEngine;
using System.Collections;

public class OnButtonClick : MonoBehaviour {

	public GameObject notificationObject;
	public string functionName;
	public string argument;
	public bool withoutArgs;
	void OnMouseDown(){
		print ("hi");
		if(withoutArgs)
			notificationObject.SendMessage (functionName);
		else
			notificationObject.SendMessage (functionName,argument);
	}

}
