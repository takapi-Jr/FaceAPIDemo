﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FaceAPI_SampleApp.Models;
using System.Windows.Input;
using Xamarin.Forms;
using System.IO;

namespace FaceAPI_SampleApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region 定数

        // パラメータ取得用キー
        public static readonly string InputKey_AccessKey = "AccessKey";

        #endregion



        #region プロパティ

        // 自作プロパティの命名規則メモ
        // privateのプロパティ名   →   _hogeHoge
        // publicのプロパティ名    →   HogeHoge

        // FaceAPIキー情報
        private KeyData _accessKey;
        public KeyData AccessKey
        {
            get { return _accessKey; }
            set { SetProperty(ref _accessKey, value); }
        }

        //// 顔認識情報
        //private KeyData _faceResult;
        //public KeyData FaceResult
        //{
        //    get { return _faceResult; }
        //    set { SetProperty(ref _faceResult, value); }
        //}

        #endregion



        #region コマンド

        /// <summary>
        /// 画像選択画面を表示するコマンド
        /// </summary>
        public ICommand PickPictureCommand => new Command(async () =>
        {
            try
            {
                //pickPictureButton.IsEnabled = false;
                Stream stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();

                if (stream != null)
                {
                    Image image = new Image
                    {
                        Source = ImageSource.FromStream(() => stream),
                        BackgroundColor = Color.Gray
                    };

                    TapGestureRecognizer recognizer = new TapGestureRecognizer();
                    recognizer.Tapped += (sender2, args) =>
                    {
                        //(MainPage as ContentPage).Content = stack;
                        //pickPictureButton.IsEnabled = true;
                    };
                    image.GestureRecognizers.Add(recognizer);

                    //(MainPage as ContentPage).Content = image;
                }
                else
                {
                    //pickPictureButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                //DisplayAlert("タイトル", "メッセージ", "OK");
            }
        });

        /// <summary>
        /// ResultPageへ画面遷移するコマンド
        /// </summary>
        private DelegateCommand _gotoResultPageCommand;
        public DelegateCommand GotoResultPageCommand
        {
            get
            {
                if (this._gotoResultPageCommand != null)
                {
                    return this._gotoResultPageCommand;
                }

                this._gotoResultPageCommand = new DelegateCommand(() =>
                {
                    this.NavigationService.NavigateAsync("ResultPage");
                });
                return this._gotoResultPageCommand;
            }
        }

        /// <summary>
        /// SettingPageへ画面遷移するコマンド
        /// </summary>
        private DelegateCommand _gotoSettingPageCommand;
        public DelegateCommand GotoSettingPageCommand
        {
            get
            {
                if (this._gotoSettingPageCommand != null)
                {
                    return this._gotoSettingPageCommand;
                }

                this._gotoSettingPageCommand = new DelegateCommand(() =>
                {
                    this.NavigationService.NavigateAsync("SettingPage");
                });
                return this._gotoSettingPageCommand;
            }
        }

        #endregion



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainPageViewModel(INavigationService navigationService) 
            : base (navigationService)
        {
            Title = "FaceAPI_Sample Main Page";

            AccessKey = new KeyData();
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
