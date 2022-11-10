using OpenTK;
using OpenGL_Game.Managers;
using System;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.OBJLoader;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Components
{
    class ComponentAudio : IComponent
    {
        public int mAudioBuffer;
        public int mAudioSource;

        public ComponentAudio(string pFileName)
        {
            mAudioBuffer = ResourceManager.LoadAudio("Audio/buzz.wav");
            mAudioSource = AL.GenSource();
            AL.Source(mAudioSource, ALSourcei.Buffer, mAudioBuffer); // attach the buffer to a source
            AL.Source(mAudioSource, ALSourceb.Looping, true);
        }

        public void SetPosition(Vector3 pPosition)
        {
            AL.Source(mAudioSource, ALSource3f.Position, ref pPosition);
            AL.SourcePlay(mAudioSource);
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }

        public void Close()
        {
            AL.DeleteBuffer(mAudioBuffer);
            AL.DeleteSource(mAudioSource);
        }
    }
}
