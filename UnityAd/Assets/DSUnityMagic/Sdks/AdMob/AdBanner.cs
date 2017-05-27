using System;
using UnityEngine;

namespace DSUnityMagic.Sdks.AdMob
{
	[Serializable]
	public abstract class AdBanner
	{
		[SerializeField]
		protected ADSize adBannerSize = ADSize.BANNER;
		
		[SerializeField]
		protected ADPosition adPosition = ADPosition.CENTER;

		[SerializeField]
		protected bool isAdVisble = false;

		public abstract void LoadAd(ADSize size, ADPosition pos);
		public abstract void LoadAd(ADSize size);
		public abstract void LoadAd();
		
		public abstract void ChangePosition(ADPosition pos);
		public abstract bool GetIsLoading();

		public virtual void SetVisible(bool isVisible) {
			this.isAdVisble = isVisible;
		}
		
		public ADSize PreviousSize() {
			return adBannerSize;
		}
		
		public ADPosition PreviousPosition() {
			return adPosition;
		}

		public bool GetVisible() {
			return isAdVisble;
		}

		public virtual void Vibration(int delay) { }
	}
}