using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;

namespace ChiliGames.VRClassroom
{
    //This script is attached to the VR body, to ensure each part is following the correct tracker. This is done only if the body is owned by the player
    //and replicated around the network with the Photon Transform View component
    public class StudentBodyFollow : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {

        public Transform[] body;
        public SkinnedMeshRenderer[] handMesh;
        public JointManager JointManager;
        public int sit;
        PhotonView pv;

        private void Awake()
        {
            pv = GetComponent<PhotonView>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!pv.IsMine)
            {
                handMesh[0].enabled = false;
                handMesh[1].enabled = false;               
                return;
            }
            for (int i = 0; i < body.Length; i++)
            {
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
            string type = (string)instantiationData[1];
            Gender = AvatarData[0];
            Model = AvatarData[1];
            Hair = AvatarData[2];
            Skintone = AvatarData[3];
            Chest = AvatarData[4];
            Leg = AvatarData[5];
            Feet = AvatarData[6];
            sit = (int)instantiationData[2];
            //  Debug.LogErrorFormat("{0} Gender {1} Model {2} Hair {3} Skintone {4} Chest {5} Leg {6} Feet {7} sit {8}", type, Gender == 0 ? "Boy" : "Girl", Model, Hair, Skintone, Chest, Leg, Feet, sit);

            GameObject mychar = PlatformManager.instance.ModelLoader.Load(Gender, Model, Hair, Skintone, Chest/*, Leg, Feet*/);
            mychar.GetComponentInChildren<Animator>().enabled = false;

            JointManager = mychar.GetComponent<JointManager>();

            if (sit > -1)
            {
                transform.position = PlatformManager.instance.studentdesk[sit].Charpos.position;
                transform.rotation = PlatformManager.instance.studentdesk[sit].Charpos.rotation;
                mychar.transform.position = PlatformManager.instance.studentdesk[sit].Charpos.position;
                mychar.transform.rotation = PlatformManager.instance.studentdesk[sit].Charpos.rotation;
            }

            mychar.transform.parent = this.transform;

            if (pv.IsMine)
            {
                PhotonTransformView ptv = JointManager.Controller.GetComponent<PhotonTransformView>();
                ptv.enabled = true;
                PlatformManager.instance.studentRig.transform.parent = this.transform;
                          
                PlatformManager.instance.SetStudentAndroid(this);
                mychar.gameObject.transform.localScale = Vector3.zero;
            }

        }
       

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(PropertiesKey.Handup))
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
