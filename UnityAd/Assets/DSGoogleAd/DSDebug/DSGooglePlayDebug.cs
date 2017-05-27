using UnityEngine;
using System.Collections;

public class DSGooglePlayDebug : DSGooglePlayPlugin//MonoBehaviour 
{
	public float adReloadTime = 4f;
	//true до первого вызова DSAdLoad
	bool firstLoad = true;
	//показывть ли сообщения
	public bool displayMessages = true;

	public bool displayAd = true;

	//масив строк текста
	public string[] textArr = new string[]{
		
		"Bugs that go away by themselves come back by themselves.",
		"When in doubt, use brute force.",
		"Deleted code is debugged code.",
		"Premature optimization is the root of all evil.",
		"Simplicity is the ultimate sophistication.",
		"With diligence it is possible to make anything run slowly.",
		"Simplicity carried to the extreme becomes elegance.",
		"The best is the enemy of the good.",
		"A data structure is just a stupid programming language.",
		"Software gets slower faster than hardware gets faster.",
		"If it doesn't work, it doesn't matter how fast it doesn't work.",
		"If it works, it's obsolete.",
		"The common language of programmers is Profanity.",
		"There is no place like 127.0.0.1.",
		"The code is 100% complete, it just doesn't work yet.",
		"Programming is hard, let's go shopping."
	};
	//массив иконок
	public Texture[]		iconArr					= new Texture[0];
	//массив текстур действия
	public Texture[]		actionArr				= new Texture[0];


	//фон рекламного обьявления
	public Texture background;

	//укатели на текущие значения ресурсов отображаемых на банере (две текстуры и текст)
	Texture curAction;
	Texture curIcon;
	string curText;

	//таймер и врема до выполнения
	float timer, critTime;
	//использовать таймер
	bool useTimer = false;
	//true после первой загрузки обьявления (когда появится)
	bool realLoaded = false;

	//прямоугольнить в котором рисуется банер
	Rect backgroundRect;

	//реклама в процессе загрузки
	bool isLoading = false;



	// Use this for initialization
	public override void Start () {
		//одиночка, для получения простого доступа
		if(adPulug == null)
			adPulug = this;
		else 
			if(displayMessages) 
				Debug.LogError("DS Debug, Dublicate DSGooglePlayPlugin. Only one, DSGooglePlayDebug or DSGooglePlayPlugin script must be enabled!");
	}


	//возвращает статус загрузки - true если загружается
	public override bool GetLoadingState()
	{
		return isLoading;
	}


	//загрузить обьявление по заданым параметрам
	public override void DSAdLoad()
	{
		//Пустой текст бывает в начале если не заполнить соответсвующее поле adUnitId
		if (adUnitId == "") {
			if(displayMessages)
				Debug.LogError("DSAdLoad() Load FAILED. adUnitId is empty. You mast enter Ad Unit Id to 'adUnitId'"); 
			return;
		}

		//может какой мудак обнулит adUnitId вручную
		if (adUnitId == null) {
			if(displayMessages)
				Debug.LogError("DSAdLoad() Load FAILED. Value 'null' fild adUnitId is not allowed"); 
			return;
		}

		//эхо о запущеном интерфейсном методе
		if (displayMessages) {
			Debug.Log ("DSPlugin Debug: DSAdLoad()");
		}

		//запуск таймера на загрузку рекламы
		GetNewAd();

		//банер теперь отображается
		isAdVisble = true;

		//загружено
		firstLoad = false;
	}



	//загрузить обьявление с новыми параметрами
	public override void DSAdLoad(string id, ADBannerSize size, ADPosition pos)
	{
		//если id != null то заомнить новое значение в adUnitId
		if (id != null) {
			adUnitId = id;
		}
		else{//при первоз загрузке нельзя задавать параметр id как null
			if(firstLoad)
			{
				if(displayMessages)
					Debug.LogError("DSAdLoad(...) Load FAILED. At the first load Ad value 'null' fild adUnitId is not allowed"); 
				return;
			}
		}

		//запомнить размер
		adBannerSize = size;

		//запомнить позицию
		adPosition = pos;

		//Пустой текст бывает в начале если не заполнить соответсвующее поле adUnitId
		if (adUnitId == "") {
			if(displayMessages)
				Debug.LogError("DSAdLoad(...) Load FAILED. adUnitId is empty. You mast enter Ad Unit Id to 'adUnitId'"); 
			return;
		}

		//эхо о запущеном интерфейсном методе
		if (displayMessages) {
			Debug.Log ("DSPlugin Debug: DSAdLoad(string, ADBannerSize, ADPosition)");
		}

		//запуск таймера на загрузку рекламы
		GetNewAd();
		
		//банер теперь отображается
		isAdVisble = true;
		
		//загружено
		firstLoad = false;
	}



