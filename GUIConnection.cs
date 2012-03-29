using UnityEngine;
using System.Collections;
using System;

public class GUIConnection : MonoBehaviour {
	
	public int listenPort = 25000;
	public string gameType = "uscrehabkinect";
	public string gameName = "First";
	private bool useNAT = false;
	
	public bool check = false;
	// Use this for initialization
	void Start () {

	}
	
	void Awake(){
		MasterServer.RequestHostList(gameType);
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	
	void OnGUI () {
 		// Checking if you are connected to the server or not
		if (Network.peerType == NetworkPeerType.Disconnected)
  		{
  			// If not connected
   			gameName = GUI.TextField(new Rect(200,50,100,30),gameName);
  			if (GUI.Button (new Rect(10,50,100,30),"Start Game"))
  			{
   				// Creating server
				useNAT = !Network.HavePublicAddress();
   				Network.InitializeServer(32, listenPort, useNAT);
				MasterServer.RegisterHost(gameType, gameName);
   
   				// Notify our objects that the level and the network is ready
   				//for (var go : GameObject in FindObjectsOfType(GameObject))
				GameObject[] games = FindObjectsOfType(typeof(GameObject)) as GameObject[];
				foreach (GameObject go in games)
   				{
    				go.SendMessage("OnNetworkLoadedLevel", 
					SendMessageOptions.DontRequireReceiver); 
   				}
				
//				Network.Instantiate(playerPrefab,new Vector3(0.19f,1.184f,-6.94f),transform.rotation,0);
//				
//				
//				GameObject obj = GameObject.Find("NITE");
//				Nite NiteObj = obj.GetComponent<Nite>();
//				
//				GameObject Avatar = GameObject.Find("AvatarNITE(Clone)");
//				AvatarNITE aN = Avatar.GetComponent<AvatarNITE>();
//				NiteObj.avatars[0] = aN;
//				NiteObj.calibratedUsers.Add(1,aN);
//				//aN.Start();
//				
//				NiteObj.networkStarted = true;
				
			}
			int y = 200;
			foreach ( HostData element in MasterServer.PollHostList() ){
				if (GUI.Button(new Rect(10,y,100,30),"Connect"))
				{
					// Connect to HostData struct, internally the correct method is used (GUID when using NAT).
					Network.Connect(element);
					// Notify our objects that the level and the network is ready
   				//for (var go : GameObject in FindObjectsOfType(GameObject))
				//GameObject[] games = FindObjectsOfType(typeof(GameObject)) as GameObject[];
				//foreach (GameObject go in games)
   				//{
    			//	go.SendMessage("OnNetworkLoadedLevel", 
				//	SendMessageOptions.DontRequireReceiver); 
   				//}
					//Network.Instantiate(playerPrefab,new Vector3(-0.19f,1.184f,-6.94f),transform.rotation,0);
					//GameObject obj = GameObject.Find("NITE");
					//Nite NiteObj = obj.GetComponent<Nite>();
					//NiteObj.networkStarted = true;
				}	
				y+=40;
			}
  		}
 		else
  		{
  			// Getting your ip address and port
  			string ipaddress = Network.player.ipAddress;
  			string port = Network.player.port.ToString();
			string GUID="NA";
   			if(useNAT)
				GUID = Network.player.guid;
  			GUI.Label(new Rect(30,20,250,40),"IP Adress: "+ipaddress+":"+port);
			if(useNAT)
			{
				GUI.Label(new Rect(30,70,250,40),"GUID : " + GUID);
			}
  			if (GUI.Button (new Rect(30,150,100,50),"Disconnect"))
  			{
   				// Disconnect from the server
   				Network.Disconnect(200);
  			}
  		}
		
//		if (GUI.Button (new Rect(100,100,100,30),"Print Games"))
//		{
//			if (MasterServer.PollHostList().Length != 0) {
//            	HostData[] hostData = MasterServer.PollHostList();
//            	int i = 0;
//            	while (i < hostData.Length) {
//					
//					string tmpIp = "";
//					int j = 0;
//           		 	while (j < hostData[i].ip.Length) {
//						tmpIp = hostData[i].ip[i] + " ";
//						j++;
//					}
//					Debug.Log("Game name: " + hostData[i].gameName + " IP: " + tmpIp);
//					i++;
//           	 	}
//            	MasterServer.ClearHostList();
//        	}
//			else{
//				Debug.Log("Did not find Master Server Games");
//			}
//		}
		
		
	}
	
	void OnConnectedToServer () {
 		// Notify our objects that the level and the network are ready
		GameObject[] games = FindObjectsOfType(typeof(GameObject)) as GameObject[];
 		foreach (GameObject go in games )
  			go.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);
	}

}
