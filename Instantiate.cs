using UnityEngine;
using System.Collections;

public class Instantiate : MonoBehaviour {

	public Transform SpaceCraft;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnNetworkLoadedLevel () {
 		// Instantiating SpaceCraft when Network is loaded
//		Network.Instantiate(SpaceCraft, transform.position, transform.rotation, 0);
//		GameObject obj = GameObject.Find("AvatarNITE");
//		Debug.Log("Found object");
	}
	
	void OnPlayerDisconnected (NetworkPlayer player) {
		Debug.Log("Player from server");	
 		Network.RemoveRPCs(player, 0);
 		Network.DestroyPlayerObjects(player);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info){
		Debug.Log("Disconnected from server");
		//Network.Destroy(GameObject.Find("Player(Clone)"));
		Application.LoadLevel("NetworkTutorial");
	}
	
    void OnPlayerConnected(NetworkPlayer player) {
        Debug.Log("Player connected from " + player.ipAddress + ":" + player.port);
    }

}
