//Attach this script to your Cube.
//After you change the scale of the Cube, either
//	Click the "Update Texture" button [if in edit mode], or...
//	Call reCalcCubeTexture() [if in runtime]

#if UNITY_EDITOR //prevents contents from compiling into the final build
    using UnityEditor;
#endif

using UnityEngine;
using System.Collections;


public class ReCalcCubeTexture : MonoBehaviour 
{

    Vector3 currentScale = new Vector3();

    void Start()
    {
        currentScale = transform.localScale;   
    }

    //This update function is only here to provide an example.
    //Remove it in your actual application.
    void Update () {

        if ( Input.GetKeyDown ( KeyCode.P ) )
        {

            int smallest = 1;
            int largest = 5;

            Vector3 randomScale = new Vector3( Random.Range( smallest , largest ) , Random.Range( smallest , largest ) , Random.Range( smallest , largest ) );
           
            //At runtime, if you need to change your cube's scale...
            transform.localScale = randomScale;

            //then immediately call reCalcCubeTexture() to update the texture. That's it!
                reCalcCubeTexture(); 
                //Note: if calling from another script which is also attached to the Cube, use this instead...
                //gameObject.GetComponent<ReCalcCubeTexture>().reCalcCubeTexture();
        }
    }

    public void reCalcCubeTexture()
    {

        //if the scale has changed, do something...
        if ( currentScale != transform.localScale )
        {

            currentScale = transform.localScale;

          //if scale is ( 1, 1, 1 ) there is no need for a custom MeshFilter (for tiling the texture), revert to the standard cube MeshFilter
            if ( currentScale == Vector3.one )
            {

                GameObject cube = GameObject.CreatePrimitive( PrimitiveType.Cube );

                DestroyImmediate( GetComponent<MeshFilter>() );
                gameObject.AddComponent<MeshFilter>();
                GetComponent<MeshFilter>().sharedMesh = cube.GetComponent<MeshFilter>().sharedMesh;

                DestroyImmediate( cube );
                return;

            }


          //if still here, update the UV map on the mesh so the texture will repeat at the correct scale

            float length = currentScale.x;
            float width = currentScale.z;
            float height = currentScale.y;

            Mesh mesh;

            #if UNITY_EDITOR
                MeshFilter meshFilter = GetComponent<MeshFilter>();
                Mesh meshCopy = ( Mesh ) Mesh.Instantiate( meshFilter.sharedMesh );
                mesh = meshFilter.mesh = meshCopy;
            #else
                mesh = GetComponent<MeshFilter>().mesh;
            #endif

            Vector2[] mesh_UVs = mesh.uv;

            //update UV map
                //Front
                mesh_UVs[ 2 ] = new Vector2( 0 , height );
                mesh_UVs[ 3 ] = new Vector2( length , height );
                mesh_UVs[ 0 ] = new Vector2( 0 , 0 );
                mesh_UVs[ 1 ] = new Vector2( length , 0 );


                //Back
                mesh_UVs[ 6 ] = new Vector2( 0 , height );
                mesh_UVs[ 7 ] = new Vector2( length , height );
                mesh_UVs[ 10 ] = new Vector2( 0 , 0 );
                mesh_UVs[ 11 ] = new Vector2( length , 0 );


                //Left
                mesh_UVs[ 19 ] = new Vector2( 0 , height );
                mesh_UVs[ 17 ] = new Vector2( width , height );
                mesh_UVs[ 16 ] = new Vector2( 0 , 0 );
                mesh_UVs[ 18 ] = new Vector2( width , 0 );


                //Right
                mesh_UVs[ 23 ] = new Vector2( 0 , height );
                mesh_UVs[ 21 ] = new Vector2( width , height );
                mesh_UVs[ 20 ] = new Vector2( 0 , 0 );
                mesh_UVs[ 22 ] = new Vector2( width , 0 );


                //Top
                mesh_UVs[ 4 ] = new Vector2( 0 , width );
                mesh_UVs[ 5 ] = new Vector2( length , width );
                mesh_UVs[ 8 ] = new Vector2( 0 , 0 );
                mesh_UVs[ 9 ] = new Vector2( length , 0 );


                //Bottom
                mesh_UVs[ 15 ] = new Vector2( 0 , width );
                mesh_UVs[ 13 ] = new Vector2( length , width );
                mesh_UVs[ 12 ] = new Vector2( 0 , 0 );
                mesh_UVs[ 14 ] = new Vector2( length , 0 );

            //apply new UV map
            mesh.uv = mesh_UVs;
            mesh.name = "Cube Instance";
            if ( renderer.sharedMaterial.mainTexture.wrapMode != TextureWrapMode.Repeat )
                renderer.sharedMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
        
        }
    }
}


//Create Button to allow the Update while in Editor
#if UNITY_EDITOR
    [CustomEditor( typeof( ReCalcCubeTexture ) )]
    public class UpdateTextures : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ReCalcCubeTexture myScript = ( ReCalcCubeTexture ) target;
            if ( GUILayout.Button( "Update Texture" ) )
            {
                myScript.reCalcCubeTexture();
            }
        }
    }
#endif
