using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [Serializable]
    public class MouseLook
    {
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;
        public bool lockCursor = true;

        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;
        private bool m_cursorIsLocked = false;
        public bool CursorIsLocked => m_cursorIsLocked;

        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }

        public static bool GetMouseLockStatus() => Cursor.lockState == CursorLockMode.Locked;

        public void LookRotation(Transform character, Transform camera)
        {
            if (!m_cursorIsLocked) { return; }

            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

            if(clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);

            if(smooth)
            {
                character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }
        }

        public void UnlockCursor() => m_cursorIsLocked = false;
        public void LockCursor() => m_cursorIsLocked = true;

        public void ChangeForceCursorLock(bool value)
        {
            lockCursor = value;
            m_cursorIsLocked = value;
            if(!lockCursor)
            {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void UpdateCursorLock()
        {
            //if the user set "lockCursor" we check & properly lock the cursos
            if (lockCursor) { InternalLockUpdate(); }
        }

        private bool previousLockStatus { get; set; }
        private List<KeyCode> lockInputs = new List<KeyCode>() { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D };
        private List<KeyCode> unlockInputs = new List<KeyCode>() { KeyCode.Tab, KeyCode.Escape };
        
        private void InternalLockUpdate()
        {
            if (!m_cursorIsLocked)
            {
                for (int i = 0; i < lockInputs.Count; i++)
                {
                    if (Input.GetKeyDown(lockInputs[i])) { m_cursorIsLocked = true; }                    
                }

                if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()) { m_cursorIsLocked = true; }
            }

            if (m_cursorIsLocked)
            {
                for (int i = 0; i < unlockInputs.Count; i++)
                {
                    if (Input.GetKeyUp(unlockInputs[i])) { m_cursorIsLocked = false; }
                }
            }

            HandleCursorLock();
        }

        public void HandleCursorLock()
        {
            if (previousLockStatus == m_cursorIsLocked) { return; }
            previousLockStatus = m_cursorIsLocked;

            if (m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }            
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

            angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

    }
}
