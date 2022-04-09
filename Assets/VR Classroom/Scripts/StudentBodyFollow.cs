using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;

namespace ChiliGames.VRClassroom {
    //This script is attached to the VR body, to ensure each part is following the correct tracker. This is done only if the body is owned by the player
    //and replicated around the network with the Photon Transform View component
    public class StudentBodyFollow : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {

        public Transform[] body;
        PhotonView pv;

        private void Awake() {
            pv = GetComponent<PhotonView>();
        }

        // Update is called once per frame
        void Update() {
            if (!pv.IsMine) return;
            for (int i = 0; i < body.Length; i++) {
                body[i].position = PlatformManager.instance.studentRigParts[i].position;
                body[i].rotation = PlatformManager.instance.studentRigParts[i].rotation;
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        public int Gender, Model, Hair, Skintone, Chest, Leg, Feet;
        public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            object[] instantiationData = info.photonView.InstantiationData;
            int[] AvatarData = (int[])instantiationData[0];
            Gender = AvatarData[0];
            Model = AvatarData[1];
            Hair = AvatarData[2];
            Skintone = AvatarData[3];
            Chest = AvatarData[4];
            Leg = AvatarData[5];
            Feet = AvatarData[6];
            Debug.LogErrorFormat("Gender {0} Model {1} Hair {2} Skintone {3} Chest {4} Leg {5} Feet {6}", Gender == 0 ? "Boy" : "Girl", Model, Hair, Skintone, Chest, Leg, Feet);
        }
    }
}
