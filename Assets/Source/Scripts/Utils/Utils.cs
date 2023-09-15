using System;
//using System.Collections;
//using System.Collections.Generic;

namespace Utils {
    public static class General { // using static Utils.General;
        public static bool StringContainsAny(string s, char[] chars) {
            foreach(char ch in chars) {
                if (s.Contains(ch)) {
                    return true;
                }
            }

            return false;
        }
    }
}
