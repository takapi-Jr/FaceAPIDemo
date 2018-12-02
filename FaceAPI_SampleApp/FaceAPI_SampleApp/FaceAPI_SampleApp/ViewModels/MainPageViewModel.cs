using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FaceAPI_SampleApp.Models;

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

        #endregion



        #region コマンド

        /// <summary>
        /// ExecutePageへ画面遷移するコマンド
        /// </summary>
        private DelegateCommand _gotoExecutePageCommand;
        public DelegateCommand GotoExecutePageCommand
        {
            get
            {
                if (this._gotoExecutePageCommand != null)
                {
                    return this._gotoExecutePageCommand;
                }

                this._gotoExecutePageCommand = new DelegateCommand(() =>
                {
                    this.NavigationService.NavigateAsync("ExecutePage");
                });
                return this._gotoExecutePageCommand;
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
