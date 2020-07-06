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

namespace FaceAPIDemo.ViewModels
{
    public class MainPageViewModel : ViewModelBase, IDisposable
    {
        public ReactiveProperty<string> APIKey { get; } = new ReactiveProperty<string>();

        public AsyncReactiveCommand ExecutePageCommand { get; } = new AsyncReactiveCommand();

        private CompositeDisposable Disposable { get; } = new CompositeDisposable();



        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

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
