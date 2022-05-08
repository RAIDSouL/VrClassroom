using System.Linq;
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

namespace ChiliGames.VRClassroom
{
    public class Whiteboard : MonoBehaviourPunCallbacks
    {
        private int textureSize = 2048;
        private Texture2D texture;
        private Color32[] whitePixels;
        new Renderer renderer;

        private Dictionary<int, MarkerData> markerIDs = new Dictionary<int, MarkerData>();

        class MarkerData
        {
            public Color32[] color;
            public bool touchingLastFrame;
            public float[] pos;
            public int pensize;
            public float lastX;
            public float lastY;
        }

        [SerializeField] Renderer otherWhiteboards;

        bool applyingTexture;

        [HideInInspector] public PhotonView pv;

        void Start()
        {
            renderer = GetComponent<Renderer>();
            texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Trilinear;
            texture.anisoLevel = 3;
            renderer.material.mainTexture = texture;

            otherWhiteboards.material.mainTexture = texture;

            texture.Apply();

            pv = GetComponent<PhotonView>();

            //Set whiteboard to white
            whitePixels = Enumerable.Repeat(new Color32(255, 255, 255, 255),
                textureSize * textureSize).ToArray();
            ClearWhiteboard();
        }

        //RPC sent by the Marker class so every user gets the information to draw in whiteboard.
        [PunRPC]
        public void DrawAtPosition(int id, float[] _pos)
        {

            if (markerIDs.ContainsKey(id))
            {
                markerIDs[id].pos = _pos;
            }
            else
            {
                return;
            }

            int x = (int)(markerIDs[id].pos[0] * textureSize - markerIDs[id].pensize / 2);
            int y = (int)(markerIDs[id].pos[1] * textureSize - markerIDs[id].pensize / 2);

            //If last frame was not touching a marker, we don't need to lerp from last pixel coordinate to new, so we set the last coordinates to the new.
            if (!markerIDs[id].touchingLastFrame)
            {
                markerIDs[id].lastX = (float)x;
                markerIDs[id].lastY = (float)y;
                markerIDs[id].touchingLastFrame = true;
            }

            if (markerIDs[id].touchingLastFrame)
            {
                texture.SetPixels32(x, y, markerIDs[id].pensize, markerIDs[id].pensize, markerIDs[id].color);

                //Lerp last pixel to new pixel, so we draw a continuous line.
                for (float t = 0.01f; t < 1.00f; t += 0.1f)
                {
                    int lerpX = (int)Mathf.Lerp(markerIDs[id].lastX, (float)x, t);
                    int lerpY = (int)Mathf.Lerp(markerIDs[id].lastY, (float)y, t);
                    texture.SetPixels32(lerpX, lerpY, markerIDs[id].pensize, markerIDs[id].pensize, markerIDs[id].color);
                }

                //so it runs once per frame even if multiple markers are touching the whiteboard
                if (!applyingTexture)
                {
                    applyingTexture = true;
                    ApplyTexture();
                }
            }

            markerIDs[id].lastX = (float)x;
            markerIDs[id].lastY = (float)y;
        }

        public void ApplyTexture()
        {
            texture.Apply();
            applyingTexture = false;
        }

        [PunRPC]
        public void ResetTouch(int id)
        {
            if (markerIDs.ContainsKey(id))
                markerIDs[id].touchingLastFrame = false;
        }

        [PunRPC]
        public void RPC_StoreMarkerID(int id, int _pensize, float[] _color)
        {
            if (!markerIDs.ContainsKey(id))
            {
                markerIDs.Add(id, new MarkerData { touchingLastFrame = false, pensize = _pensize }); ;
                markerIDs[id].color = SetColor(new Color(_color[0], _color[1], _color[2]), id);
            }
        }

        //Creates the color array for the marker id
        public Color32[] SetColor(Color32 color, int id)
        {
            return Enumerable.Repeat(new Color32(color.r, color.g, color.b, 255), markerIDs[id].pensize * markerIDs[id].pensize).ToArray();
        }

        //To clear the whiteboard.
        public void ClearWhiteboard()
        {
            pv.RPC("RPC_ClearWhiteboard", RpcTarget.AllBuffered);
        }

        [PunRPC]
        public void RPC_ClearWhiteboard()
        {
            texture.SetPixels32(whitePixels);
            texture.Apply();
        }
    }
}
