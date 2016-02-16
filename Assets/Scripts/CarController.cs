    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
 
    [System.Serializable]
    public class AxleInfo {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor;
        public bool steering;
    }
         
    public class CarController : MonoBehaviour {
        public List<AxleInfo> axleInfos;
        public GameObject[] wheels = new GameObject[4];
        public float maxMotorTorque;
        public float maxSteeringAngle;
         
        // finds the corresponding visual wheel
        // correctly applies the transform
        public void ApplyLocalPositionToVisuals(WheelCollider collider)
        {
            if (collider.transform.childCount == 0) {
                return;
            }
         
            Transform visualWheel = collider.transform.GetChild(0);
         
            Vector3 position;
            Quaternion rotation;
            collider.GetWorldPose(out position, out rotation);
         
            visualWheel.transform.position = position;
            visualWheel.transform.rotation = rotation;
        }

        public void updateMesh()
        {
            for(int i = 0; i < 4 ; i++)
            {
                Quaternion quat;
                Vector3 pos;

                wheels[i].GetComponent<WheelCollider>().GetWorldPose(out pos, out quat);
                quat *= Quaternion.Euler(0, 0, 90);

                //wheels[i].transform.position = pos;
                wheels[i].transform.rotation = quat;
            }
        }

        public void FixedUpdate()
        {
            float motor = maxMotorTorque * Input.GetAxis("Vertical");
            float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
         
            foreach (AxleInfo axleInfo in axleInfos) {
                if (axleInfo.steering) {
                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;
                }
                if (axleInfo.motor) {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;
                }
                ApplyLocalPositionToVisuals(axleInfo.leftWheel);
                ApplyLocalPositionToVisuals(axleInfo.rightWheel);
                updateMesh();
            }
        }
    }
        