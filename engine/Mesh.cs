using Silk.NET.OpenGL;
using Silk.NET.Maths;
using System;

namespace UntitledEngine
{
    public class Mesh
    {

        private uint VAO, VBO, EBO;
        private uint vertexCount;

        private Shader shader;

        private GL gl;

        public Mesh(float[] vertices, uint[] indices, Shader shader)
        {
            gl = GLContext.Instance ?? throw new InvalidOperationException("GL context is not initialized.");
            this.shader = shader;
            vertexCount = (uint)indices.Length;

            // Generate and bind VAO
            VAO = gl.GenVertexArray();
            gl.BindVertexArray(VAO);

            // Generate and bind VBO
            VBO = gl.GenBuffer();
            gl.BindBuffer(BufferTargetARB.ArrayBuffer, VBO);
            gl.BufferData(BufferTargetARB.ArrayBuffer, new ReadOnlySpan<float>(vertices), BufferUsageARB.StaticDraw);

            // Generate and bind EBO
            EBO = gl.GenBuffer();
            gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, EBO);
            gl.BufferData(BufferTargetARB.ElementArrayBuffer, new ReadOnlySpan<uint>(indices), BufferUsageARB.StaticDraw);

            // Set up vertex attribute pointers (currently position only)
            gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            gl.EnableVertexAttribArray(0);

            // Unbind VAO to prevent accidental modification
            gl.BindVertexArray(0);
        }

        public void Draw(Matrix4X4<float> model)
        {
            shader.Use(); // Bind shader
            int modelLocation = gl.GetUniformLocation(shader.ID, "model");

            float[] modelArray = new float[16]; // Matrix4x4 is 4x4, so 16 floats in total

            gl.UniformMatrix4(modelLocation, false, modelArray);

            gl.BindVertexArray(VAO);
            gl.DrawElements(PrimitiveType.Triangles, vertexCount, DrawElementsType.UnsignedInt, 0);
            gl.BindVertexArray(0);

        }

        public void Cleanup()
        {
            gl.BindVertexArray(0);
            gl.DeleteBuffer(VBO);
            gl.DeleteBuffer(EBO);
            gl.DeleteVertexArray(VAO);
        }

        public Shader GetShader() => shader;
    }
}
