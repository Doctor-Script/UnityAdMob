package com.example.googleplayplugin;

import com.google.android.gms.ads.*;
import com.unity3d.player.UnityPlayer;

import android.app.Activity;
import android.content.Context;
import android.widget.FrameLayout;
import android.widget.LinearLayout;
import android.os.Vibrator;
//import android.view.Gravity;
import android.view.ViewGroup.LayoutParams;

//Траблы
//РЕСЕТ - сначала удалил потому добавил (*1*)
//создание нового AdView, ДАЖЕ ЕСЛИ ЕСТЬ СТАРЫЙ (*2*)


//большая часть инифы взята из
//http://fastegggames.com/blog/2014/3/16/tutorial-google-play-ads-with-admob-in-unity
public class playads {
	//код с банера, инициализироване для виду, всеравно передается через парамтры при вызове загрузки
	private String adUnitID = "ca-app-pub-4779550302896183/9092560553";
	//основная активност - из Юнити
    private Activity activity; //Store the android main activity
    //AdView - виджет для отображения рекламы
    private AdView adView; //The AdView we will display to the user
    //типа разметки для банера (он тут будет один)
    private LinearLayout layout; //The layout the AdView will sit on
    

//int graityOfLauout1;
    //Поизция банера (коды констант Gravity), сохраняется для работы в других потоках
    int currentPos;
    //Размер банера (код обрабатывается DispatchSize и возвращает размер банера типа AdSize, 
    //сохраняется для работы в других потоках
    int sizeOfBanner;

    //true когда приложение отправило запрос и ожидает загрузки обьявления
    boolean isLoading;

    //Конструктор класса
    public playads () {
    	//получает текущую активити из приложения на юнити
    	activity = UnityPlayer.currentActivity;
    	
    	//хз нах они ети инициализации, если все данные передаются в параметры
    	sizeOfBanner = 1;
    	currentPos = 80;
    	//изначально зарузки точно нет
    	isLoading = false;
    }
 
    //ОСНОВНАЯ функция загрузки обьявлений
    public void AdLoad(String id, int bannersize, int layoutGraity)
    {
    	//если функция вызвана то приложение ожидает прихода обьявления - в процессе загрузки
    	isLoading = true;

    	//Вся настройка рекламы производится в паралельныом потоке (хз чо так, взял из примеров)
    	//и параметры данной функции туда не достают
    	currentPos = layoutGraity;
	  	sizeOfBanner = bannersize;
	  	adUnitID = id;
	  	
	  	//запуск паралельного потока
	  	activity.runOnUiThread(new Runnable() {
	  		public void run(){
	  			
	  			//создание нового AdView, ДАЖЕ ЕСЛИ ЕСТЬ СТАРЫЙ (*2*)
	            adView = new AdView(activity);
	            
	           //установка Id конкретного банера
	            adView.setAdUnitId(adUnitID);

	            //установка размера банера (DispatchSize обрабатывает код из юнити)
	            adView.setAdSize(DispatchSize(sizeOfBanner));
	      
	            //постороение запроса, который отсылается для получения рекламы
	            AdRequest request = new AdRequest.Builder().build();
	            
	            //AdListener отслежывает события рекламы
	            adView.setAdListener(new AdListener() {
	            	//вызывается когда реклама загружена
					public void onAdLoaded() {
						
						
						if(layout == null)//если layout еще не создан
						{
							//вызов еще одного потока для добавления adView в layout
							 activity.runOnUiThread(new Runnable() {
								public void run(){
									
									//сначала создается layout (т.к. его еще нету)
									layout = new LinearLayout(activity);
									
									//добавляется  adView в layout
									layout.addView(adView);
									//по ходу добавляет layout в activity, ну тоесть делатет его видимым
									activity.addContentView(layout, new LayoutParams(LayoutParams.WRAP_CONTENT,LayoutParams.WRAP_CONTENT));
								}
							 });
						}
						//иначе, если layout уже создан
						else 
						{
							//типа РЕСЕТА
							//сначала добавил потом удалил (*1*)
							layout.removeAllViews();
							layout.addView(adView);
						}
						
						//устанока позиции банера
						((FrameLayout.LayoutParams)layout.getLayoutParams()).gravity = currentPos;

						//данные получены - загрузка завершена
						isLoading = false;
					}
	            });
	            
	            //отправление запроса с последующей загрузкой банера
	            adView.loadAd(request);
	        }
	  	});
	}
    
