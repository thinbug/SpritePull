using UnityEngine;

using UnityEditor;


public class SpriteTools : Editor
{
    [MenuItem("Tools/ProcessPng")]
    public static void ProcessTileResources()
    {

        Process("main");

    }

    private static void Process(string file, int SIZEW = 128, int SIZEH = 128)
    {
        Texture2D sp = Resources.Load<Texture2D>( file);
        int W = sp.width;
        int H = sp.height;
        int ROW = H / SIZEH;
        int COLUMN = W / SIZEW;

        string path = "Assets\\Resources\\" + file + ".png";
        Debug.Log("processing tile resource:" + path);
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

        var blocks = new SpriteMetaData[COLUMN * ROW];

        for (int i = 0; i < ROW; ++i)
        {
            for (int j = 0; j < COLUMN; ++j)
            {
                int id = i * COLUMN + j;
                SpriteMetaData tmp = new SpriteMetaData();
                tmp.name = file + "_" + id;
                tmp.rect = new Rect(j * SIZEW, ROW * SIZEH - SIZEH - i * SIZEH, SIZEW, SIZEH);
                blocks[id] = tmp;
            }
        }
        textureImporter.spritesheet = blocks;
        textureImporter.fadeout = !textureImporter.fadeout;
        textureImporter.SaveAndReimport();

        //sb unity's bug(force refresh)
        textureImporter.fadeout = false;
        textureImporter.SaveAndReimport();
    }
}
