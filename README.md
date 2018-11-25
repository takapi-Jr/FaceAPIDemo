# FaceAPI_SampleApp
Face APIサンプルアプリ(Xamarin.Formsアプリ)  
MicrosoftのFace APIを使用して、表情などの情報を取得するサンプルアプリ。  

# 使用したNugetパッケージとバージョン
|Nugetパッケージ名|バージョン|
|:-|:-|
|NETStandard.Library|v2.0.3|
|Xamarin.Forms|v3.1.0.697729(2018/08時点の最新を使用)|
|Prism.DryIoc.Forms|v7.0.0.396(2018/08時点の最新を使用)|
|Microsoft.Azure.CognitiveServices.Vision.Face|v2.2.0-preview <br> (Nugetから取得する際は「プレリリースを含める」のチェックボックスをONにして検索)|
|Xamarin.Forms.BehaviorsPack|v2.0.0|
|Xamarin.Android.Support.Design <br> Xamarin.Android.Support.v4 <br> Xamarin.Android.Support.v7.AppCompat <br> Xamarin.Android.Support.v7.CardView <br> Xamarin.Android.Support.v7.MediaRouter|v25.4.0.2(最新を使うと怒られた) <br> ※補足1：monoandroidのバージョンによって使用できるバージョンが異なる <br> ※補足2：ターゲットAndroidバージョンが7.0(APIレベル24)未満の場合は、v23.3.0を指定する必要あり|

参考ページ：  
Xamarin.Forms の Android プロジェクトで最新のサポートライブラリを使用するには（Xamarin.Forms をアップデートする場合も同様です）  
http://ytabuchi.hatenablog.com/entry/2017/04/28/200000  
Xamarin.Android でサポートライブラリ 26.0.x を使う際の注意点  
https://ytabuchi.hatenablog.com/entry/2017/11/10/233014  

# Androidビルド時の設定
|項目名|バージョン|
|:-|:-|
|Androidバージョンを使用したコンパイル(ターゲットフレームワーク))|Android8.1(Oreo)<br>Xamarin.Formsを最新にしているため、ターゲットフレームワークも現時点で最新のものを使用|
|最小Androidバージョン|Android5.0(APIレベル21 - Lollipop)|
|ターゲットAndroidバージョン|SDKバージョンを使用したコンパイルの使用|

# その他設定(未設定)
- [Androidプロジェクト]の右クリックメニューから[プロパティ]を選択
    - [Androidオプション]
        - [パッケージングプロパティ]
            - [Fast Deploymentの使用(デバッグモードのみ)]<br>下記エラーが出たため、チェックをOFFへ変更<br>Error accepting stdout and stderr (127.0.0.1:29255): Address already in use

参考ページ：  
Xamarin Android (Visual Studio 2015) Could not connect to the debugger  
https://stackoverflow.com/questions/32589438/xamarin-android-visual-studio-2015-could-not-connect-to-the-debugger  
