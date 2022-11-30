using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteToMesh : MonoBehaviour
{

    //面片的顶点数据
    static public Vector2[] quadVertices = {
        new Vector2(-0.5f, -0.5f),  
        new Vector2(0.5f, -0.5f),   //
        new Vector2(-0.5f, 0.5f),
        new Vector2(0.5f, 0.5f)
    };
    static public Vector2[] quadVertices2 = {
        new Vector2(0f, 0f),//左下
        new Vector2(1f, 0f),//右下
        new Vector2(0f, 1f),//左上
        new Vector2(1f, 1f)//右上
    };
    //面片的uv
    static public Vector2[] quadUVs = {
        new Vector2(0f, 0f),
        new Vector2(0.1f, 0f),
        new Vector2(0f, 1f),
        new Vector2(0.1f, 1f)
    };
    //面片的trangles
    static public ushort[] quadTrangles = {
        0,
        3,
        1,
        3,
        0,
        2
    };

    // Start is called before the first frame update
    void Start()
    {


        int gridcount = 10;
        SpriteRenderer sprender = GetComponent<SpriteRenderer>();
        List<Vector2> vector2s = new List<Vector2>();
        List<ushort> triangles = new List<ushort>();
        Sprite sp = sprender.sprite;


        //每个单位方块单位大小
        Vector2 unitsize = sp.textureRect.size / gridcount;

        Vector2[] quad = quadVertices2;
  

        ushort addt = 0;
        Vector2 atpos = unitsize;
        for (int i = 0; i < gridcount; i++)
        {
            for (int j = 0; j < gridcount; j++)
            {

               
                atpos.x = unitsize.x * j;
                atpos.y = unitsize.y * i;

                Vector2 p;
                p.x = quad[0].x * unitsize.x + atpos.x;
                p.y = quad[0].y * unitsize.y + atpos.y;
                vector2s.Add(p);
                p.x = quad[1].x * unitsize.x + atpos.x;
                p.y = quad[1].y * unitsize.y + atpos.y;
                vector2s.Add(p);
                p.x = quad[2].x * unitsize.x + atpos.x;
                p.y = quad[2].y * unitsize.y + atpos.y;
                vector2s.Add(p);
                p.x = quad[3].x * unitsize.x + atpos.x;
                p.y = quad[3].y * unitsize.y + atpos.y;
                vector2s.Add(p);
                

                for (int k = 0; k < quadTrangles.Length; k++)
                {
                    ushort us = quadTrangles[k];
                    us += addt;
                    triangles.Add(us);
                }
                addt += 4;


            }
        }

        Vector2[] vv = sp.vertices;
        ushort[] tt = sp.triangles;

        //for (int i = 0; i < vv.Length; ++i)
        //    vv[i] = (vv[i] * sp.pixelsPerUnit) + sp.pivot;
        //Mathf.Clamp();//用这个鉴定下范围最好
        //这个函数最后传入的顶点信息是图片大小的顶点信息（例如1000x500）需要传入一个角是(0，0)，一个角是(1000，500)
        sp.OverrideGeometry(vector2s.ToArray(), triangles.ToArray());
        //sp.OverrideGeometry(vv, tt);
    }


}
