using System;
using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using UnityEngine;

public class ServerBehaviour : MonoBehaviour
{
    [SerializeField] private string _IPAddress = "localhost";
    [SerializeField] private int _port = 2002;
	[SerializeField] private NatsukiRemu _natsukiRemu;

	private AndroidPlugins _androidPlugins;

    [Serializable]
    public class ServerResponse {
        public string id;
        public string date; //DateTime is not supported by JsonUtility
    }

    [Serializable]
    public class GPSLoc {
        public string currentLoc;
    }

    [Serializable]
    public class Utility {
        public string currentGPSLoc;
		public bool muted;
    }

	[Serializable]
	public class RemuClass {
		public string trigger;
		public string voiceLine;

	}

    private string GetIPAddress() {
		// ""
        return "http://" + _IPAddress + ":" + _port;
    }
    
    private void ErrorOccured(Exception error) {
		Debug.LogError("An error has occured: " + error);
    }

	private void Awake() {
		_androidPlugins = GetComponent<AndroidPlugins>();
	}
    
    void Start() {

		// RestClient.Post<ServerResponse>(GetIPAddress(), new GPSLoc {
        //     currentLoc = ""
        // }).Then(response => {
        //     Debug.Log("レム has connected");
            
        //     Debug.Log("ID: " + response.id);
        //     Debug.Log("Date: " + response.date);
        // }).Catch(err => {
        //     ErrorOccured(err);
        // });
    }

    public void TouchScreenNetwork() {
        RestClient.Post(GetIPAddress() + "/touch", new RemuClass {
			trigger = _natsukiRemu.globalTrigger
		}).Then(response => {
			Debug.Log(response.StatusCode);
		});
        
    }

	public void VolumeMutedStatus() {
		RestClient.Post(GetIPAddress() + "/muted", new Utility {
			muted = _androidPlugins.IsMuted()
		}).Then(response => {
			Debug.Log(response.StatusCode);
		});
	}

}
