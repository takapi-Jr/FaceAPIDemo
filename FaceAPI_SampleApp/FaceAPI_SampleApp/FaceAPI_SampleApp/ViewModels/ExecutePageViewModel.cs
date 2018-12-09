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
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace FaceAPI_SampleApp.ViewModels
{
	public class ExecutePageViewModel : ViewModelBase
    {
        #region 定数
        
        // パラメータ取得用キー
        public static readonly string InputKey_AccessKey = "AccessKey";
        // 東日本エンドポイント
        public static readonly string FaceUriEndPoint = "https://japaneast.api.cognitive.microsoft.com";
        
        // 取得したい(顔の)情報を配列で設定。
        private static readonly FaceAttributeType[] faceAttributes =
            { FaceAttributeType.Age, FaceAttributeType.Gender, FaceAttributeType.Emotion, FaceAttributeType.Smile };

        #endregion



        #region プロパティ

        // FaceAPIキー情報
        private KeyData _accessKey;
        public KeyData AccessKey
        {
            get { return _accessKey; }
            set { SetProperty(ref _accessKey, value); }
        }

        // 画像情報(画像表示用)
        private ImageSource _faceImageSource;
        public ImageSource FaceImageSource
        {
            get { return _faceImageSource; }
            set { SetProperty(ref _faceImageSource, value); }
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
        /// 画像を選択してFaceAPIを実行するコマンド
        /// </summary>
        public ICommand PickPictureCommand => new Command(async () =>
        {
            try
            {
                // Pluginの初期化
                await CrossMedia.Current.Initialize();

                // 画像選択可能か判定
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    return;
                }

                // 画像選択画面を表示
                var file = await CrossMedia.Current.PickPhotoAsync();

                // pngファイルの画像を表示できていない
                //switch (Path.GetExtension(file.Path))
                //{
                //    case ".png":
                //    case ".PNG":
                //        break;
                //    default:
                //        break;
                //}

                // 画像を選択しなかった場合は終了
                if (file == null)
                {
                    System.Diagnostics.Debug.WriteLine("画像選択なし");
                    return;
                }

                // 選択した画像ファイルの中身をメモリに読み込む
                var bytes = GetImageBytes(file);
                file.Dispose();

                // 表示用画像設定
                FaceImageSource = ImageSource.FromStream(() =>
                {
                    // メモリから画像表示
                    return new MemoryStream(bytes.ToArray());
                });

                // FaceAPI実行
                await ExecuteFaceAPIAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("PickPictureCommand", $"例外が発生しました。\n{ex}", "OK");
            }
        });

        /// <summary>
        /// カメラで撮影してFaceAPIを実行するコマンド
        /// </summary>
        public ICommand TakePictureCommand => new Command(async () =>
        {
            try
            {

                // Pluginの初期化
                await CrossMedia.Current.Initialize();

                // 撮影可能か判定
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    return;
                }

                // カメラが起動し写真を撮影する。撮影した写真はストレージに保存され、ファイルの情報が return される
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    // ストレージに保存するファイル情報
                    // すでに同名ファイルがある場合は、temp_1.jpg などの様に連番がつけられ名前の衝突が回避される
                    Directory = "TempPhotos",
                    Name = "temp.jpg"
                });

                // カメラ撮影しなかった場合は終了
                if (file == null)
                {
                    System.Diagnostics.Debug.WriteLine("カメラ撮影なし");
                    return;
                }

                // 一時的に保存した画像ファイルの中身をメモリに読み込み、ファイルは削除してしまう
                var bytes = GetImageBytes(file);
                File.Delete(file.Path);
                file.Dispose();

                // 写真を画面上の image 要素に表示する
                FaceImageSource = ImageSource.FromStream(() =>
                {
                    // メモリから画像表示
                    return new MemoryStream(bytes.ToArray());
                });

                // FaceAPI実行
                await ExecuteFaceAPIAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("TakePictureCommand", $"例外が発生しました。\n{ex}", "OK");
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
        /// FaceAPI実行
        /// </summary>
        public async Task ExecuteFaceAPIAsync()
        {
            try
            {
                // クライアント作成
                var faceClient = new FaceClient(new ApiKeyServiceClientCredentials(AccessKey.APIKey), new System.Net.Http.DelegatingHandler[] { })
                {
                    Endpoint = FaceUriEndPoint,
                };

                // ストリーム(タップした画像)から検出処理
                System.Diagnostics.Debug.WriteLine("FaceAPI実行");
                var faceList = await faceClient.Face.DetectWithStreamAsync(GetStreamFromImageSource(FaceImageSource), true, false, faceAttributes);

                if (faceList.Count == 0 || faceList == null)
                {
                    return;
                }
                DetectedData = faceList[0];
            }
            catch (APIErrorException ex)
            {
                await Application.Current.MainPage.DisplayAlert("ExecuteFaceAPIAsync", $"APIErrorExceptionです。\n{ex}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("ExecuteFaceAPIAsync", $"例外が発生しました。\n{ex}", "OK");
            }

            return;
        }

        /// <summary>
        /// MediaFileからbyteのqueueに変換
        /// </summary>
        /// <param name="mediaFile">画像情報</param>
        /// <returns>画像のバイト配列(キュー)</returns>
        public Queue<byte> GetImageBytes(MediaFile mediaFile)
        {
            var bytes = new Queue<byte>();
            using (var stream = mediaFile.GetStream())
            {
                var length = stream.Length;
                int b;
                while ((b = stream.ReadByte()) != -1)
                {
                    bytes.Enqueue((byte)b);
                }
            }
            return bytes;
        }

        /// <summary>
        /// ImageSourceからStreamを取得
        /// </summary>
        /// <param name="source">表示用画像ソース</param>
        /// <returns>ストリーム</returns>
        public Stream GetStreamFromImageSource(ImageSource source)
        {
            StreamImageSource streamImageSource = (StreamImageSource)source;
            System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
            Task<Stream> task = streamImageSource.Stream(cancellationToken);
            Stream stream = task.Result;
            return stream;
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
