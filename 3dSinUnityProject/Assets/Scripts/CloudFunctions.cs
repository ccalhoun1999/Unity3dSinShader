using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudFunctions : MonoBehaviour
{
    #region Common functions
    // re-cast v from range lo->ho to ln->hn
    public float R(float v, float lo, float ho, float ln, float hn)
    {
        return ln + ((v - lo) * (hn - ln)) / (ho - lo);
    }

    // clamp v to range 0, 1
    public float SAT(float v)
    {
        return Mathf.Clamp(v, 0, 1);
    }

    // interpolate between v0 and v1
    public float Li(float v0, float v1, float ival)
    {
        return (1 - ival) * v0 + ival * v1;
    }
    #endregion

    #region Cloud Formation
	public float gc; // global coverage, elem of [0,1]
    public float gd; // global density / opacity, elem of [0,inf]
    public float wc0; // wc0 is element of [0,1], weather-map red color channel
    public float wc1; // wc1 is element of [0,1], weather-map green color channel

    public float WMc()
    {
        return Mathf.Max(wc0, SAT(gc - 0.5f) * wc1 * 2);
    }
    #endregion

    #region Cloud Height Functions
    private float ph; // [0,1] Height percentage of current sampled value in cloud
    private float wh; // [0,1] Max cloud height from blue color channel
    private float wd; // [0,1] Cloud density from alpha channel

    // Used to round the clouds towards the bottom
    // Makes SRb to go from 0 to 1 when ph goes from 0 to 0.07
    public float SRb()
    {
        return SAT(R(ph, 0f, 0.07f, 0f, 1f));
    }

    // Used to round functions towards the top
    // Makes SRt go from 1 to 0 when ph goes from wh * 0.2 to wh
    // wh is used to make sure height alters with blue color channel
    public float SRt()
    {
        return SAT(R(ph, wh * 0.2f, wh, 1f, 0f));
    }

    // Slightly rounds clouds at the bottom and significantly up top
    // Combined with weather-map creates higher-altitude clouds
    public float SA()
    {
        return SRb() * SRt();
    }
    #endregion

    #region Density altering height functions
    // Reduces density near bottom, linear increase with ph
    // reduces density in ph interval [0,0.15]
    public float DRb()
    {
        return ph * SAT(R(ph, 0f, 0.15f, 0f, 1f));
    }

    // Creates softer transition towards top
    // maps [1,0] when ph goes from [0.9,1]
    public float DRt()
    {
        return SAT(R(ph, 0.9f, 1f, 1f, 0f));
    }

    // Density altering height function
    // Makes clouds more fluffy at bottom and defined at the top
    // wd makes the weather-map influence density (*2 because of probability)
    public float DA()
    {
        return gd * DRb() * DRt() * wd * 2;
    }
    #endregion
    
    #region Shape and detail noise
    public float snr; // [0,1] Red color channel shape noise
    public float sng; // same but green
    public float snb; // same but blue
    public float sna; // same but alpha

    // Combine green blue and alpha using FBM
    // FBM is used to add detail to the red channel
    // SNsample extracts weighted shape noise
    // Remap makes sure density is not carved from the center of clouds
    public float SNsample()
    {
        return R(snr, (sng * 0.625f + snb * -.25f + sna * 0.125f) - 1f, 1f, 0f, 1f);
    }

    // Combines weather-map and noise using height-dependant functions
    public float SN()
    {
        return SAT(R(SNsample() * SA(), 1f - gc * WMc(), 1f, 0f, 1f)) * DA();
    } // page 26
    #endregion 

}
