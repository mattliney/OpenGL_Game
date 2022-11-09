using OpenTK;
using OpenGL_Game.Managers;
using System;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.OBJLoader;

namespace OpenGL_Game.Components
{
    abstract class ComponentShader : IComponent
    {
        public int mPGMID;

        public ComponentShader(string pVertexShaderName, string pFragmentShaderName)
        {
            mPGMID = GL.CreateProgram();
            GL.AttachShader(mPGMID, ResourceManager.LoadShader(pVertexShaderName, ShaderType.VertexShader));
            GL.AttachShader(mPGMID, ResourceManager.LoadShader(pFragmentShaderName, ShaderType.FragmentShader));
            GL.LinkProgram(mPGMID);
            Console.WriteLine(GL.GetProgramInfoLog(mPGMID));
        }

        public abstract void ApplyShader(Matrix4 pModel, Geometry pGeometry);

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_SHADER; }
        }
    }
}
