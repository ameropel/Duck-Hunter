package com.Ameropel.DuckHunter;

import com.google.ads.AdRequest;
import com.google.ads.AdSize;
import com.google.ads.AdView;
import com.unity3d.player.UnityPlayer;
import android.app.Activity;
import android.view.ViewGroup.LayoutParams;
import android.widget.LinearLayout;


public class AdMob 
{
    private String pubID = "YOURADMOBPUBLICIDGOESHERE"; //Your public AdMob ID. Make sure this is correct, or you wont get any credit for the Ad.
    private Activity activity; //Store the android main activity
    private AdView adView; //The AdView we will display to the user
    private LinearLayout layout; //The layout the AdView will sit on
    
    //Constructor
    AdMob()
    {
    }
}