	//Изменить позицию банера
	public override void DSAdSetPosition(ADPosition pos)
	{
		//показать сообщения
		if (displayMessages) 
		{
			//если реклама еще не загружена
			if(firstLoad)
			{
				Debug.LogWarning("DSAdSetPosition(ADPosition) FAILED. Ad sill not loaded! You have to call 'DSAdLoad()' first");
				return;
			}
			//если загружена то отобразить эхо о запущеном интерфейсном методе
			else Debug.Log ("DSPlugin Debug: DSAdSetPosition(ADPosition)");
		}

		//заомнить позицию
		adPosition = pos;

		if(!isLoading)
			//изменить координаты банера
			backgroundRect = GetAdRect (adPulug.adBannerSize, adPulug.adPosition);

		//банер теперь отображается
		isAdVisble = true;

	}



	//включить или отключить показ
	public override void DSAdSetVisible(bool isVisible)
	{
		//если реклама еще не загружена
		if (displayMessages) {
			if(firstLoad)
			{
				Debug.LogWarning("DSAdSetVisible(bool) FAILED. Ad sill not loaded! You have to call 'DSAdLoad()' first");
				return;
			}//Эхо
			else Debug.Log ("DSPlugin Debug: DSAdSetVisible(bool)");
		}

		if(isLoading) return;

		//включить
		if(isVisible && !isAdVisble)
		{
			if (displayMessages) Debug.Log("____________________________DSAdSetVisible(true)");
			isAdVisble = true;
		}
		//выключить
		if(!isVisible && isAdVisble)
		{
			if (displayMessages) Debug.Log("____________________________DSAdSetVisible(false)");
			isAdVisble = false;
		}
	}


	//установить размер и позицию по константам
	private Rect GetAdRect(DSGooglePlayPlugin.ADBannerSize size, DSGooglePlayPlugin.ADPosition horizontalPosition)//, AdVerticalPosition verticalPosition)
	{
		
		float x = 0, y = 0, width = 0, height = 0;


		//установка размера
		switch(size){
			
		case DSGooglePlayPlugin.ADBannerSize.BANNER:
			width	= 320;
			height	= 50;
			break;
			
		case DSGooglePlayPlugin.ADBannerSize.MEDIUM_RECTANGLE://AdSize.IAB_BANNER:
			width	= 300;
			height	= 250;
			break;
			
		case DSGooglePlayPlugin.ADBannerSize.FULL_BANNER://AdSize.IAB_LEADERBOARD:
			width	= 486;
			height	= 60;
			break;
			
		case DSGooglePlayPlugin.ADBannerSize.LEADERBOARD://AdSize.IAB_MRECT:
			width	= 728;
			height	= 90;
			break;
			
		case DSGooglePlayPlugin.ADBannerSize.SMART_BANNER://AdSize.SMART_BANNER:
			width	= Screen.width;
			height	= 32;
			break;
		}
		
		if(width > Screen.width){
			
			width = Screen.width;
		}
		
		if(height > Screen.height){
			
			height = Screen.height;
		}




		//установка позиции
		switch(horizontalPosition)
		{
			
		
		case DSGooglePlayPlugin.ADPosition.TOP_LEFT:
			x = 0;
			y = 0;
			break;

		case DSGooglePlayPlugin.ADPosition.TOP_CENTER:
			x = (Screen.width / 2) - (width / 2);
			y = 0;
			break;

		case DSGooglePlayPlugin.ADPosition.TOP_RIGHT:
			x = Screen.width - width;
			y = 0;
			break;



		case DSGooglePlayPlugin.ADPosition.CENTER_LEFT:
			x = 0;
			y = (Screen.height / 2) - (height / 2);
			break;

		case DSGooglePlayPlugin.ADPosition.CENTER:
			x = (Screen.width / 2) - (width / 2);
			y = (Screen.height / 2) - (height / 2);
			break;

		case DSGooglePlayPlugin.ADPosition.CENTER_RIGHT:
			x = Screen.width - width;
			y = (Screen.height / 2) - (height / 2);
			break;




			
		case DSGooglePlayPlugin.ADPosition.BOTTOM_LEFT:
			x = 0;
			y = Screen.height - height;
			break;
			
		case DSGooglePlayPlugin.ADPosition.BOTTOM_CENTER:
			x = (Screen.width / 2) - (width / 2);
			y = Screen.height - height;
			break;

		case DSGooglePlayPlugin.ADPosition.BOTTOM_RIGHT:
			x = Screen.width - width;
			y = Screen.height - height;
			break;

		}

		return( new Rect(x, y, width, height) );
	}


