using Acr.UserDialogs;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FaceAPIDemo.Models
{
    public static class Common
    {
        /// <summary>
        /// パーミッションチェック処理
        /// </summary>
        /// <returns>権限付与フラグ(true:付与された, false:付与されなかった)</returns>
        public static async Task<bool> CheckPermissions(List<Permissions.BasePermission> permissions)
        {
            foreach (var permission in permissions)
            {
                var status = await Common.CheckAndRequestPermissionAsync(permission);
                if (status != PermissionStatus.Granted)
                {
                    // Notify user permission was denied
                    return false;
                }
            }

            return true;
        }

        public static async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
            where T : Permissions.BasePermission
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            return status;
        }

        /// <summary>
        /// FaceAPI実行
        /// </summary>
        public static async Task<DetectedFace> ExecuteFaceAPIAsync(string apiKey, string faceUriEndPoint, FaceAttributeType[] faceAttributes, ImageSource faceImageSource)
        {
            var ret = new DetectedFace();
            try
            {
                // クライアント作成
                var faceClient = new FaceClient(new ApiKeyServiceClientCredentials(apiKey), new System.Net.Http.DelegatingHandler[] { })
                {
                    Endpoint = faceUriEndPoint,
                };

                // ストリーム(タップした画像)から検出処理
                System.Diagnostics.Debug.WriteLine("FaceAPI実行");
                var faceList = await faceClient.Face.DetectWithStreamAsync(Common.GetStreamFromImageSource(faceImageSource), true, false, faceAttributes);

                if (faceList.Count == 0 || faceList == null)
                {
                    await UserDialogs.Instance.AlertAsync($"顔検出できませんでした。", "ExecuteFaceAPIAsync", "OK");
                    return null;
                }
                ret = faceList[0];
            }
            catch (APIErrorException ex)
            {
                await UserDialogs.Instance.AlertAsync($"APIErrorExceptionです。\n{ex}", "ExecuteFaceAPIAsync", "OK");
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync($"例外が発生しました。\n{ex}", "ExecuteFaceAPIAsync", "OK");
            }

            return ret;
        }

        /// <summary>
        /// ImageSourceからStreamを取得
        /// </summary>
        /// <param name="source">表示用画像ソース</param>
        /// <returns>ストリーム</returns>
        public static Stream GetStreamFromImageSource(ImageSource source)
        {
            StreamImageSource streamImageSource = (StreamImageSource)source;
            System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
            Task<Stream> task = streamImageSource.Stream(cancellationToken);
            Stream stream = task.Result;
            return stream;
        }
    }
}
