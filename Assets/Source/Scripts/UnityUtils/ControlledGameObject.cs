using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils {
    public class ControlledGameObject<T> where T : MonoBehaviour {
        public readonly GameObject gameObj;
        public readonly T controller;

        public ControlledGameObject(GameObject gameObj, T controller) {
            this.gameObj = gameObj;
            this.controller = controller;
        }

        public Transform transform {
            get {
                return gameObj.transform;
            }
        }
    }
}