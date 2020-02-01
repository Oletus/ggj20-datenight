// Copyright Olli Etuaho 2018.

using UnityEngine;

namespace LPUnityUtils {

    /// <summary>
    /// Pointer. An abstraction over mouse and touch pointers.
    /// </summary>
    public class Pointer
    {
        public bool isTouch = false;
        public int fingerId = 0;

        public Vector3 position
        {
            get
            {
                if (this.isTouch)
                {
                    Touch touch;
                    GetTouchInternal (out touch);
                    return touch.position;
                }
                return Input.mousePosition;
            }
        }

        /// <summary>
        /// Return a new Pointer if there's a pointer down event on this frame. Should be called from Update().
        /// Does not react to multi-touch on the same frame. TODO: Add an alternative method to robustly handle multi-touch.
        /// </summary>
        /// <returns>The pointer down or null.</returns>
        public static Pointer CreateOnPointerDown()
        {
            if (Input.GetMouseButtonDown (0))
            {
                return new Pointer ();
            }
            else
            {
                for (int touchIndex = 0; touchIndex < Input.touchCount; ++touchIndex)
                {
                    Touch touch = Input.GetTouch (touchIndex);
                    if (touch.phase == TouchPhase.Began)
                    {
                        return Pointer.CreateFromTouch (touch);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Return a new Pointer if there's a pointer up event on this frame. Should be called from Update().
        /// Does not react to multi-touch on the same frame. TODO: Add an alternative method to robustly handle multi-touch.
        /// </summary>
        /// <returns>The pointer up or null.</returns>
        public static Pointer CreateOnPointerUp()
        {
            if (Input.GetMouseButtonUp (0))
            {
                return new Pointer ();
            }
            else
            {
                for (int touchIndex = 0; touchIndex < Input.touchCount; ++touchIndex)
                {
                    Touch touch = Input.GetTouch (touchIndex);
                    if (touch.phase == TouchPhase.Ended)
                    {
                        return Pointer.CreateFromTouch (touch);
                    }
                }
            }
            return null;
        }
    
        public static Pointer CreateFromTouch(Touch touch)
        {
            Pointer pointer = new Pointer ();
            pointer.isTouch = true;
            pointer.fingerId = touch.fingerId;
            return pointer;
        }

        public bool IsDown()
        {
            if (!this.isTouch)
            {
                return (Input.GetMouseButton (0));
            }
            Touch touch;
            bool hasTouch = GetTouchInternal (out touch);
            return hasTouch && touch.phase != TouchPhase.Canceled && touch.phase != TouchPhase.Ended;
        }

        public Ray GetRay(Camera cam)
        {
            return cam.ScreenPointToRay (this.position);
        }

        private bool GetTouchInternal(out Touch touch)
        {
            for (int index = 0; index < Input.touchCount; ++index)
            {
                if (this.fingerId == Input.GetTouch (index).fingerId)
                {
                    touch = Input.GetTouch (index);
                    return true;
                }
            }
            touch = new Touch ();
            return false;
        }
    }

}  // namespace LPUnityUtils