	//выбрать рандомный элемент из массива
	private T GetRandomElement<T>(T[] array) where T : class{
		
		int index, length;
		
		length = ( array == null ? 0 : array.Length );
		
		index = Random.Range(0, length);
		
		return(length == 0 ? null : array[index]);
	}


	//запустить таймер на загрузку обьявлнеий
	void GetNewAd()
	{
		useTimer = true;
		timer = 0;
		critTime = adReloadTime;
		isLoading = true;
	}


	//функция обновления таймера
	void UpdateTimer()
	{

		
		//если таймер достиг заданого времени
		if(timer > critTime)
		{
			realLoaded = true;
			
			//перезагрузить обьявление
			backgroundRect = GetAdRect (adPulug.adBannerSize, adPulug.adPosition);
			curText = GetRandomElement(textArr);
			curAction = GetRandomElement(actionArr);
			curIcon = GetRandomElement(iconArr);
			
			//обнулить таймер
			timer = 0f;
			
			isLoading = false;
			
			//и поставить на обновление обьявлений (в реале оно само, частота задается через сайт)
			critTime = 30f;
		}
	}



	void Update () {

		//if(Time.timeScale == 0.0f){
			//обновить таймер
			if(useTimer)
				timer += Time.deltaTime;


			//обновить таймер
			UpdateTimer ();
		//}
	}

	/*
	//если игра на паузе (Time.timeScale == 0.0f) то банер не заустися, FixedUpdate () не будет выполнятся
	void FixedUpdate () {

		//обновить таймер
		if(useTimer)
			timer += Time.fixedDeltaTime;


		//обновить таймер
		UpdateTimer ();
	}*/


	//банер рисоется как гуи
	void OnGUI()
	{
		if(!displayAd) return;

		if(isAdVisble && realLoaded)
		{
			//построение прямоугольников для отоборажения
			Rect	iconRect		= new Rect(backgroundRect.x + 4, backgroundRect.y + 4, 38, 38);
			Rect	actionRect		= new Rect(backgroundRect.x + backgroundRect.width - 34, backgroundRect.y + 4, 30, 30);
			Rect	textRect		= new Rect(backgroundRect.x + 4 + 38 + 4, backgroundRect.y + 4, backgroundRect.width - 4 - 38 - 4 - 4 - 30 - 4, backgroundRect.height - 8);

			//отрисовка фона
			GUI.DrawTexture(backgroundRect, background);

			//отрисовка иконки
			if(curIcon != null){
				GUI.DrawTexture(iconRect, curIcon);
			}

			//отрисовка дейвия
			if(curAction != null){
				GUI.DrawTexture(actionRect, curAction);
			}

			//отрисовка текста
			if(curText != null){

				GUIStyle textStyle = new GUIStyle();
				
				textStyle.normal.textColor	= Color.black;
				textStyle.fontStyle			= FontStyle.Bold;
				textStyle.wordWrap			= true;
				textStyle.alignment			= TextAnchor.MiddleCenter;
				textStyle.normal.textColor	= Color.white;//textColor;

				GUI.Label(textRect, curText, textStyle);
			}
		}
	}
}
