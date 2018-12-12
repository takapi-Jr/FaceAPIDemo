using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using FaceAPI_SampleApp.Models;
using System.Windows.Input;
using Xamarin.Forms;
using System.Globalization;

namespace FaceAPI_SampleApp.ViewModels
{
	public class SettingPageViewModel : ViewModelBase
    {
        #region プロパティ

        // 名前
        public string NameEntry { get; } = "FaceAPITest";

        // APIキー
        private string _apiKeyEntry;
        public string APIKeyEntry
        {
            get { return _apiKeyEntry; }
            set { SetProperty(ref _apiKeyEntry, value); }
        }

        #endregion



        #region コマンド

        public ICommand SettingAPIKeyCommand => new Command(() =>
        {
            try
            {
                var tempAccessKey = new KeyData()
                {
                    Name = NameEntry,
                    APIKey = APIKeyEntry,
                };

                // MainPageへ遷移
                var navigationParameters = new NavigationParameters()
                {
                    //{ "キー", 値 },
                    { MainPageViewModel.InputKey_AccessKey, tempAccessKey },
                };

                // ルートに戻る
                this.NavigationService.GoBackToRootAsync(navigationParameters);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("SettingAPIKeyCommand", $"例外が発生しました。{ex}", "OK");
            }
        });

        #endregion



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SettingPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "FaceAPI_Sample Setting Page";
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
            
        }
    }

    /// <summary>
    /// MultiTrigger使用時の文字列の長さ比較用クラス
    /// </summary>
    public class MultiTriggerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if ((int)value > 0)         // length > 0 ?
                return true;            // some data has been entered
            else
                return false;           // input is empty
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
