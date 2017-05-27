using UnityEngine;
using DSUnityMagic.Sdks.AdMob;

public class DSADPluginDemo : MonoBehaviour
{
	public GameObject freezeIndicator;
	public GameObject responsivenessIndicaor;
	private AdBanner banner;

	void Start()
	{
		Material indicatorMaterial = responsivenessIndicaor.renderer.material;
		indicatorMaterial.color = Color.white;

		banner = AdBannerFactory.GenerateBanner("ca-app-pub-3940256099942544/6300978111");
		banner.LoadAd();
	}

	void Update()
	{
		// Change color for each tap
		if(Input.GetMouseButtonDown(0))
		{
			Material indicatorMaterial = responsivenessIndicaor.renderer.material;

			if (indicatorMaterial.color == Color.white)
			{
				indicatorMaterial.color = Color.black;
			} else {
				indicatorMaterial.color = Color.white;
			}
		}
	}

	void FixedUpdate() 
	{
		if(freezeIndicator != null)
		{
			// On ad loading - indiactor is red 
			if (banner.GetIsLoading()) {
				freezeIndicator.renderer.material.color = Color.red;
			} else {
				freezeIndicator.renderer.material.color = Color.blue;
			}

			// Not freezed
			freezeIndicator.transform.RotateAround(freezeIndicator.transform.position, Vector3.up, 0.99f);
		}
	}

	private void LoadAdBySize(ADSize size) {
		banner.LoadAd(size);
	}

	private void SetPosition(ADPosition position) {
		banner.ChangePosition(position);
	}

	private void SetVisible(bool visible) {
		banner.SetVisible(visible);
	}
	
	void OnGUI()
	{
		// Top
		if(GUI.Button(new Rect(0, 0, 90 , 60), "TOP_LEFT")) {
			SetPosition(ADPosition.TOP_LEFT);
		}
		if(GUI.Button(new Rect(90, 0, 90, 60), "TOP_CENTER")) {
			SetPosition(ADPosition.TOP_CENTER);
		}
		if(GUI.Button(new Rect(180, 0, 90, 60), "TOP_RIGHT")) {
			SetPosition(ADPosition.TOP_RIGHT);
		}

		// Center
		if(GUI.Button(new Rect(0, 60, 90, 60), "CEN_LEFT")) {
			SetPosition(ADPosition.CENTER_LEFT);
		}
		if(GUI.Button(new Rect(90, 60, 90, 60), "CENTER")) {
			SetPosition(ADPosition.CENTER);
		}
		if(GUI.Button(new Rect(180, 60, 90, 60), "CEN_RIGHT")) {
			SetPosition(ADPosition.CENTER_RIGHT);
		}

		// Bottom
		if(GUI.Button(new Rect(0, 120, 90, 60), "BOT_LEFT")) {
			SetPosition(ADPosition.BOTTOM_LEFT);
		}
		if(GUI.Button(new Rect(90, 120, 90, 60), "BOT_CEN")) {
			SetPosition(ADPosition.BOTTOM_CENTER);
		}
		if(GUI.Button(new Rect(180, 120, 90, 60), "BOT_RIGHT")) {
			SetPosition(ADPosition.BOTTOM_RIGHT);
		}

		// Show - hide
		if(GUI.Button(new Rect(0, 180, 135, 60), "ON")) {
			SetVisible(true);
		}
		if(GUI.Button(new Rect(135, 180, 135, 60), "OFF")) {
			SetVisible(false);
		}
			
		// Load size
		if(GUI.Button(new Rect(Screen.width - 180, 0, 180, 60), "BANNER")) {
			LoadAdBySize(ADSize.BANNER);
		}
		if(GUI.Button(new Rect(Screen.width - 180, 60, 180, 60), "MEDIUM_RECTANGLE")) {
			LoadAdBySize(ADSize.MEDIUM_RECTANGLE);
		}
		if(GUI.Button(new Rect(Screen.width - 180, 120, 180, 60), "FULL_BANNER")) {
			LoadAdBySize(ADSize.FULL_BANNER);
		}
		if(GUI.Button(new Rect(Screen.width - 180,180, 180, 60), "LEADERBOARD")) {
			LoadAdBySize(ADSize.LEADERBOARD);
		}
		if(GUI.Button(new Rect(Screen.width - 180, 240, 180, 60), "SMART_BANNER")) {
			LoadAdBySize(ADSize.SMART_BANNER);
		}
//		if(GUI.Button(new Rect(Screen.width - 180,300, 180, 60), "LARGE_BANNER")) {
//			LoadAdBySize(ADSize.MEDIUM_RECTANGLE);//TODO fix crash in Unity
//		}

		if(GUI.Button(new Rect(Screen.width - 180,Screen.height - 60, 180, 60), "VIBRO_TEST")) {  
			banner.Vibration(1500);
		}

		if(GUI.Button(new Rect(0,Screen.height - 60, 180, 60), "QUIT")) {
			Application.Quit();
		}

		StubAdBanner holder = banner as StubAdBanner;
		if (holder != null) {
			holder.OnGUI();
		}
	}
}