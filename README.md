# UnityAdMob 
Library for using [Google AdMob](https://www.google.com/admob/) with [Unity 3D](https://unity3d.com). This is very old project, so it compatible with Unity 4.3

Contains:
* __JavaAdLib__ - java code whitch cooperates directly with google-play-services.jar
* __UnityAd__ - C# wrapper for java class of JavaAdLib. It contains [library classes](https://github.com/Doctor-Script/UnityAdMob/tree/master/UnityAd/Assets/DSUnityMagic/Sdks/AdMob) and [DSADPluginDemo.cs](https://github.com/Doctor-Script/UnityAdMob/blob/master/UnityAd/Assets/DSADPluginDemo.cs) which demonstrate how to use it.
* __SimpleAdTest__ - very simplified ad banner test.


Open __JavaAdLib__ project with Eclipse (File -> Import... -> Android -> Existing Android Code Into Workspace) and export .jar file (Export.. -> JAR file). You need put this .jar to plugins\android in Unity editor root. Then you can open Unity project and build it for Android

## Useful links:
* [User guide](https://firebase.google.com/docs/admob/?hl=ru-ru)
* [Safe testing library integration](https://developers.google.com/admob/android/test-ads)
