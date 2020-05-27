using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;
        private Vector3 lastChangedPos;

        // Use this for initialization
        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            m_LastTargetPosition = target.position;
            lastChangedPos = transform.position;
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;
        }


        // Update is called once per frame
        private void Update()
        {
            // only update lookahead pos if accelerating or changed direction
            if(target == null)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward*m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);
            float x = newPos.x;
            float y = newPos.y;
            if (newPos.y < -0.5 || newPos.y >=7)
            {
                y = transform.position.y;
            }
            if(newPos.x < -0.5)
            {
                x = transform.position.x;
            }

            transform.position = new Vector3(x, y, transform.position.z);
            UpdateBackgroundTile(x, lastChangedPos.x);
            lastChangedPos = transform.position;
            m_LastTargetPosition = target.position;
        }
        void UpdateBackgroundTile(float newPosX, float lastPosX)
        {
            
            float xScale = (newPosX - lastPosX);
            GameObject bckgrd = GameObject.FindGameObjectWithTag("Background");
            if (bckgrd == null) return;
            foreach(SpriteRenderer child in bckgrd.GetComponentsInChildren<SpriteRenderer>())
            {
                child.size = new Vector2(child.size.x + xScale, child.size.y);
            }

        }
    }
}
