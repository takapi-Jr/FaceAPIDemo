using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FaceAPI_SampleApp.Models
{
    /// <summary>
    /// 画像選択処理インターフェース
    /// </summary>
    public interface IPicturePicker
    {
        Task<Stream> GetImageStreamAsync();
    }
}
