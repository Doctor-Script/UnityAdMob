package googleExample;

import com.google.android.gms.ads.AdRequest;
import com.google.android.gms.ads.AdSize;
import com.google.android.gms.ads.AdView;
import com.google.example.gms.ads.banner.R;

import android.app.Activity;
import android.os.Bundle;
import android.widget.LinearLayout;

/**
 * A simple {@link Activity} that embeds an AdView.
 */
public class BannerSample extends Activity
{
	private static final String AD_UNIT_ID = "ca-app-pub-3940256099942544/6300978111";
	private AdView adView;

	@Override
	public void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		adView = new AdView(this);
		adView.setAdSize(AdSize.BANNER);
		adView.setAdUnitId(AD_UNIT_ID);
		
		LinearLayout layout = (LinearLayout) findViewById(R.id.linearLayout);
		layout.addView(adView);
		
		AdRequest adRequest = new AdRequest.Builder().build();
		
		adView.loadAd(adRequest);
	}

	@Override
	public void onResume()
	{
		super.onResume();
		
		if (adView != null) {
			adView.resume();
		}
	}

	@Override
	public void onPause()
	{
		if (adView != null) {
			adView.pause();
		}
		
		super.onPause();
	}

	@Override
	public void onDestroy()
	{
		if (adView != null) {
			adView.destroy();
		}
		
		super.onDestroy();
	}
}