using UnityEngine;

namespace DSUnityMagic.Sdks.AdMob
{
	public static class AdBannerFactory
	{
		public static AdBanner GenerateBanner(string adUnitId)
		{
			if (Application.platform == RuntimePlatform.Android) {
				return new AndroidAdBanner(adUnitId);
			}
			return new StubAdBanner();
		}
	}
}