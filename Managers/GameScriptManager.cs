using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL_Game.Objects;
using System.IO;
using OpenGL_Game.Components;

namespace OpenGL_Game.Managers
{
    class GameScriptManager : ScriptManager
    {
        public GameScriptManager() { }

        public override void LoadEntityList(string pFileName, EntityManager pEntityManager)
        {
            try
            {
                Entity e = null;
                List<IComponent> eComponents = new List<IComponent>();
                StreamReader sr = new StreamReader(pFileName);
                string line = sr.ReadLine();

                while(line != null) //if line is null, the end of the file has been reached.
                {
                    while(line != "ENTITY_END") // if line is "ENTITY_END" a new entity must be declared.
                    {
                        if(line.Split('=')[0] == "NAME") //the line below this (or at the top of the script) should be the name for the new entity.
                        {
                            e = new Entity(line.Split('=')[1]);
                        }
                        else
                        {
                            ParseLine(line, e);
                        }
                        line = sr.ReadLine();
                    }
                    pEntityManager.AddEntity(e);
                    line = sr.ReadLine();
                }
            }
            catch
            {

            }
        }

        override protected void ParseLine(string pLine, Entity pEntity)
        {
            string[] splitLine = pLine.Split('=');

            switch (splitLine[0])
            {
                case "COMPONENT_AUDIO":
                    pEntity.AddComponent(new ComponentAudio(splitLine[1]));
                    break;

                case "COMPONENT_COLLISION_SPHERE":
                    pEntity.AddComponent(new ComponentCollisionSphere(float.Parse(splitLine[1])));
                    break;

                case "COMPONENT_GEOMETRY":
                    pEntity.AddComponent(new ComponentGeometry(splitLine[1]));
                    break;

                case "COMPONENT_POSITION":
                    string[] splitPosition = splitLine[1].Split(',');
                    float xPos = float.Parse(splitPosition[0]);
                    float yPos = float.Parse(splitPosition[1]);
                    float zPos = float.Parse(splitPosition[2]);

                    pEntity.AddComponent(new ComponentPosition(xPos, yPos, zPos));
                    break;

                case "COMPONENT_SHADER_DEFAULT":
                    pEntity.AddComponent(new ComponentShaderDefault());
                    break;

                case "COMPONENT_SHADER_NOLIGHTS":
                    pEntity.AddComponent(new ComponentShaderNoLights());
                    break;

                case "COMPONENT_TEXTURE":
                    break;

                case "COMPONENT_VELOCITY":
                    string[] velocitySplit = splitLine[1].Split(',');
                    float xVel = float.Parse(velocitySplit[0]);
                    float yVel = float.Parse(velocitySplit[1]);
                    float zVel = float.Parse(velocitySplit[2]);
                    pEntity.AddComponent(new ComponentVelocity(xVel, yVel, zVel));
                    break;

                default:
                    break;
            }
        }
    }
}
