using System;
using UnityEngine;

namespace DSUnityMagic.Sdks.AdMob
{
	[Serializable]
	public class AndroidAdBanner : AdBanner
	{
		private AndroidJavaObject javaAdLoader;

		[SerializeField]
		private string adUnitId;

		public AndroidAdBanner(string adUnitId) {
			this.adUnitId = adUnitId;
		}

		public override void LoadAd(ADSize size, ADPosition pos)
		{
			adBannerSize = size;
			adPosition = pos;
			LoadAd();
		}

		public override void LoadAd(ADSize size)
		{
			adBannerSize = size;
			LoadAd();
		}

		public override void LoadAd()
		{
			// Not in constructor, because can be called before AndroidJavaObject can be loaded
			#if UNITY_ANDROID
			if (javaAdLoader == null && Application.platform == RuntimePlatform.Android) {
				javaAdLoader = new AndroidJavaObject("com.example.googleplayplugin.playads");
			}
			#endif

			isAdVisble = true;

			if (javaAdLoader != null) {
				javaAdLoader.Call("AdLoad", adUnitId,  (int)adBannerSize, (int)adPosition);
			}
		}

		public override void ChangePosition(ADPosition pos)
		{
			adPosition = pos;
			isAdVisble = true;

			if (javaAdLoader != null) {
				javaAdLoader.Call("AdSetPosistion",(int)adPosition);
			}
		}

		public override void SetVisible(bool isVisible)
		{
			if(isVisible && !isAdVisble)
			{
				if (javaAdLoader != null) {
					javaAdLoader.Call ("AdShow");
				}
				isAdVisble = true;
			}
			if(!isVisible && isAdVisble)
			{
				if (javaAdLoader != null) {
					javaAdLoader.Call ("AdHide");
				}
				isAdVisble = false;
			}
		}
		
		public override bool GetLoadingState()
		{
			if (javaAdLoader == null) {
				return javaAdLoader.Call<bool>("GetLoadingState");
			}
			return false;
		}
		
		public override void Vibration(int delay)
		{
			if (javaAdLoader != null) {
				javaAdLoader.Call("VibrationTest", delay);
			}
		}
	}
}