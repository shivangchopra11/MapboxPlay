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
		string url = "https://api.instagram.com/v1/media/search?lat=28.666666666667&lng=77.216666666667&distance=10000000&access_token=1234845436.cab71bf.c4f2b5cc425a48dcb4bbcabb7f5aef5d";
		string url1 = "https://api.instagram.com/v1/media/search?lat=28.7500749&lng=77.1154765&distance=10000000&access_token=5603831518.4cd38f1.29c7da189d37456bb6892822937486ac";
		WWW www = new WWW(url);
		yield return www;
		WWW www1 = new WWW(url1);
		yield return www1;
		string api_response = www.text;
		string api_response1 = www1.text;
		Debug.Log(api_response1);
		Debug.Log(api_response);

		IDictionary apiParse = (IDictionary)Json.Deserialize(api_response);
		IDictionary apiParse1 = (IDictionary)Json.Deserialize(api_response1);
		IList instagramPicturesList1 = (IList)apiParse1["data"];
		IList instagramPicturesList = (IList)apiParse["data"];
		foreach(IDictionary temp in instagramPicturesList1) {
			instagramPicturesList.Add(temp);
		}
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
