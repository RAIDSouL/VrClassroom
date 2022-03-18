using UnityEngine;
using Photon.Pun;

namespace ChiliGames.VRClassroom {
    //This class sends a Raycast from the marker and detect if it's hitting the whiteboard (tag: Finish)
    public class Marker : MonoBehaviour {
        private Whiteboard whiteboard;
        public Transform drawingPoint;
        public Renderer markerTip;
        private RaycastHit touch;
        private bool touching;
        private bool firstTimeTouching = true;
        float drawingDistance = 0.015f;
        Quaternion lastAngle;
        PhotonView pv;
        [SerializeField] int penSize = 8;
        [SerializeField] Color32 color = Color.blue;
        private bool grabbed;
        private int currentFrame = 0;

        public void ToggleGrab(bool b) {
            if (b) grabbed = true;
            else grabbed = false;
        }

        private void Start() {
            pv = GetComponent<PhotonView>();
            markerTip.material.color = color;
        }

        void Update() {
            //if the marker is not in possesion of the user, or is not grabbed, we don't run update.
            if (!pv.IsMine) return;
            if (!grabbed) return;

            //Run raycast every 2 frames for performance
            currentFrame++;
            if (currentFrame % 2 != 0)
            {
                return;
            }           

            //Cast a raycast to detect whiteboard.
            if (Physics.Raycast(drawingPoint.position, drawingPoint.up, out touch, drawingDistance)) {
                //The whiteboard has the tag "Finish".
                if (touch.collider.CompareTag("Finish")) {
                    if (!touching) {
                        touching = true;
                        lastAngle = transform.rotation;
                        whiteboard = touch.collider.GetComponent<Whiteboard>();
                    }
                    if (whiteboard == null) return;

                    //Save reference of marker ID, color and size the first time we touch the whiteboard
                    if (firstTimeTouching)
                    {
                        whiteboard.pv.RPC("RPC_StoreMarkerID", RpcTarget.AllBuffered, pv.ViewID, penSize, new float[] { color.r, color.g, color.b });
                        firstTimeTouching = false;
                    }

                    //Only send the RPC every 4 frames. For the teacher, the DrawAtPosition method is called every 2 frames (raycast frequency)
                    whiteboard.DrawAtPosition(pv.ViewID, new float[] { touch.textureCoord.x, touch.textureCoord.y });
                    if(currentFrame % 4 == 0)
                    {
                        whiteboard.pv.RPC("DrawAtPosition", RpcTarget.OthersBuffered, pv.ViewID, new float[] { touch.textureCoord.x, touch.textureCoord.y });
                        //reset frame counter every 4 frames
                        currentFrame = 0;
                    }
                }
            } else if (whiteboard != null) {
                touching = false;
                whiteboard.pv.RPC("ResetTouch", RpcTarget.AllBuffered, pv.ViewID); ;
                whiteboard = null;
            }
        }

        private void LateUpdate() {
            if (!pv.IsMine) return;

            //lock rotation of marker when touching whiteboard.
            if (touching) {
                transform.rotation = lastAngle;
            }
        }
    }
}

