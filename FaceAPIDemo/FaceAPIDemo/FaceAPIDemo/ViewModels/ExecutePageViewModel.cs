using FaceAPIDemo.Models;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using Xamarin.Forms;

namespace FaceAPIDemo.ViewModels
{
    public class ExecutePageViewModel : ViewModelBase, IDisposable
    {
        /// <summary>
        /// 東日本エンドポイント
        /// </summary>
        public static readonly string FaceUriEndPoint = "https://japaneast.api.cognitive.microsoft.com";

        /// <summary>
        /// 取得したい(顔の)情報を配列で設定
        /// </summary>
        public static readonly FaceAttributeType[] FaceAttributes = { FaceAttributeType.Age, FaceAttributeType.Gender, FaceAttributeType.Emotion, FaceAttributeType.Smile };


        public ReactiveProperty<string> APIKey { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<ImageSource> FaceImageSource { get; } = new ReactiveProperty<ImageSource>();
        public ReactiveProperty<DetectedFace> DetectedData { get; } = new ReactiveProperty<DetectedFace>();

        public AsyncReactiveCommand GetImageCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand TakePhotoCommand { get; } = new AsyncReactiveCommand();

        private CompositeDisposable Disposable { get; } = new CompositeDisposable();



        public ExecutePageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Execute Page";

            GetImageCommand.Subscribe(async () =>
            {
                DetectedData.Value = null;

                FaceImageSource.Value = await Media.GetImageToImageSource();
                if (FaceImageSource.Value != null)
                {
                    DetectedData.Value = await Common.ExecuteFaceAPIAsync(APIKey.Value, FaceUriEndPoint, FaceAttributes, FaceImageSource.Value);
                }
            }).AddTo(this.Disposable);

            TakePhotoCommand.Subscribe(async () =>
            {
                DetectedData.Value = null;

                FaceImageSource.Value = await Media.TakePhotoToImageSource();
                if (FaceImageSource.Value != null)
                {
                    DetectedData.Value = await Common.ExecuteFaceAPIAsync(APIKey.Value, FaceUriEndPoint, FaceAttributes, FaceImageSource.Value);
                }
            }).AddTo(this.Disposable);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            APIKey.Value = parameters.GetValue<string>("APIKey");
        }

        public void Dispose()
        {
            this.Disposable.Dispose();
        }
    }
}
