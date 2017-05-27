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

//������
//����� - ������� ������ ������ ������� (*1*)
//�������� ������ AdView, ���� ���� ���� ������ (*2*)


//������� ����� ����� ����� ��
//http://fastegggames.com/blog/2014/3/16/tutorial-google-play-ads-with-admob-in-unity
public class playads {
	//��� � ������, ���������������� ��� ����, �������� ���������� ����� �������� ��� ������ ��������
	private String adUnitID = "ca-app-pub-4779550302896183/9092560553";
	//�������� ��������� - �� �����
    private Activity activity; //Store the android main activity
    //AdView - ������ ��� ����������� �������
    private AdView adView; //The AdView we will display to the user
    //���� �������� ��� ������ (�� ��� ����� ����)
    private LinearLayout layout; //The layout the AdView will sit on
    

//int graityOfLauout1;
    //������� ������ (���� �������� Gravity), ����������� ��� ������ � ������ �������
    int currentPos;
    //������ ������ (��� �������������� DispatchSize � ���������� ������ ������ ���� AdSize, 
    //����������� ��� ������ � ������ �������
    int sizeOfBanner;

    //true ����� ���������� ��������� ������ � ������� �������� ����������
    boolean isLoading;

    //����������� ������
    public playads () {
    	//�������� ������� �������� �� ���������� �� �����
    	activity = UnityPlayer.currentActivity;
    	
    	//�� ��� ��� ��� �������������, ���� ��� ������ ���������� � ���������
    	sizeOfBanner = 1;
    	currentPos = 80;
    	//���������� ������� ����� ���
    	isLoading = false;
    }
 
    //�������� ������� �������� ����������
    public void AdLoad(String id, int bannersize, int layoutGraity)
    {
    	//���� ������� ������� �� ���������� ������� ������� ���������� - � �������� ��������
    	isLoading = true;

    	//��� ��������� ������� ������������ � ������������ ������ (�� �� ���, ���� �� ��������)
    	//� ��������� ������ ������� ���� �� �������
    	currentPos = layoutGraity;
	  	sizeOfBanner = bannersize;
	  	adUnitID = id;
	  	
	  	//������ ������������ ������
	  	activity.runOnUiThread(new Runnable() {
	  		public void run(){
	  			
	  			//�������� ������ AdView, ���� ���� ���� ������ (*2*)
	            adView = new AdView(activity);
	            
	           //��������� Id ����������� ������
	            adView.setAdUnitId(adUnitID);

	            //��������� ������� ������ (DispatchSize ������������ ��� �� �����)
	            adView.setAdSize(DispatchSize(sizeOfBanner));
	      
	            //����������� �������, ������� ���������� ��� ��������� �������
	            AdRequest request = new AdRequest.Builder().build();
	            
	            //AdListener ����������� ������� �������
	            adView.setAdListener(new AdListener() {
	            	//���������� ����� ������� ���������
					public void onAdLoaded() {
						
						
						if(layout == null)//���� layout ��� �� ������
						{
							//����� ��� ������ ������ ��� ���������� adView � layout
							 activity.runOnUiThread(new Runnable() {
								public void run(){
									
									//������� ��������� layout (�.�. ��� ��� ����)
									layout = new LinearLayout(activity);
									
									//�����������  adView � layout
									layout.addView(adView);
									//�� ���� ��������� layout � activity, �� ������ ������� ��� �������
									activity.addContentView(layout, new LayoutParams(LayoutParams.WRAP_CONTENT,LayoutParams.WRAP_CONTENT));
								}
							 });
						}
						//�����, ���� layout ��� ������
						else 
						{
							//���� ������
							//������� ������� ����� ������ (*1*)
							layout.removeAllViews();
							layout.addView(adView);
						}
						
						//�������� ������� ������
						((FrameLayout.LayoutParams)layout.getLayoutParams()).gravity = currentPos;

						//������ �������� - �������� ���������
						isLoading = false;
					}
	            });
	            
	            //����������� ������� � ����������� ��������� ������
	            adView.loadAd(request);
	        }
	  	});
	}
    
    //���������� true ���� ������� � �������� �������� (������� ������� ����������)
    public boolean GetLoadingState()
    {
    	//���������� ������ ��������
    	return isLoading;
    }
    
    
    //AdSize ������� ���, � � ����� ������ ���� - � ���������� ��� ��� �������� �� ���������
    //�� ����� �� ��� ��������� (int) ���, � ������� ���������, � ������ ����� ������������ ���
    AdSize DispatchSize(int size)
    {
    	//������ ��� �������� � �����)
    	AdSize adsz;
    	//����� ������� �������(�������� �������� � ������������
    	//https://developers.google.com/mobile-ads-sdk/docs/admob/intermediate?hl=ru-ru
    	switch(size)
    	{
    		case 1:  adsz = AdSize.BANNER;
    		break;
//    		case 2:  adsz = AdSize.LARGE_BANNER;//��� �������� ����� �������� (������ �� �����)
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
    
    //�������� �������. ����������� �� ������� ��������� �� ��� ���������� ������� ��������
    //�������� ������� ����� �� ������������ � Gravity. ��������� ������������� � ��������������� ���������
    //�������� ������������ (| - ��������� ���: Gravity.TOP | Gravity.RIGHT)
    public void AdSetPosistion(int pos)
    {
    	//���������� ��� ��������� � ������ ������
    	currentPos = pos;

    	//���� ������� � �������� �������� �� ���������� ����� ������� � ������ �� �������� 
    	if(isLoading) return;
    	//����� ������
    	activity.runOnUiThread(new Runnable() {
	        public void run(){
	        	
	        	//���� ������
				//������� ������� ����� ������ (*1*)
			  	layout.removeAllViews();
			  	layout.addView(adView);
			  	//�������� ������ �������
			    ((FrameLayout.LayoutParams)layout.getLayoutParams()).gravity = currentPos;
	        }
    	});
    	
    }
  
    //�� ���������� ������� - ������� �� layout
    public void AdHide()
    {
    	//������� �� �������� ���� ������� � �������� ��������
    	if(isLoading) return;
    	
    	//����� ������
    	activity.runOnUiThread(new Runnable() {
	        public void run(){
	        	//�������� adView (������ � layout �������� ������ ����)
	            layout.removeAllViews();
	        }});
    }
    
    //���������� ������� - �������� adView � layout
    public void AdShow()
    {
    	//������� �� �������� ���� ������� � �������� ��������
    	if(isLoading) return;
    	
    	//����� ������
	    activity.runOnUiThread(new Runnable() {
	        public void run(){
	        	
	        	//�������� adView � layout
	            layout.addView(adView);

	        }});
    }
    
    //���� ����� �������� �������� �� ������� �����
    //�������� ������ ��������� ������� ����� � ����������� �� �����
    public void VibrationTest(int delay)
    {
    	//��������� ��������� �� ��������
    	Vibrator vbr = (Vibrator) activity.getSystemService(Context.VIBRATOR_SERVICE);
		 
		//��������������� ��������� ��������
		vbr.vibrate(delay);
    }

}
