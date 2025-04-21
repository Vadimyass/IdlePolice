using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.UIUtils
{
    public static class ImageUtils
    {
        public static Color GetColorByIntRGB(int r, int g, int b)
        {
            float newR = (float)r / 255;
            float newG = (float)g / 255;
            float newB = (float)b / 255;
            return new Color(newR, newG, newB);
        }
        
        public static Color GetColorByIntRGBA(int r, int g, int b,int a)
        {
            float newR = (float)r / 255;
            float newG = (float)g / 255;
            float newB = (float)b / 255;
            float newA = (float)a / 255;
            return new Color(newR, newG, newB,newA);
        }
    }
}