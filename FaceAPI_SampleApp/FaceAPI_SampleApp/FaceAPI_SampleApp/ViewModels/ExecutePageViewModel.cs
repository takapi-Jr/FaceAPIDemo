using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using FaceAPI_SampleApp.Models;
using System.IO;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace FaceAPI_SampleApp.ViewModels
{
	public class ExecutePageViewModel : ViewModelBase
    {
        #region 定数
        
        // パラメータ取得用キー
        public static readonly string InputKey_AccessKey = "AccessKey";
        // 東日本エンドポイント
        public static readonly string FaceUriEndPoint = "https://japaneast.api.cognitive.microsoft.com/face/v1.0";
        // テスト文字列
        public static readonly string TestString = "Test";
        
        // 取得したい(顔の)情報を配列で設定。
        private static readonly FaceAttributeType[] faceAttributes =
            { FaceAttributeType.Age, FaceAttributeType.Gender, FaceAttributeType.Emotion };

        #endregion



        #region プロパティ

        // FaceAPIキー情報
        private KeyData _accessKey;
        public KeyData AccessKey
        {
            get { return _accessKey; }
            set { SetProperty(ref _accessKey, value); }
        }

        // 画像情報
        private ImageSource _faceImageSource;
        public ImageSource FaceImageSource
        {
            get { return _faceImageSource; }
            set { SetProperty(ref _faceImageSource, value); }
        }

        // 画像情報(ストリーム)
        private Stream _faceImageStream;
        public Stream FaceImageStream
        {
            get { return _faceImageStream; }
            set { SetProperty(ref _faceImageStream, value); }
        }

        // 検出情報
        private DetectedFace _detectedData;
        public DetectedFace DetectedData
        {
            get { return _detectedData; }
            set { SetProperty(ref _detectedData, value); }
        }


        #endregion



        #region コマンド

        /// <summary>
        /// 画像選択画面を表示するコマンド
        /// </summary>
        public ICommand PickPictureCommand => new Command(async () =>
        {
            try
            {
                // 画像選択画面を表示
                Stream FaceImageStream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();
                
                // 選択した画像情報を取得
                if (FaceImageStream != null)
                {
                    FaceImageSource = ImageSource.FromStream(() => FaceImageStream);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("PickPictureCommand", "画像選択に失敗しました。", "OK");
            }
        });

        /// <summary>
        /// FaceAPIを実行するコマンド
        /// </summary>
        public ICommand FaceAPIExecuteCommand => new Command(async () =>
        {
            try
            {
                var faceClient = new FaceClient(new ApiKeyServiceClientCredentials(AccessKey.APIKey), new System.Net.Http.DelegatingHandler[] { });
                faceClient.Endpoint = FaceUriEndPoint;

                // ストリーム(タップした画像)から検出処理
                var faceList = await faceClient.Face.DetectWithStreamAsync(FaceImageStream, false, false, faceAttributes);
                DetectedData = faceList[0];
                
                // URLから検出処理
                //var faceList = await faceClient.Face.DetectWithUrlAsync(speaker.Avatar, true, false, faceAttributes);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("FaceAPIExecuteCommand", "FaceAPI実行に失敗しました。", "OK");
            }
        });

        #endregion



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExecutePageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "FaceAPI_Sample Execute Page";
        }

        /// <summary>
        /// OnNavigatingTo後呼び出し(このページ"から"画面遷移時に実行)
        /// </summary>
        public override void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        /// <summary>
        /// 画面表示後呼び出し(このページ"に"画面遷移後に実行)
        /// </summary>
        public override void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        /// <summary>
        /// 画面表示前呼び出し(このページ"に"画面遷移時に実行)
        /// </summary>
        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            // NavigationParametersに同じキーのパラメーターを持っているかどうかの確認
            if (parameters.ContainsKey(InputKey_AccessKey))
            {
                // プロパティに格納
                var tempAccessKey = (KeyData)parameters[InputKey_AccessKey];
                AccessKey = tempAccessKey;
            }
        }
    }
}
