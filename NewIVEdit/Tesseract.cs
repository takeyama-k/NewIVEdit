using System;
using System.Windows;
using Tesseract;
using System.IO;
using System.Drawing;
namespace NewIVEdit
{
    public static class Tess
    {
        public static Tesseract.TesseractEngine TessEngine = new TesseractEngine(Path.Combine(System.Windows.Forms.Application.StartupPath, @"tessdata"), "eng");
        public static string ProcessBitmap(Bitmap target)
        {
            using (var pix = PixConverter.ToPix(target))
            {
                using (var page = TessEngine.Process(pix))
                {
                    return page.GetText();
                }
            }
        }
    }

    
}
