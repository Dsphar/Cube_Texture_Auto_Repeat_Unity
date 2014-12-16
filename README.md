Texture Repeat (on a Unity Cube)
================================

A short C# Script that lets you change the scale of a Primitive Cube without stretching the texture. The texture will instead repeat itself at its original scale.

How To
======================

Step 1 - Add the "ReCalcCubeTexture.cs" script to your project

Step 2 - Attach it to any Cube GameObjects that you want it to effect

Step 3 - Anytime, after changing the scale of the object, call the reCalcCubeTexture() method

    Note:
    1- This can be done in Edit mode by clicking the "Update Texture" button in the Inspector.
    2- It can be done at runtime by using the line...
          gameObject.GetComponent<ReCalcCubeTexture>().reCalcCubeTexture();

Step 4 - Profit?
