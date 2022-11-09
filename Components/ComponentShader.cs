using OpenTK;
using OpenGL_Game.Managers;
using System;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.OBJLoader;

namespace OpenGL_Game.Components
{
    abstract class ComponentShader : IComponent
    {
        public int mShaderID;

        public ComponentShader(string pVertexShaderName, string pFragmentShaderName)
        {
            mShaderID = GL.CreateProgram();
            GL.AttachShader(mShaderID, ResourceManager.LoadShader(pVertexShaderName, ShaderType.VertexShader));
            GL.AttachShader(mShaderID, ResourceManager.LoadShader(pFragmentShaderName, ShaderType.FragmentShader));
            GL.LinkProgram(mShaderID);
            Console.WriteLine(GL.GetProgramInfoLog(mShaderID));
        }

        public abstract void ApplyShader(Matrix4 pModel, Geometry pGeometry);

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_SHADER; }
        }
    }
}
