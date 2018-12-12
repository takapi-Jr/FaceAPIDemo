using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FaceAPI_SampleApp.Models
{
    public static class Common
    {
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