    //возвращает true если функция в процессе загрузки (ожидает прихода обьявления)
    public boolean GetLoadingState()
    {
    	//возврацает статус загрузки
    	return isLoading;
    }
    
    
    //AdSize сложный тип, и в Юнити такого нету - и передавать его как параметр не получится
    //по этому из Юни передаеся (int) код, с номером константы, а данный метод разшифровует его
    AdSize DispatchSize(int size)
    {
    	//размер для возврата в конце)
    	AdSize adsz;
    	//поиск нужного размера(описание констант в документации
    	//https://developers.google.com/mobile-ads-sdk/docs/admob/intermediate?hl=ru-ru
    	switch(size)
    	{
    		case 1:  adsz = AdSize.BANNER;
    		break;
//    		case 2:  adsz = AdSize.LARGE_BANNER;//при загрузке этого ВЫЛЕТАЕТ (только из Юнити)
//    		break;
    		case 3:  adsz = AdSize.MEDIUM_RECTANGLE;
    		break;
    		case 4:  adsz = AdSize.FULL_BANNER;
    		break;
    		case 5:  adsz = AdSize.LEADERBOARD;
    		break;
    		default: adsz = AdSize.SMART_BANNER;
          break;
    	}
    	return adsz;
    }
    
    //Устанока позиции. Расщифровки не требует поскольку из Юни передаются готовые кностаны
    //Значение констан взято из документации к Gravity. Константы вертикального и горизонтального положения
    //поибтово скалдываются (| - побитовое или: Gravity.TOP | Gravity.RIGHT)
    public void AdSetPosistion(int pos)
    {
    	//запоминает для обработки в другом потоке
    	currentPos = pos;

    	//если реклама в процессе загрузки то запоминает новую позицию и дальше не работает 
    	if(isLoading) return;
    	//вызов потока
    	activity.runOnUiThread(new Runnable() {
	        public void run(){
	        	
	        	//типа РЕСЕТА
				//сначала добавил потом удалил (*1*)
			  	layout.removeAllViews();
			  	layout.addView(adView);
			  	//устанока нужной позиции
			    ((FrameLayout.LayoutParams)layout.getLayoutParams()).gravity = currentPos;
	        }
    	});
    	
    }
  
    //НЕ показывать рекламу - удалить из layout
    public void AdHide()
    {
    	//функция не работает если реклама в процессе загрузки
    	if(isLoading) return;
    	
    	//вызов потока
    	activity.runOnUiThread(new Runnable() {
	        public void run(){
	        	//удаление adView (больше в layout всеравно ничего нету)
	            layout.removeAllViews();
	        }});
    }
    
    //показывать рекламу - добавить adView в layout
    public void AdShow()
    {
    	//функция не работает если реклама в процессе загрузки
    	if(isLoading) return;
    	
    	//вызов потока
	    activity.runOnUiThread(new Runnable() {
	        public void run(){
	        	
	        	//добавить adView в layout
	            layout.addView(adView);

	        }});
    }
    
    //типа бонус включает вибрацию на заданое время
    //помогает быстро проверить наличие связи с библиотекой из Юнити
    public void VibrationTest(int delay)
    {
    	//Поулчение указателя на вибратор
    	Vibrator vbr = (Vibrator) activity.getSystemService(Context.VIBRATOR_SERVICE);
		 
		//непосредственно включение вибрации
		vbr.vibrate(delay);
    }

}
