using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using UntitledEngine.Common;

namespace UntitledEngine
{
    public class Mesh
    {

        private int VAO, VBO, EBO;
        private int vertexCount;

        private Shader shader;

        public Mesh(float[] vertices, int[] indices, Shader shader)
        {
            this.shader = shader;
            vertexCount = indices.Length;

            // Generate and bind VAO
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            // Generate and bind VBO
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Generate and bind EBO
            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(float), indices, BufferUsageHint.StaticDraw);

            // Set up vertex attribute pointers (currently position only)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Unbind VAO to prevent accidental modification
            GL.BindVertexArray(0);
        }

        // Method to create a simple quad
        public static Mesh CreateQuadMesh(Shader shader)
        {
            float[] vertices = new float[]
{
                -0.5f,  0.5f, 0f,
                0.5f,  0.5f, 0f,
                0.5f, -0.5f, 0f,
                -0.5f, -0.5f, 0f
};

            int[] indices = new int[]
            {
                0, 1, 2,
                2, 3, 0
            };

            return new Mesh(vertices, indices, shader);
        }

        public void Draw()
        {
            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, vertexCount, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

        public void Cleanup()
        {
            GL.BindVertexArray(0);
            GL.DeleteBuffer(VBO);
            GL.DeleteBuffer(EBO);
            GL.DeleteVertexArray(VAO);
        }

        public Shader GetShader() => shader;
    }
}
