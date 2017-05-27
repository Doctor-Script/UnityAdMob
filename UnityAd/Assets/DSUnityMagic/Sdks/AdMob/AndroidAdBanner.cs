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
				javaAdLoader = new AndroidJavaObject("dsUnityMagic.sdks.adMob.JavaAdBannerLoader");
			}
			#endif

			isAdVisble = true;

			if (javaAdLoader != null) {
				javaAdLoader.Call("loadAd", adUnitId,  (int)adBannerSize, (int)adPosition);
			}
		}

		public override void ChangePosition(ADPosition pos)
		{
			adPosition = pos;
			isAdVisble = true;

			if (javaAdLoader != null) {
				javaAdLoader.Call("setAdPosistion", (int)adPosition);
			}
		}

		public override void SetVisible(bool isVisible)
		{
			if(isVisible && !isAdVisble)
			{
				if (javaAdLoader != null) {
					javaAdLoader.Call ("showAd");
				}
				isAdVisble = true;
			}
			if(!isVisible && isAdVisble)
			{
				if (javaAdLoader != null) {
					javaAdLoader.Call ("hideAd");
				}
				isAdVisble = false;
			}
		}
		
		public override bool GetIsLoading()
		{
			if (javaAdLoader != null) {
				return javaAdLoader.Call<bool>("getIsLoading");
			}
			return false;
		}
		
		public override void Vibration(int delay)
		{
			if (javaAdLoader != null) {
				javaAdLoader.Call("vibrate", delay);
			}
		}
	}
}