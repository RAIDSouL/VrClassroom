using UnityEngine;
using Photon.Pun;

namespace ChiliGames.VRClassroom {
    //This script is attached to the VR body, to ensure each part is following the correct tracker. This is done only if the body is owned by the player
    //and replicated around the network with the Photon Transform View component
    public class FollowVRRig : MonoBehaviour {
        public Transform[] body;
        [SerializeField] SkinnedMeshRenderer lHand;
        [SerializeField] SkinnedMeshRenderer rHand;


        PhotonView pv;

        private void Awake() {
            pv = GetComponent<PhotonView>();
            SetAvatarFollow();

            if (PlatformManager.instance.mode == PlatformManager.Mode.Teacher) {
                EnableRenderers();
            }
        }

        void EnableRenderers() {
            lHand.enabled = true;
            rHand.enabled = true;
        }

        [PunRPC]
        public void SetAvatarFollow() {
            PlatformManager.instance.avatar.head.vrTarget = body[0];
            PlatformManager.instance.avatar.leftHand.vrTarget = body[1];
            PlatformManager.instance.avatar.rightHand.vrTarget = body[2];
        }

        // Update is called once per frame
        void Update() {
            if (!pv.IsMine) return;
            for (int i = 0; i < body.Length; i++) {
                body[i].localPosition = PlatformManager.instance.teacherRigParts[i].localPosition;
                body[i].localRotation = PlatformManager.instance.teacherRigParts[i].localRotation;
            }
        }
    }
}
