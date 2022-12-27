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
        public bool mIsLooping;
        public bool mIsPlaying;

        public ComponentAudio(string pFileName, bool pIsLooping)
        {
            mAudioBuffer = ResourceManager.LoadAudio(pFileName);
            mAudioSource = AL.GenSource();
            AL.Source(mAudioSource, ALSourcei.Buffer, mAudioBuffer); // attach the buffer to a source
            AL.Source(mAudioSource, ALSourceb.Looping, pIsLooping);
            mIsLooping = pIsLooping;
            mIsPlaying = false;
        }

        public void SetPosition(Vector3 pPosition)
        {
            AL.Source(mAudioSource, ALSource3f.Position, ref pPosition);
        }

        public void PlayAudio()
        {
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
