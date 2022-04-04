﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;
using System.IO;
using System.Runtime.InteropServices;

namespace Graphics
{
    class Texture
    {
        uint mtexture;
        int width, height;
        int TexUnit;
        public Texture(string path, int texUnit)
        {
            Bitmap bitmap = (Bitmap)Bitmap.FromFile(path);
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            width = bitmap.Width;
            height = bitmap.Height;

            TexUnit = texUnit;
            Gl.glActiveTexture(texUnit);
            uint[] tex = { 0 };
            //1 - generate ID
            Gl.glGenTextures(1, tex);
            mtexture = tex[0];
            //2 - Bind
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, mtexture);

            //4 - Specifying Parameters
            //wrapping S & T
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_MIRRORED_REPEAT);

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);

            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);

            //3 - Allocation (Buffering)
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, width, height, 0, Gl.GL_BGRA, 
                Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);



            if (bitmap != null)
            {
                bitmap.UnlockBits(bitmapData);
                bitmap.Dispose();
            }
        }

        public void CleanUp()
        {
            Gl.glDeleteTextures(1, (IntPtr)mtexture);
        }
        public void Bind()
        {
            Gl.glActiveTexture(TexUnit);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, mtexture);
        }
    }
}
