using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using Xamarin.Essentials;

namespace FaceAPIDemo.ViewModels
{
    public class MainPageViewModel : ViewModelBase, IDisposable
    {
        public ReactiveProperty<string> APIKey { get; } = new ReactiveProperty<string>(Preferences.Get(nameof(APIKey), ""));

        public AsyncReactiveCommand ExecutePageCommand { get; } = new AsyncReactiveCommand();

        private CompositeDisposable Disposable { get; } = new CompositeDisposable();



        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

            APIKey.Subscribe((param) =>
            {
                Preferences.Set(nameof(APIKey), param);
            }).AddTo(this.Disposable);

            ExecutePageCommand.Subscribe(async () =>
            {
                var navigationParameters = new NavigationParameters()
                {
                    //{ "キー", 値 },
                    { "APIKey", APIKey.Value },
                };
                await this.NavigationService.NavigateAsync("ExecutePage", navigationParameters);
            }).AddTo(this.Disposable);
        }

        public void Dispose()
        {
            this.Disposable.Dispose();
        }
    }
}
