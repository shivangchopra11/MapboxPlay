using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using TMPro;

public class InstaGetter : MonoBehaviour {

	public AbstractMap _map;

	// Use this for initialization
	IEnumerator Start () {
		string url = "https://api.instagram.com/v1/media/search?lat=28.666666666667&lng=77.216666666667&distance=1000000&access_token=1234845436.cab71bf.c4f2b5cc425a48dcb4bbcabb7f5aef5d";
		WWW www = new WWW(url);
		yield return www;
		string api_response = www.text;
		Debug.Log(api_response);

		IDictionary apiParse = (IDictionary)Json.Deserialize(api_response);
		IList instagramPicturesList = (IList)apiParse["data"];
		foreach(IDictionary instagramPicture in instagramPicturesList) {
			IDictionary images = (IDictionary)instagramPicture["images"];
			IDictionary standardResolution = (IDictionary)images["standard_resolution"];
			string mainPic_url = (string)standardResolution["url"];
			Debug.Log(mainPic_url);

			WWW mainPic = new WWW(mainPic_url);
			yield return mainPic;

			IDictionary location = (IDictionary)instagramPicture["location"];
			double lat = (double)location["latitude"];
			double lon = (double)location["longitude"];

			GameObject instaPost = Instantiate(Resources.Load("InstaPost")) as GameObject;
			instaPost.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = mainPic.texture;
			
			instaPost.transform.position = _map.GeoToWorldPosition(new Vector2d(lat,lon));
			instaPost.transform.SetParent(_map.transform);

			//username and profile pic
			IDictionary user = (IDictionary)instagramPicture["user"];
			string username = (string)user["username"];
			string profilePicture_url = (string)user["profile_picture"];
			WWW profilePic = new WWW(profilePicture_url);
			yield return profilePic;
			instaPost.transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = profilePic.texture;
			instaPost.transform.GetChild(2).GetComponent<TextMeshPro>().text = username;


			//location name
			string placename = (string)location["name"];
			instaPost.transform.GetChild(3).GetComponent<TextMeshPro>().text = placename;


			//likes
			IDictionary Likes = (IDictionary)instagramPicture["likes"];
			string likes = (string)Likes["count"].ToString();
			instaPost.transform.GetChild(4).GetComponent<TextMeshPro>().text = likes + " likes";

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
