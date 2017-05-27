using System;
using UnityEngine;

namespace DSUnityMagic.Sdks.AdMob
{
	[Serializable]
	public class StubAdBanner : AdBanner
	{
		private float width;
		private float height;
		private Rect rect;
		
		public override void LoadAd(ADSize size, ADPosition pos) {
			LoadAd(size);
		}
		
		public override void LoadAd(ADSize size)
		{
			switch (size)
			{
			case ADSize.BANNER:
				width	= 320;
				height	= 50;
				break;
				
			case ADSize.MEDIUM_RECTANGLE:
				width	= 300;
				height	= 250;
				break;
				
			case ADSize.FULL_BANNER:
				width	= 486;
				height	= 60;
				break;
				
			case ADSize.LEADERBOARD:
				width	= 728;
				height	= 90;
				break;
				
			case ADSize.SMART_BANNER:
				width	= Screen.width;
				height	= 32;
				break;
			}

			isAdVisble = true;
			adBannerSize = size;
			ChangePosition(adPosition);
		}
		
		public override void LoadAd() {
			LoadAd(adBannerSize);
		}
		
		public override void ChangePosition(ADPosition pos)
		{
			adPosition = pos;

			switch (pos)
			{
			case ADPosition.TOP_LEFT:
				rect = new Rect(0, 0, width, height);
				break;
			case ADPosition.TOP_CENTER:
				rect = new Rect((Screen.width - width) / 2, 0, width, height);
				break;
			case ADPosition.TOP_RIGHT:
				rect = new Rect(Screen.width - width, 0, width, height);
				break;
				
			case ADPosition.CENTER_LEFT:
				rect = new Rect(0, (Screen.height - height) / 2, width, height);
				break;
			case ADPosition.CENTER:
				rect = new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height);
				break;
			case ADPosition.CENTER_RIGHT:
				rect = new Rect(Screen.width - width, (Screen.height - height) / 2, width, height);
				break;
				
			case ADPosition.BOTTOM_LEFT:
				rect = new Rect(0, Screen.height - height, width, height);
				break;
			case ADPosition.BOTTOM_CENTER:
				rect = new Rect((Screen.width - width) / 2, Screen.height - height, width, height);
				break;
			case ADPosition.BOTTOM_RIGHT:
				rect = new Rect(Screen.width - width, Screen.height - height, width, height);
				break;
			}
		}

		public override bool GetIsLoading() {
			return false;
		}

		public void OnGUI()
		{
			if (isAdVisble) {
				GUI.Box(rect, GetType().Name);
			}
		}
	}
}