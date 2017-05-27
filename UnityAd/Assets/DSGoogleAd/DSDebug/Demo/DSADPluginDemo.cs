using UnityEngine;
using System.Collections;

public class DSADPluginDemo : MonoBehaviour {

	//для одноразового испольнения после всех инициализаций
	bool first = true;

	//обьект для проверки зависания
	public GameObject go;
	public GameObject go2;

	//состояние меняет с каждым климком
	bool pressed = false;

	// Use this for initialization
	//void Start () {} 
	
	// Update is called once per frame
	void Update () {
	
		//выполняет один раз после всех инициализаций
		if(first)
		{
			//если плагин не заданый то вывести сообщение
			if(DSGooglePlayPlugin.adPulug == null) 
				Debug.LogError("adPulug is null. SGooglePlayDebug or DSGooglePlayPlugin myst be enable");

			//загрузить банер
			DSGooglePlayPlugin.adPulug.DSAdLoad ();

			//болше не выполнять этого
			first = false;
		}

		//для тестирования ввода
		//меняет цвет куба с белого на черный при каждом клике
		if(Input.GetMouseButtonDown(0))
		{
			if (pressed)
			{
				go2.renderer.material.color = Color.white;
				pressed = false;
			}
			else
			{
				go2.renderer.material.color = Color.black;
				pressed = true;
			}
		}
	}


	void FixedUpdate () 
	{
		if(DSGooglePlayPlugin.adPulug.GetLoadingState()) go.renderer.material.color = Color.red;
		else  go.renderer.material.color = Color.blue;

		//если обьект задан
		if(go)
		{
			//постоянно поворачивать вокруг вериткальной оси, для проверки зависаний
			go.transform.RotateAround(go.transform.position, Vector3.up, 0.99f);
		}
	}


	//отривока всех кнопок
	void OnGUI()
	{
		//9 кнопок отвечающих за перемещение банера

		//верхний ряд
		if(GUI.Button(new Rect(0,0, 90 , 60), "TOP_LEFT"))
		{
			DSGooglePlayPlugin.adPulug.DSAdSetPosition(DSGooglePlayPlugin.ADPosition.TOP_LEFT);
		}
		if(GUI.Button(new Rect(90,0, 90, 60), "TOP_CENTER"))
		{
			DSGooglePlayPlugin.adPulug.DSAdSetPosition(DSGooglePlayPlugin.ADPosition.TOP_CENTER);
		}
		if(GUI.Button(new Rect(180,0, 90, 60), "TOP_RIGHT"))
		{
			DSGooglePlayPlugin.adPulug.DSAdSetPosition(DSGooglePlayPlugin.ADPosition.TOP_RIGHT);
		}
		
		
		//срединй ряд
		if(GUI.Button(new Rect(0,60, 90, 60), "CEN_LEFT"))
		{
			DSGooglePlayPlugin.adPulug.DSAdSetPosition(DSGooglePlayPlugin.ADPosition.CENTER_LEFT);
		}
		if(GUI.Button(new Rect(90,60, 90, 60), "CENTER"))
		{
			DSGooglePlayPlugin.adPulug.DSAdSetPosition(DSGooglePlayPlugin.ADPosition.CENTER);
		}
		if(GUI.Button(new Rect(180,60, 90, 60), "CEN_RIGHT"))
		{
			DSGooglePlayPlugin.adPulug.DSAdSetPosition(DSGooglePlayPlugin.ADPosition.CENTER_RIGHT);
		}
		

		//нижний ряд
		if(GUI.Button(new Rect(0,120, 90, 60), "BOT_LEFT"))
		{
			DSGooglePlayPlugin.adPulug.DSAdSetPosition(DSGooglePlayPlugin.ADPosition.BOTTOM_LEFT);
		}
		if(GUI.Button(new Rect(90,120, 90, 60), "BOT_CEN"))
		{
			DSGooglePlayPlugin.adPulug.DSAdSetPosition(DSGooglePlayPlugin.ADPosition.BOTTOM_CENTER);
		}
		if(GUI.Button(new Rect(180,120, 90, 60), "BOT_RIGHT"))
		{
			DSGooglePlayPlugin.adPulug.DSAdSetPosition(DSGooglePlayPlugin.ADPosition.BOTTOM_RIGHT);
		}
		
		
		//2 кноки для включения/отключения показа банера
		if(GUI.Button(new Rect(0,180, 135, 60), "ON"))
		{
			DSGooglePlayPlugin.adPulug.DSAdSetVisible(true);
		}
		if(GUI.Button(new Rect(135,180, 135, 60), "OFF"))
		{
			DSGooglePlayPlugin.adPulug.DSAdSetVisible(false);
		}
			
			

		//5 кнопок отвечаюцих за перезагрузку рекламы с новыми размером
		if(GUI.Button(new Rect(Screen.width - 180,0, 180, 60), "BANNER"))
		{
			DSGooglePlayPlugin.adPulug.DSAdLoad(null, DSGooglePlayPlugin.ADBannerSize.BANNER, DSGooglePlayPlugin.adPulug.PreviousPosition());
		}
		if(GUI.Button(new Rect(Screen.width - 180,60, 180, 60), "MEDIUM_RECTANGLE"))
		{
			DSGooglePlayPlugin.adPulug.DSAdLoad(null, DSGooglePlayPlugin.ADBannerSize.MEDIUM_RECTANGLE, DSGooglePlayPlugin.adPulug.PreviousPosition());
		}
		if(GUI.Button(new Rect(Screen.width - 180,120, 180, 60), "FULL_BANNER"))
		{
			DSGooglePlayPlugin.adPulug.DSAdLoad(null, DSGooglePlayPlugin.ADBannerSize.FULL_BANNER, DSGooglePlayPlugin.adPulug.PreviousPosition());
		}
		if(GUI.Button(new Rect(Screen.width - 180,180, 180, 60), "LEADERBOARD"))
		{
			DSGooglePlayPlugin.adPulug.DSAdLoad(null, DSGooglePlayPlugin.ADBannerSize.LEADERBOARD, DSGooglePlayPlugin.adPulug.PreviousPosition());
		}
		if(GUI.Button(new Rect(Screen.width - 180,240, 180, 60), "SMART_BANNER"))
		{
			DSGooglePlayPlugin.adPulug.DSAdLoad(null, DSGooglePlayPlugin.ADBannerSize.SMART_BANNER, DSGooglePlayPlugin.adPulug.PreviousPosition());
		}
		/*
		//вызов LARGE_BANNER в Юнити выкидывает ошибку
		if(GUI.Button(new Rect(Screen.width - 180,300, 180, 60), "LARGE_BANNER"))
		{
			DSAdLoad(null, ADBannerSize.MEDIUM_RECTANGLE, null);
			//jo.Call("LoadAd3", "ca-app-pub-4779550302896183/9092560553",  2, (int)adPosition);
		}*/
		
		//Вибро тест
		if(GUI.Button(new Rect(Screen.width - 180,Screen.height - 60, 180, 60), "VIBRO_TEST"))
		{
			Debug.Log("VIBRO BZZZZZZZZZ");
			#if UNITY_ANDROID    
				DSGooglePlayPlugin.adPulug.Vibro(1500);
			#endif
		}

		if(GUI.Button(new Rect(0,Screen.height - 60, 180, 60), "_QUIT"))
		{
			Application.Quit();
		}
			
			
			
			
			

	}
}
