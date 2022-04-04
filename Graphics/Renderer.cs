using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;
using GlmNet;
using System.IO;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        uint vertexBufferID;
        uint vertexBufferID2;
        uint vertexBufferID3;
        uint vertexBufferID4;
        uint vertexBufferID5;
        uint vertexBufferID6;
        uint vertexBufferID7;
        int transID;
        int viewID;
        int projID;
        mat4 scaleMat;

        mat4 ProjectionMatrix;
        mat4 ViewMatrix;


        public Camera cam;

        Texture tex1;
        Texture tex2;
        Texture tex3;
        Texture tex4;
        Texture tex5;
        Texture tex6;
        Texture tex7;

        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");

            tex1 = new Texture(projectPath + "\\Textures\\back.png", 1);
            tex2 = new Texture(projectPath + "\\Textures\\Ground.jpg", 2);
            tex3 = new Texture(projectPath + "\\Textures\\top.png", 3);
            tex4 = new Texture(projectPath + "\\Textures\\left.png", 4);
            tex5 = new Texture(projectPath + "\\Textures\\right.png", 5);
            tex6 = new Texture(projectPath + "\\Textures\\bottom.png", 6);
            tex7 = new Texture(projectPath + "\\Textures\\front.png", 7);


            Gl.glClearColor(0, 0, 0.4f, 1);


            float[] verts_back = {
                -1.0f, -1.0f, -1.0f,
                 1,0,0,
                 0,1,

                 -1.0f, 1.0f, -1.0f,
                 1,0,0,
                 0,0,

                 1.0f,  1.0f, -1.0f,
                 1,0,0,
                 1,0,

                 1.0f,  -1.0f, -1.0f,
                 1,0,0,
                 1,1

            };
            float[] verts_top = {
                1.0f, 1.0f, 1.0f,
                 1,0,0,
                 0,1,

                 1.0f, 1.0f, -1.0f,
                 1,0,0,
                 0,0,

                 -1.0f,  1.0f, -1.0f,
                 1,0,0,
                 1,0,

                 -1.0f,  1.0f, 1.0f,
                 1,0,0,
                 1,1

            };
            float[] verts_left = {
                -1.0f, -1.0f, 1.0f,
                 1,0,0,
                 0,1,

                 -1.0f, 1.0f, 1.0f,
                 1,0,0,
                 0,0,

                 -1.0f,  1.0f, -1.0f,
                 1,0,0,
                 1,0,

                 -1.0f,  -1.0f, -1.0f,
                 1,0,0,
                 1,1

            };
            float[] verts_right = {
                1.0f, -1.0f, -1.0f,
                 1,0,0,
                 0,1,

                 1.0f, 1.0f, -1.0f,
                 1,0,0,
                 0,0,

                 1.0f,  1.0f, 1.0f,
                 1,0,0,
                 1,0,

                 1.0f,  -1.0f, 1.0f,
                 1,0,0,
                 1,1

            };

            float[] verts_bottom = {
                1.0f, -1.0f, -1.0f,
                 1,0,0,
                 0,1,

                 1.0f, -1.0f, 1.0f,
                 1,0,0,
                 0,0,

                 -1.0f,  -1.0f, 1.0f,
                 1,0,0,
                 1,0,

                 -1.0f,  -1.0f, -1.0f,
                 1,0,0,
                 1,1

            };

            float[] verts_front = {
                1.0f, -1.0f, -1.0f,
                 1,0,0,
                 1,1,

                 1.0f, 1.0f, -1.0f,
                 1,0,0,
                 1,0,

                 -1.0f,  1.0f, -1.0f,
                 1,0,0,
                 0,0,

                 -1.0f,  -1.0f, -1.0f,
                 1,0,0,
                 0,1

            };

            float[] ground = {
                -5.0f, -1.0f, 5.0f,//1
                 0,0,1,
                 0,0,

                 5.0f, -1.0f, -5.0f,//2
                 0,0,1,
                 1,1,

                -5.0f, -1.0f, -5.0f,
                 0,0,1,
                 1,0,

                 5.0f, -1.0f, 5.0f,
                 0,0,1,
                 0,1,

                -5.0f, -1.0f, 5.0f,//1
                 0,0,1,
                 0,0,

                 5.0f, -1.0f, -5.0f,//2
                 0,0,1,
                 1,1
            };

            vertexBufferID = GPU.GenerateBuffer(verts_back);
            vertexBufferID2 = GPU.GenerateBuffer(ground);
            vertexBufferID3 = GPU.GenerateBuffer(verts_top);
            vertexBufferID4 = GPU.GenerateBuffer(verts_left);
            vertexBufferID5 = GPU.GenerateBuffer(verts_right);
            vertexBufferID6 = GPU.GenerateBuffer(verts_bottom);
            vertexBufferID7 = GPU.GenerateBuffer(verts_front);
            scaleMat = glm.scale(new mat4(1),new vec3(2f, 2f, 2.0f));

            cam = new Camera();

            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();

            transID = Gl.glGetUniformLocation(sh.ID, "model");
            projID = Gl.glGetUniformLocation(sh.ID, "projection");
            viewID = Gl.glGetUniformLocation(sh.ID, "view");

        }

        public void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            sh.UseShader();

            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, scaleMat.to_array());
            Gl.glUniformMatrix4fv(projID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());
            Gl.glUniformMatrix4fv(viewID, 1, Gl.GL_FALSE, ViewMatrix.to_array());

            GPU.BindBuffer(vertexBufferID2);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            tex2.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);

            GPU.BindBuffer(vertexBufferID);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            tex1.Bind();
            Gl.glDrawArrays(Gl.GL_POLYGON, 0, 4);

            GPU.BindBuffer(vertexBufferID3);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            tex3.Bind();
            Gl.glDrawArrays(Gl.GL_POLYGON, 0, 4);

            GPU.BindBuffer(vertexBufferID4);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            tex4.Bind();
            Gl.glDrawArrays(Gl.GL_POLYGON, 0, 4);

            GPU.BindBuffer(vertexBufferID5);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            tex5.Bind();
            Gl.glDrawArrays(Gl.GL_POLYGON, 0, 4);

            GPU.BindBuffer(vertexBufferID6);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            tex6.Bind();
            Gl.glDrawArrays(Gl.GL_POLYGON, 0, 4);

            GPU.BindBuffer(vertexBufferID7);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));

            tex7.Bind();
            Gl.glDrawArrays(Gl.GL_POLYGON, 0, 4);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            Gl.glDisableVertexAttribArray(2);
        }
        public void Update()
        {
            cam.UpdateViewMatrix();
            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();
        }
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
