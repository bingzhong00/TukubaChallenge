// Copyright (c) 2011 TAJIMA Yoshiyuki 
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
// 
//   1. Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//   2. Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE FREEBSD PROJECT ``AS IS'' AND ANY EXPRESS OR 
// IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
// SHALL THE FREEBSD PROJECT OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
// OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE 
// OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
// OF THE POSSIBILITY OF SUCH DAMAGE.
// 
// The views and conclusions contained in the software and documentation are those 
// of the authors and should not be interpreted as representing official policies, 
// either expressed or implied, of the FreeBSD Project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Imaging;

namespace LocationPresumption
{

    /// <summary>
    /// 内部ＭＡＰ生成用　ＢＭＰクラス 24bitカラーのみ対応
    /// </summary>
    public class BitmapAccess
    {
        private Bitmap Bmp;
        private BitmapData img;

        public BitmapAccess(Bitmap bmp) {
            Bmp = bmp;
        }

        public void BeginAccess() {
            img = Bmp.LockBits(
                new Rectangle(0, 0, Bmp.Width, Bmp.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);
        }

        public void EndAccess() {
            Bmp.UnlockBits(img);
        }

        public Color this[int x, int y] {
            get {
                unsafe {
                    byte* adr = (byte*)img.Scan0;
                    //int pos = x * 3 + img.Stride * (Bmp.Height - 1 - y);
                    int pos = x * 3 + img.Stride * (y);
                    byte b = adr[pos + 0];
                    byte g = adr[pos + 1];
                    byte r = adr[pos + 2];
                    return Color.FromArgb(r, g, b);
                }
            }
            set {
                unsafe {
                    byte* adr = (byte*)img.Scan0;
                    //int pos = x * 3 + img.Stride * (Bmp.Height - 1 - y);
                    int pos = x * 3 + img.Stride * (y);
                    adr[pos + 0] = value.B;
                    adr[pos + 1] = value.G;
                    adr[pos + 2] = value.R;
                }
            }
        }

    }

}
