﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace QuartersSDK {


    public class EditorUtils {
        
        [MenuItem("Quarters/Deauthorize Quarters User")]
        private static void DeAuthorize() {
            
            Session.Invalidate();
            Debug.Log("Quarters user deauthorized");
            
        }
    }
}