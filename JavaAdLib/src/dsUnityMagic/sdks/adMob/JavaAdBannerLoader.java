package dsUnityMagic.sdks.adMob;

import com.google.android.gms.ads.*;
import com.unity3d.player.UnityPlayer;

import android.app.Activity;
import android.content.Context;
import android.widget.FrameLayout;
import android.widget.LinearLayout;
import android.os.Vibrator;
import android.view.ViewGroup.LayoutParams;

public class JavaAdBannerLoader
{
	private String adUnitID;
	private Activity currentUnityActivity;
	private AdView adView;
	private LinearLayout layout;

	int currentAdPosition;
	int currentAdSize;

	// true when request is sended and it waits answer
	boolean isLoading;

	public JavaAdBannerLoader()
	{
		currentUnityActivity = UnityPlayer.currentActivity;
		currentAdSize = 1;
		currentAdPosition = 80;
		isLoading = false;
	}
	
	public void loadAd(String id, int bannersize, int layoutGraity)
	{
		isLoading = true;
		currentAdPosition = layoutGraity;
		currentAdSize = bannersize;
		adUnitID = id;
		
		currentUnityActivity.runOnUiThread(new Runnable()
		{
			public void run()
			{
				if (adView != null) {
					adView.destroy();
				}
				
				adView = new AdView(currentUnityActivity);
				adView.setAdUnitId(adUnitID);
				adView.setAdSize(dispatchSize(currentAdSize));
				AdRequest request = new AdRequest.Builder().build();
				
				// AdListener handles ad events
				adView.setAdListener(new JavaAdBannerListener());
				adView.loadAd(request);
			}
		});
	}
	
	private class JavaAdBannerListener extends AdListener
	{
		public void onAdLoaded()
		{
			if(layout == null)
			{
				currentUnityActivity.runOnUiThread(new Runnable()
				{
					public void run()
					{
						layout = new LinearLayout(currentUnityActivity);
						layout.addView(adView);
						LayoutParams params = new LayoutParams(LayoutParams.WRAP_CONTENT,
								LayoutParams.WRAP_CONTENT);
						currentUnityActivity.addContentView(layout, params);
					}
				});
			} else {
				layout.removeAllViews();
				layout.addView(adView);
			}

			((FrameLayout.LayoutParams)layout.getLayoutParams()).gravity = currentAdPosition;
			isLoading = false;
		}
	}

	public boolean getIsLoading() {
		return isLoading;
	}

	private AdSize dispatchSize(int size)
	{
		AdSize adSize;
		switch(size)
		{
			case 1: adSize = AdSize.BANNER;
			break;
//			case 2: adSize = AdSize.LARGE_BANNER;// TODO fix crash in Unity
//			break;
			case 3: adSize = AdSize.MEDIUM_RECTANGLE;
			break;
			case 4:  adSize = AdSize.FULL_BANNER;
			break;
			case 5:  adSize = AdSize.LEADERBOARD;
			break;
			default: adSize = AdSize.SMART_BANNER;
			break;
		}
		return adSize;
	}

	// Unity sends correct values: Bitwise or of Gravity constants
	// Like a Gravity.TOP | Gravity.RIGHT
	public void setAdPosistion(int adPosition)
	{
		currentAdPosition = adPosition;

		if(isLoading) {
			return;
		}
		
		currentUnityActivity.runOnUiThread(new Runnable()
		{
			public void run()
			{
				layout.removeAllViews();
				layout.addView(adView);
				((FrameLayout.LayoutParams)layout.getLayoutParams()).gravity = currentAdPosition;
			}
		});
	}

	public void hideAd()
	{
		if(isLoading) {
			return;
		}
		
		currentUnityActivity.runOnUiThread(new Runnable()
		{
			public void run() {
				layout.removeAllViews();
			}
		});
	}

	public void showAd()
	{
		if(isLoading) {
			return;
		}
		
		currentUnityActivity.runOnUiThread(new Runnable()
		{
			public void run() {
				layout.addView(adView);
			}
		});
	}

	public void vibrate(int delay)
	{
		Vibrator vibrator = (Vibrator)currentUnityActivity.getSystemService(Context.VIBRATOR_SERVICE);
		vibrator.vibrate(delay);
	}
}