using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Utils
{
    public static Color ColorBytes(byte r, byte g, byte b)
    {
        return new Color((float)r / 255, (float)g / 255, (float)b / 255);
    }

    public static List<Color> GeneratePalette()
    {
        // Palette unknown:
        //Color[] paletteSource = new Color[]
        //{
        //    Utils.ColorB(235, 53, 118),
        //    Utils.ColorB(96, 163, 114),
        //    Utils.ColorB(6, 165, 133),
        //    Utils.ColorB(120, 80, 118),
        //    Utils.ColorB(158, 84, 96)
        //};

        // "Vintage Modern":
        Color[] paletteSource = new Color[]
        {
            Utils.ColorBytes(140,35,24),
            Utils.ColorBytes(94, 140, 106),
            Utils.ColorBytes(136, 166, 94),
            Utils.ColorBytes(191, 179, 90),
            Utils.ColorBytes(242, 196, 90)
        };


        // "Cheer up emo kid"
        //Color[] paletteSource = new Color[]
        //{
        //    Utils.ColorB(85, 98, 112),
        //    Utils.ColorB(78, 205, 196),
        //    Utils.ColorB(199, 244, 100),
        //    Utils.ColorB(255, 107, 107),
        //    Utils.ColorB(196, 77, 88)
        //};


        int addToHue = Random.Range(0, 359);
        List<Color> pallete = new List<Color>();
        HSLColor hsl;
        for (int i = 0; i < 5; i++)
        {
            hsl = HSLColor.FromRGBA(paletteSource[i]);
            hsl.h += addToHue;
            pallete.Add(hsl.ToRGBA());
        }

        return pallete;
    }

    public static List<Color> GenerateShadowPalette(List<Color> palette, float shadowLuminance)
    {
        List<Color> shadowPalette = new List<Color>();
        foreach (Color c in palette)
        {
            HSLColor hsl = HSLColor.FromRGBA(c);
            hsl.l = shadowLuminance;
            shadowPalette.Add(hsl.ToRGBA());
        }
        return shadowPalette;
    }
}