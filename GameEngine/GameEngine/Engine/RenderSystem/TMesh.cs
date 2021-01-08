using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class TMeshVertex
    {
        public float x = 0, y = 0, z = 0;
        public float nx = 0, ny = 0, nz = 0;
        public float u = 0, v = 0;

        public TMeshVertex Copy()
        {
            TMeshVertex newObj = new TMeshVertex();
            newObj.x = this.x;
            newObj.y = this.y;
            newObj.z = this.z;
            newObj.nx = this.nx;
            newObj.ny = this.ny;
            newObj.nz = this.nz;
            newObj.u = this.u;
            newObj.v = this.v;
            return newObj;
        }
    };

    public class TMeshUV
    {
        public float u = 0;
        public float v = 0;
    }

    public class TMesh
    {
        private enum ReadType
        {
            INVALID,

            VERTEX,
            TRIANGLE,
            NORMAL,
            UV_VERT,
            UV_TRIANGLE,
            MAP_DIFFUSE,
            MAP_BUMP
        }
        public int VertexNum;
        public List<TMeshVertex> Verts = new List<TMeshVertex>();

        public int TriangleNum;
        public int[] TriangleIndices;

        public string DiffuseMap;
        public string BumpMap;

        public void Load(string aFileName)
        {
            string[] txt = File.ReadAllLines(aFileName);
            ReadType readtype = ReadType.INVALID;
            List<TMeshUV> uvs = new List<TMeshUV>();
            int ChannelMap = 0;
            int CurrentNormalTriangle = 0;
            int CurrentNormalVertex = 0;

            foreach(string line in txt)
            {
                if (line.Contains("*MESH_NUMVERTEX"))
                {
                    this.VertexNum = int.Parse(line.Split(' ')[1]);
                }
                if (line.Contains("*MESH_NUMFACES"))
                {
                    this.TriangleNum = int.Parse(line.Split(' ')[1]);
                    this.TriangleIndices = new int[this.TriangleNum * 3];
                }
                if (line.Contains("*MESH_VERTEX_LIST"))
                {
                    readtype = ReadType.VERTEX;
                    continue;
                }
                if (readtype == ReadType.VERTEX)
                {
                    if (line.Contains("*MESH_VERTEX"))
                    {
                        string[] data = line.Split('\t');
                        TMeshVertex vert = new TMeshVertex();
                        vert.x = float.Parse(data[4]);
                        vert.y = float.Parse(data[5]);
                        vert.z = float.Parse(data[6]);
                        this.Verts.Add(vert);
                    }
                }
                if (line.Contains("*MESH_FACE_LIST"))
                {
                    readtype = ReadType.TRIANGLE;
                    continue;
                }
                if(readtype == ReadType.TRIANGLE)
                {
                    if (line.Contains("*MESH_FACE"))
                    {
                        string pre = Regex.Replace(line, "\\s", "");
                        int m1 = pre.IndexOf("A:");
                        int m2 = pre.IndexOf("B:");
                        int m3 = pre.IndexOf("C:");
                        int m4 = pre.IndexOf("AB:");

                        int id = int.Parse(pre.Substring(10, m1 - 11));
                        int vert1ID = int.Parse(pre.Substring(m1 + 2, m2 - m1 - 2));
                        int vert2ID = int.Parse(pre.Substring(m2 + 2, m3 - m2 - 2));
                        int vert3ID = int.Parse(pre.Substring(m3 + 2, m4 - m3 - 2));

                        this.TriangleIndices[id * 3 + 0] = vert1ID;
                        this.TriangleIndices[id * 3 + 1] = vert2ID;
                        this.TriangleIndices[id * 3 + 2] = vert3ID;
                    }
                }
                if (line.Contains("*MESH_TVERTLIST") && ChannelMap == 0)
                {
                    readtype = ReadType.UV_VERT;
                    continue;
                }
                if(readtype == ReadType.UV_VERT)
                {
                    if (line.Contains("*MESH_TVERT"))
                    {
                        TMeshUV pair = new TMeshUV();
                        string pre = line.Substring(line.IndexOf("*MESH_TVERT"));
                        pair.u = float.Parse(pre.Split('\t')[1]);
                        pair.v = 1 - float.Parse(pre.Split('\t')[2]);
                        uvs.Add(pair);
                    }
                }
                if (line.Contains("*MESH_TFACELIST") && ChannelMap == 0)
                {
                    readtype = ReadType.UV_TRIANGLE;
                    continue;
                }
                if (readtype == ReadType.UV_TRIANGLE)
                {
                    if (line.Contains("*MESH_TFACE"))
                    {
                        string pre = line.Substring(line.IndexOf("*MESH_TFACE"));
                        string[] data = pre.Split('\t');
                        string head = data[0];
                        int triangleID = int.Parse(head.Split(' ')[1]);
                        int id1 = int.Parse(data[1]);
                        int id2 = int.Parse(data[2]);
                        int id3 = int.Parse(data[3]);

                        int vert1 = this.TriangleIndices[triangleID * 3 + 0];
                        int vert2 = this.TriangleIndices[triangleID * 3 + 1];
                        int vert3 = this.TriangleIndices[triangleID * 3 + 2];

                        if (this.Verts[vert1].u == 0 && this.Verts[vert1].v == 0)
                        {
                            this.Verts[vert1].u = uvs[id1].u;
                            this.Verts[vert1].v = uvs[id1].v;
                        }
                        else
                        {
                            if (this.Verts[vert1].u != uvs[id1].u || this.Verts[vert1].v != uvs[id1].v)
                            {
                                this.TriangleIndices[triangleID * 3 + 0] = this.VertexNum;
                                TMeshVertex newVert = this.Verts[vert1].Copy();
                                newVert.u = uvs[id1].u;
                                newVert.v = uvs[id1].v;
                                this.Verts.Add(newVert);
                                this.VertexNum++;
                            }
                        }


                        if (this.Verts[vert2].u == 0 && this.Verts[vert2].v == 0)
                        {
                            this.Verts[vert2].u = uvs[id2].u;
                            this.Verts[vert2].v = uvs[id2].v;
                        }
                        else
                        {
                            if (this.Verts[vert2].u != uvs[id2].u || this.Verts[vert2].v != uvs[id2].v)
                            {
                                this.TriangleIndices[triangleID * 3 + 1] = this.VertexNum;
                                TMeshVertex newVert = this.Verts[vert2].Copy();
                                newVert.u = uvs[id2].u;
                                newVert.v = uvs[id2].v;
                                this.Verts.Add(newVert);
                                this.VertexNum++;
                            }
                        }

                        if (this.Verts[vert3].u == 0 && this.Verts[vert3].v == 0)
                        {
                            this.Verts[vert3].u = uvs[id3].u;
                            this.Verts[vert3].v = uvs[id3].v;
                        }
                        else
                        {
                            if (this.Verts[vert3].u != uvs[id3].u || this.Verts[vert3].v != uvs[id3].v)
                            {
                                this.TriangleIndices[triangleID * 3 + 2] = this.VertexNum;
                                TMeshVertex newVert = this.Verts[vert3].Copy();
                                newVert.u = uvs[id3].u;
                                newVert.v = uvs[id3].v;
                                this.Verts.Add(newVert);
                                this.VertexNum++;
                            }
                        }
                    }
                }
                if (line.Contains("*MESH_MAPPINGCHANNEL"))
                {
                    readtype = ReadType.INVALID;
                    ChannelMap++;
                }
                if (line.Contains("*MESH_NORMALS"))
                {
                    readtype = ReadType.NORMAL;
                    continue;
                }
                if (readtype == ReadType.NORMAL)
                {
                    if (line.Contains("*MESH_FACENORMAL"))
                    {
                        string pre = line.Substring(line.IndexOf("*MESH_FACENORMAL"));
                        string[] data = pre.Split('\t');
                        string head = data[0];
                        CurrentNormalTriangle = int.Parse(head.Split(' ')[1]);
                        CurrentNormalVertex = 0;
                        continue;
                    }
                    if (line.Contains("*MESH_VERTEXNORMAL"))
                    {
                        string pre = line.Substring(line.IndexOf("*MESH_VERTEXNORMAL"));
                        string[] data = pre.Split('\t');
                        float nx = float.Parse(data[1]);
                        float ny = float.Parse(data[2]);
                        float nz = float.Parse(data[3]);
                        int vertID = this.TriangleIndices[CurrentNormalTriangle * 3 + CurrentNormalVertex];
                        
                        if (this.Verts[vertID].nx == 0 && this.Verts[vertID].ny == 0 && this.Verts[vertID].nz == 0)
                        {
                            this.Verts[vertID].nx = nx;
                            this.Verts[vertID].ny = ny;
                            this.Verts[vertID].nz = nz;
                        }
                        else
                        {
                            if (this.Verts[vertID].nx != nx || this.Verts[vertID].ny != ny || this.Verts[vertID].nz != nz)
                            {
                                this.TriangleIndices[CurrentNormalTriangle * 3 + CurrentNormalVertex] = this.VertexNum;
                                TMeshVertex newVert = this.Verts[vertID].Copy();
                                newVert.nx = nx;
                                newVert.ny = ny;
                                newVert.nz = nz;
                                this.Verts.Add(newVert);
                                this.VertexNum++;
                            }
                        }
                        CurrentNormalVertex++;
                    }
                }
                if(line.Contains("*MAP_DIFFUSE"))
                {
                    readtype = ReadType.MAP_DIFFUSE;
                    continue;
                }
                if (readtype == ReadType.MAP_DIFFUSE)
                {
                    if (line.Contains("*BITMAP"))
                    {
                        string pre = line.Substring(line.IndexOf("*BITMAP"));
                        string[] data = pre.Split(' ');
                        string file = data[1];
                        this.DiffuseMap = file.Replace("\"", "");
                        readtype = ReadType.INVALID;
                    }
                }
                if (line.Contains("*MAP_BUMP"))
                {
                    readtype = ReadType.MAP_BUMP;
                    continue;
                }
                if (readtype == ReadType.MAP_BUMP)
                {
                    if (line.Contains("*BITMAP"))
                    {
                        string pre = line.Substring(line.IndexOf("*BITMAP"));
                        string[] data = pre.Split(' ');
                        string file = data[1];
                        this.BumpMap = file.Replace("\"", "");
                        readtype = ReadType.INVALID;
                    }
                }
            }
        }
    }
}
