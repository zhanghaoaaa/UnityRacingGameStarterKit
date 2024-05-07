#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [AddComponentMenu("[ExUI]/运行时编辑器/AutoSelectUI")]
    public class AutoSelectUI : MonoBehaviour
    {
        [Header("是否是UI")]
        public bool isUI = true;
        [Header("是否选中暂停")]
        public bool isPaused = false;

        private static AutoSelectUI m_instance;
        public static AutoSelectUI Instance
        {
            get
            {
                if (m_instance == null)
                {
                    GameObject goObj = new GameObject("[Temp][AutoSelectUI]");
                    goObj.hideFlags = HideFlags.HideAndDontSave;
                    m_instance = goObj.AddComponent<AutoSelectUI>();
                }
                return m_instance;
            }
        }

        public static bool HasInstance
        {
            get { return m_instance != null; }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isUI)
                {
                    GameObject obj = GetTopMostUIObj();
                    if (obj)
                    {
                        Selection.activeObject = obj;
                        EditorGUIUtility.PingObject(Selection.activeObject);
                        if (isPaused)
                        {
                            EditorApplication.isPaused = true;
                        }
                    }
                }
                else
                {
                    if (Selection.activeObject)
                    {
                        EditorGUIUtility.PingObject(Selection.activeObject);
                    }
                }
            }
        }

        private GameObject GetTopMostUIObj()
        {
            List<RaycastResult> resList = null;
#if UNITY_ANDROID || UNITY_IPHONE
            resList = GetPointerRaycastUI(Input.GetTouch(0).position);
#else
            resList = GetPointerRaycastUI(Input.mousePosition);
#endif
            if (resList.Count > 0)
            {
                return resList[0].gameObject;
            }
            return null;
        }

        private List<RaycastResult> GetPointerRaycastUI(Vector2 screenPosition)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results;
        }
    }
}

#endif