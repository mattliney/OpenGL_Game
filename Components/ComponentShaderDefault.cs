using OpenTK;
using OpenGL_Game.Managers;
using System;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.OBJLoader;
using OpenGL_Game.Scenes;

namespace OpenGL_Game.Components
{
    class ComponentShaderDefault : ComponentShader
    {
        public int mUniform_stex;
        public int mUniform_mmodelviewproj;
        public int mUniform_mmodel;
        public int mUniform_diffuse;

        public ComponentShaderDefault() : base("Shaders/vs.glsl", "Shaders/fs.glsl")
        {
            mUniform_stex = GL.GetUniformLocation(mPGMID, "s_texture");
            mUniform_mmodelviewproj = GL.GetUniformLocation(mPGMID, "ModelViewProjMat");
            mUniform_mmodel = GL.GetUniformLocation(mPGMID, "ModelMat");
            mUniform_diffuse = GL.GetUniformLocation(mPGMID, "v_diffuse");
        }

        public override void ApplyShader(Matrix4 pModel, Geometry pGeometry)
        {
            GL.UseProgram(mPGMID);
            GL.Uniform1(mUniform_stex, 0);

            GL.UniformMatrix4(mUniform_mmodel, false, ref pModel);
            Matrix4 modelViewProjection = pModel * GameScene.gameInstance.camera.view * GameScene.gameInstance.camera.projection;
            GL.UniformMatrix4(mUniform_mmodelviewproj, false, ref modelViewProjection);

            pGeometry.Render(mUniform_diffuse);

            GL.UseProgram(0);
        }
    }
}
