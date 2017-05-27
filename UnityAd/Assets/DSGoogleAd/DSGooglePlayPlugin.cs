#define PC_DEBUG1

using UnityEngine;
using System.Collections;



//Трабл
//во время загрузки рекламы, включение и выключение показа игнорируется, но перемешение реюотает, оно будет применено после загрузки


//1. реклама загружена и работает если (isAdVisble && GetLoadingState()) == true

public class DSGooglePlayPlugin : MonoBehaviour {

	/// указатель на единий экземпляр плагина
	public static DSGooglePlayPlugin adPulug;

	/// обьект для доступа к джава классу
	AndroidJavaObject jo;

	/// видимый ли плагин
	protected bool isAdVisble = false;

	/// Ид банера, с сайта адмоб
	public string adUnitId;

	//константы отвечающие за размер банера, соласовано с DisptchSize(int) в джава классе
	public enum ADBannerSize : int
	{
		BANNER = 1,
		MEDIUM_RECTANGLE = 3,
		FULL_BANNER = 4,
		LEADERBOARD = 5,
		SMART_BANNER = 0,
	}
	public ADBannerSize adBannerSize;

	//константы размера, являют собой побитовые сложений класса Grvity, 
	//отвечающие за вертикальное и горизонтальное положение. Константы присоваеваются на прямую в layout.grvity
	public enum ADPosition : int
	{		
		TOP_LEFT = 51,
		TOP_CENTER = 49,
		TOP_RIGHT = 5,
		
		CENTER_LEFT = 19,
		CENTER = 17,
		CENTER_RIGHT = 21,
		
		BOTTOM_LEFT = 80,
		BOTTOM_CENTER = 81,
		BOTTOM_RIGHT = 85,
	}
	public ADPosition adPosition;



	public virtual void Start () {
		//если обьекта еще нету то запомнить указатльль на текущий
		if(adPulug == null)
			adPulug = this;
#if PC_DEBUG
		else //иначе выввести сообщение об ошибке
			Debug.LogError("DS Plugin, Dublicate DSGooglePlayPlugin. Only one, DSGooglePlayDebug or DSGooglePlayPlugin script must be enabled!");
#endif

		#if UNITY_ANDROID    
		jo = new AndroidJavaObject("com.example.googleplayplugin.playads");
		#endif
	}

	/// Позволяет протестировать правльность интеграции плагина, если правильно - завибрирует
	public void Vibro(int delay)
	{
		//вызов вибрации из джава класса
		jo.Call("VibrationTest", delay);
	}

	/// если данный параметр в DSAdLoad(...) должен быть проигнорирован
	public ADBannerSize PreviousSize()
	{
		return adBannerSize;
	}

	/// если данный параметр в DSAdLoad(...) должен быть проигнорирован
	public ADPosition PreviousPosition()
	{
		return adPosition;
	}
	
	/// возвращает true если банер видимый, или загружается
	public bool GetVisible()
	{
		return isAdVisble;
	}

	/// возвращает true если обьявление в процеесе загрузки
	public virtual bool GetLoadingState()
	{
		return jo.Call<bool> ("GetLoadingState");
	}

	/// загрузить новое обьявление с установленными параметрами
	public virtual void DSAdLoad()
	{
		//банер теперь видимый
		isAdVisble = true;
		//вызов загрузки из джава
		jo.Call("AdLoad", adUnitId,  (int)adBannerSize, (int)adPosition);
	}

	/// загрузить обьявление с новыми параметрами
	public virtual void DSAdLoad(string id, ADBannerSize size, ADPosition pos)
	{
		//если в параметр задано null то игнориваровать его
		if (id != null) {
			adUnitId = id;
		}

		//запомнить размер, игнорируется если как параметр передано PreviousSize()
		adBannerSize = size;

		//запомниь позицию, игнорируется если как параметр передано PreviousPosition()
		adPosition = pos;

		//банер теперь видимый
		isAdVisble = true;
		//вызов загрузки из джава
		jo.Call("AdLoad", adUnitId,  (int)adBannerSize, (int)adPosition);
	}

	/// изменить позицию, без перезагрузки банера
	public virtual void DSAdSetPosition(ADPosition pos)
	{
		//запомнть позицию
		adPosition = pos;

		//банер теперь видимый
		isAdVisble = true;
		//вызов метода джавы для перемещения банера
		jo.Call("AdSetPosistion",(int)adPosition);
	}

	//включить или отключить показ обьявлений
	public virtual void DSAdSetVisible(bool setVisible)
	{
		//если нужно включить и было выключено
		if(setVisible && !isAdVisble)
		{
			//вызвать метод из библиотки на джаве
			jo.Call ("AdShow");
			//устанвить как видимый
			isAdVisble = true;
		}
		//если нужно выключить и было включено
		if(!setVisible && isAdVisble)
		{
			//вызвать метод из библиотки на джаве
			jo.Call ("AdHide");
			//устанвить как видимый
			isAdVisble = false;
		}
	}
}
