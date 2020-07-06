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
    public static class Media
    {
        public static readonly List<Permissions.BasePermission> GetImagePermissions = new List<Permissions.BasePermission>
        {
            new Permissions.StorageWrite(),
            new Permissions.StorageRead(),
        };

        public static readonly List<Permissions.BasePermission> TakePhotoPermissions = new List<Permissions.BasePermission>
        {
            new Permissions.Camera(),
            new Permissions.StorageWrite(),
            new Permissions.StorageRead(),
        };

        /// <summary>
        /// 画像を選択してImageSourceを取得
        /// </summary>
        /// <returns></returns>
        public static async Task<ImageSource> GetImageToImageSource()
        {
            // パーミッションチェック
            var grantedFlag = await Common.CheckPermissions(GetImagePermissions);
            if (!grantedFlag)
            {
                return null;
            }

            // Pluginの初期化
            await CrossMedia.Current.Initialize();

            // 画像選択可能か判定
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                return null;
            }

            // 画像選択画面を表示
            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                SaveMetaData = false,
                PhotoSize = PhotoSize.Full,
            });

            // 画像を選択しなかった場合は終了
            if (file == null)
            {
                System.Diagnostics.Debug.WriteLine("画像選択なし");
                return null;
            }

            // 一時的に保存した画像ファイルの中身をメモリに読み込み、ファイルは削除してしまう
            var bytes = Media.GetImageBytes(file);
            File.Delete(file.Path);
            file.Dispose();

            // 写真を画面上の image 要素に表示する
            var imageSource = ImageSource.FromStream(() =>
            {
                // メモリから画像表示
                return new MemoryStream(bytes.ToArray());
            });

            return imageSource;
        }

        /// <summary>
        /// カメラで撮影してImageSourceを取得
        /// </summary>
        /// <returns></returns>
        public static async Task<ImageSource> TakePhotoToImageSource()
        {
            // パーミッションチェック
            var grantedFlag = await Common.CheckPermissions(TakePhotoPermissions);
            if (!grantedFlag)
            {
                return null;
            }

            // Pluginの初期化
            await CrossMedia.Current.Initialize();

            // 撮影可能か判定
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                return null;
            }

            // カメラが起動し写真を撮影する。撮影した写真はストレージに保存され、ファイルの情報が return される
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                // ストレージに保存するファイル情報
                // すでに同名ファイルがある場合は、temp_1.jpg などの様に連番がつけられ名前の衝突が回避される
                Directory = "TempPhotos",
                Name = "temp.jpg",
                SaveMetaData = false,
                PhotoSize = PhotoSize.Full,
            });

            // カメラ撮影しなかった場合は終了
            if (file == null)
            {
                System.Diagnostics.Debug.WriteLine("カメラ撮影なし");
                return null;
            }

            // 一時的に保存した画像ファイルの中身をメモリに読み込み、ファイルは削除してしまう
            var bytes = Media.GetImageBytes(file);
            File.Delete(file.Path);
            file.Dispose();

            // 写真を画面上の image 要素に表示する
            var imageSource = ImageSource.FromStream(() =>
            {
                // メモリから画像表示
                return new MemoryStream(bytes.ToArray());
            });

            return imageSource;
        }

        /// <summary>
        /// MediaFileからbyteのqueueに変換
        /// </summary>
        /// <param name="mediaFile">画像情報</param>
        /// <returns>画像のバイト配列(キュー)</returns>
        public static Queue<byte> GetImageBytes(MediaFile mediaFile)
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
    }
}
