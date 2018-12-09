using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using FaceAPI_SampleApp.Droid;
using FaceAPI_SampleApp.Models;
using Plugin.CurrentActivity;
using Prism;
using Prism.Ioc;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FaceAPI_SampleApp.Droid
{
    [Activity(Label = "FaceAPI_SampleApp", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));

            CrossCurrentActivity.Current.Init(this, bundle);

            Instance = this;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            //containerRegistry.Register<IPicturePicker, PicturePickerImplementation>();
        }
    }
}

