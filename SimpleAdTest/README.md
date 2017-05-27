SimpleAdTest can have some problems:

* [NoClassDefFoundError on instantiating AdView](https://stackoverflow.com/questions/2247998/noclassdeffounderror-eclipse-and-android)^
  * Properties -> Java Build Path -> "Order and Export"
  * google-play-services must be upper then "src" and "gen" and it must be checked


* [adView.loadAd() is crashing my entire program](https://stackoverflow.com/questions/22850184/adview-loadad-is-crashing-my-entire-program).
  * it must be present res -> values -> vesion.xml and add
`<integer name="google_play_services_version">4242000</integer>`
