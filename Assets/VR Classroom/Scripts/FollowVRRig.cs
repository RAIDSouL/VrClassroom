using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace ChiliGames.VRClassroom {
    //This script is attached to the VR body, to ensure each part is following the correct tracker. This is done only if the body is owned by the player
    //and replicated around the network with the Photon Transform View component
    public class FollowVRRig : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {
        public Transform[] body;
        [SerializeField] SkinnedMeshRenderer lHand;
        [SerializeField] SkinnedMeshRenderer rHand;
        public JointManager JointManager;
        int sit;
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
            string type = (string)instantiationData[1];
            Gender = AvatarData[0];
            Model = AvatarData[1];
            Hair = AvatarData[2];
            Skintone = AvatarData[3];
            Chest = AvatarData[4];
            Leg = AvatarData[5];
            Feet = AvatarData[6];
            sit = (int)instantiationData[2];
            Debug.LogErrorFormat("{0} Gender {1} Model {2} Hair {3} Skintone {4} Chest {5} Leg {6} Feet {7} sit {8}", type,Gender == 0 ? "Boy" : "Girl", Model, Hair, Skintone, Chest, Leg, Feet,sit);
            GameObject mychar = PlatformManager.instance.ModelLoader.Load(Gender,Model,Hair,Skintone,Chest/*,Leg,Feet*/);
            mychar.transform.parent = this.transform;
            JointManager = mychar.GetComponent<JointManager>();
            if (sit > -1)
            {
                mychar.transform.position = PlatformManager.instance.studentdesk[sit].Charpos.position;
                mychar.transform.rotation = PlatformManager.instance.studentdesk[sit].Charpos.rotation;
            }
            if (pv.IsMine)
            {
                if (PlatformManager.instance.mode == PlatformManager.Mode.Teacher)
                {
                    PlatformManager.instance.teacherRig.transform.parent = this.transform;
                    gameObject.GetComponentInChildren<Animator>().gameObject.transform.position = this.transform.position+ new Vector3(0, .33f, 0);                
                }
                else
                    PlatformManager.instance.studentRig.transform.parent = this.transform;

                mychar.gameObject.SetActive(false);
            }
        }
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(PropertiesKey.Handup)&& PlatformManager.instance.mode != PlatformManager.Mode.Teacher)
            {
                object changevalue = changedProps[PropertiesKey.Handup];
                object[] data = (object[])changevalue;
                int target = (int)data[0];
                if (sit == target)
                {
                    bool up = (bool)data[1];
                    JointManager.OnHandup(up);
                }
            }
        }
    }
}
