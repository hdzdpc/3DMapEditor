using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapPCG;
using RoomGenerator;

public class MapGenerator : MonoBehaviour
{
    public int row = 10;
    public int col = 10;
    public int maxHeight = 10;
    [Range(0, 360)]
    public float rotationOffset = 0;
    [Range(0, 1)]
    public float sampleScale = 0.8f;
    public float viewBlockSize = 30;
    public GameObject virtual_camera;
    public AreaLeader areaLeaderTemplate;
    public Texture2D debugImage;
    public int room_height = 3;
    [Range(0, 1)]
    public float room_size = 0.1f;
    [Range(0, 1)]
    public float room_rand_rotate = 0.1f;

    public GameObject grass_block_top;
    public GameObject grass_block_cliff;
    public GameObject grass_block_cliff_top;
    public GameObject grass_block_hill;
    public GameObject grass_block_hill_corner_out;
    public GameObject grass_block_hill_corner_inner;

    public GameObject dust_block_top;
    public GameObject dust_block_cliff;
    public GameObject dust_block_cliff_top;
    public GameObject dust_block_hill;
    public GameObject dust_block_hill_corner_out;
    public GameObject dust_block_hill_corner_inner;

    public GameObject road_block_top;
    public GameObject road_block_cliff;
    public GameObject road_block_cliff_top;
    public GameObject road_block_hill;
    public GameObject road_block_hill_corner_out;
    public GameObject road_block_hill_corner_inner;
    public GameObject road_top_corner_inner;
    public GameObject road_top_corner_out;
    public GameObject road_top_side;
    public GameObject road_top_end;
    public GameObject road_top_single;
    public GameObject road_top_straight;

    public GameObject water_block_top;
    public GameObject water_block_cliff;
    public GameObject water_block_cliff_top;
    public GameObject water_block_hill;
    public GameObject water_block_hill_corner_out;
    public GameObject water_block_hill_corner_inner;
    public GameObject water_top_corner_inner;
    public GameObject water_top_corner_out;
    public GameObject water_top_side;
    public GameObject water_top_end;
    public GameObject water_top_single;
    public GameObject water_top_straight;
    public float planetScale = 2.4f;
    public float rockScale = 1f;

    public GameObject wall;
    public GameObject nonwall;
    public GameObject floor;
    public GameObject door;
    public GameObject[] windows;

    public GameObject[] tree;
    public GameObject[] rock;
    public GameObject[] grass;

    // DoubleBed, SmallContainer, Sofa, Table, TV, DiningTable, Toilet, Washer, Frozer, Cooker
    public GameObject bed;
    public GameObject smalCter;
    public GameObject sofa;
    public GameObject table;
    public GameObject tv;
    public GameObject diningTable;
    public GameObject toliet;
    public GameObject washer;
    public GameObject frozer;
    public GameObject cooker;
    public GameObject stair;


    public bool inUnity = true;  // ����¶���û�
    public double mapSize = 0;  // ����¶���û�
    public double landScale = 0.7;  // ½��ռ�ܵ�ͼ�ı���
    public int landWave = 5;  // ½������̶ȣ�Խ�����Խ��
    public double landHeight = 0.2;  // ½�����߶�
    public int islandNum = 3;  // ��������
    public double islandBalance = 1.0;  // ½�ؾ��ȳ̶ȣ�Խ��Խ���ȣ�ȡֵ��Χ0.0--1.0
    public int hillNum = 3;  // ��������
    public double hillScale = 0.4;  // ����ռ½�صı���
    public double hillBalance = 0.8;  // ������ȳ̶ȣ�Խ��Խ���ȣ�ȡֵ��Χ0.0--1.0
    public int hillWave = 10;  // ��������̶ȣ�Խ�����Խ��
    public double hillHeight = 1.0;  // �������߶�
    public int riverWidth = 1;  // ������ȣ�Խ��Խ��
    public int townNum = 3;  // ��������
    public double townScale = 0.3;  // ����ռ½�صı���
    public double townBalance = 1.0;  // ������ȳ̶ȣ�Խ��Խ���ȣ�ȡֵ��Χ0.0--1.0
    public int roomNum = 4;  // ÿ������ķ�������
    public double treeCover = 0.05;  // ��ľ������
    public double grassCover = 0.4;  // �ݸ�����
    public double rockCover = 0.05;  // ʯͷ������
    public bool hillGrowTree = false;  // �������Ƿ񳤲�



    public Color[] colors;
    public float[] color_powers;
    public GameObject display;

    MapSurfaceData[,] surface;
    AreaLeader[,] areaLeader;

    /*static public MapObjectData demo()
    {




        List<Requirement> req = new List<Requirement>();
        Requirement r;
        r = new Requirement(7, 7, false, RoomType.social, 1);
        req.Add(r);
        r = new Requirement(4, 4, false, RoomType.social, 0);
        req.Add(r);
        r = new Requirement(3, 3, false, RoomType.none, -1);
        req.Add(r);
        r = new Requirement(5, 5, true, RoomType.personal, -1);
        req.Add(r);


        MapPCG.Rectangle rectangle = MapPCG.TownPCG.GenerateRectangles(10, 10, 100, 100, 1)[0];
        MapPCG.Building building = MapPCG.TownPCG.GenerateBuilding(rectangle);
        return RoomGenerator.RoomGenerator.Generate(building, req);
    }*/

    /*MapSurfaceData[,] surfaceTestGenerator()
    {
        float scale = sampleScale;
        MapSurfaceData[,] res = new MapSurfaceData[row, col];

        Polygon pg = new Polygon();
        pg.points = new Point[]{ 
            new Point(row * 1 / 10, col * 1 / 10),
            new Point(row * 5 / 10, 1),
            new Point(row * 9 / 10, col * 1 / 10),
            new Point(row, col * 5 / 10),
            new Point(row * 9 / 10, col * 1 / 10),
            new Point(row * 5 / 10, col),
            new Point(row * 1 / 10, col * 9 / 10),
            new Point(1, col * 5 / 10),
            new Point(row * 1 / 10, col * 1 / 10)
        };
        pg.count = pg.points.Length;
        MapData md = new HillsPCG().GenerateHillsMap(pg, 8);

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                res[i, j] = new MapSurfaceData();
                
                res[i, j].height = Mathf.FloorToInt(md.heightmap[(int)(i * scale), (int)(j * scale)] * maxHeight);
                if (md.maptype[(int)(i * scale), (int)(j * scale)] == -1)
                    res[i, j].height = 0;

                if (res[i, j].height > maxHeight)
                    res[i, j].type = SurfaceTypes.DUST;
                else
                    res[i, j].type = SurfaceTypes.GRASS_BLOCK;
            }
        }

        int road_s_x = Random.Range(0, row);
        int road_s_y = Random.Range(0, col);
        for (int i = 0; i < 150; i++)
        {
            int[] dx = { -1, 0, 1, 0 };
            int[] dy = { 0, -1, 0, 1 };
            bool end = true;
            res[road_s_x, road_s_y].type = SurfaceTypes.ROAD;
            for (int t = 0; t < 8; t++)
            {
                int ix = Random.Range(0, 4);
                int x = road_s_x + dx[ix];
                int y = road_s_y + dy[ix];
                bool inbox = x >= 0 && x < row && y >= 0 && y < col;
                int tmp = 0;
                if (inbox && x + 1 < row && res[x + 1, y].type == SurfaceTypes.ROAD) tmp++;
                if (inbox && x - 1 > -1 && res[x - 1, y].type == SurfaceTypes.ROAD) tmp++;
                if (inbox && y + 1 < col && res[x, y + 1].type == SurfaceTypes.ROAD) tmp++;
                if (inbox && y - 1 > -1 && res[x, y - 1].type == SurfaceTypes.ROAD) tmp++;
                
                if (x >= 0 && x < row && y >= 0 && y < col && 
                    (res[x, y].type == SurfaceTypes.DUST || res[x, y].type == SurfaceTypes.GRASS_BLOCK) &&
                    tmp == 1)
                {
                    res[x, y].type = SurfaceTypes.ROAD;
                    end = false;
                    road_s_x = x;
                    road_s_y = y;
                    break;
                }
                
            }
            if (end) break;
        }

        return res;
    }*/

    void GetBuiltiFace(MapSurfaceData[,] md, int x, int y, Vector3 pos, Transform pa)
    {
        GameObject top, corneri, cornero, side, end, single, straight;
        Quaternion rotation = new Quaternion();
        switch (md[x, y].type)
        {
            case SurfaceTypes.ROAD:
                top = road_block_top;
                corneri = road_top_corner_inner;
                cornero = road_top_corner_out;
                side = road_top_side;
                end = road_top_end;
                single = road_top_single;
                straight = road_top_straight;
                break;
            default:
                top = water_block_top;
                corneri = water_top_corner_inner;
                cornero = water_top_corner_out;
                side = water_top_side;
                end = water_top_end;
                single = water_top_single;
                straight = water_top_straight;
                break;
        }

        int[] dx = { -1, 0, 1, 0, -1, 1, 1, -1 };
        int[] dy = { 0, -1, 0, 1, -1, 1, -1, 1 };

        int status = 0;
        for (int i = 7; i >= 0; i--)
        {
            SurfaceTypes ntype = SurfaceTypes.WATER;
            int nheight = 0;
            if (x + dx[i] >= 0 && x + dx[i] < row && y + dy[i] >= 0 && y + dy[i] < col)
            {
                ntype = md[x + dx[i], y + dy[i]].type;
                nheight = (int)(md[x + dx[i], y + dy[i]].height);
            }
            status <<= 1;
            if (ntype == md[x, y].type)
                status |= 1;
        }

        int cnt1 = 0;
        if ((status & 1) > 0) cnt1++;
        if ((status & 2) > 0) cnt1++;
        if ((status & 4) > 0) cnt1++;
        if ((status & 8) > 0) cnt1++;


        if (cnt1 == 0)
        {
            rotation.eulerAngles = new Vector3(0, md[x, y].placeDirection * 90 + rotationOffset, 0);
            Instantiate(single, pos, rotation, pa);
        }
        if (cnt1 == 1)
        {
            if ((status & 1) > 0)
            {
                rotation.eulerAngles = new Vector3(0, 180 + rotationOffset, 0);
                Instantiate(end, pos, rotation, pa);
            }
            if ((status & 2) > 0)
            {
                rotation.eulerAngles = new Vector3(0, 90 + rotationOffset, 0);
                Instantiate(end, pos, rotation, pa);
            }
            if ((status & 4) > 0)
            {
                rotation.eulerAngles = new Vector3(0, 0 + rotationOffset, 0);
                Instantiate(end, pos, rotation, pa);
            }
            if ((status & 8) > 0)
            {
                rotation.eulerAngles = new Vector3(0, 270 + rotationOffset, 0);
                Instantiate(end, pos, rotation, pa);
            }
        }
        if (cnt1 == 2)
        {
            if ((status & 1) > 0 && (status & 2) > 0)
            {
                rotation.eulerAngles = new Vector3(0, 180 + rotationOffset, 0);
                Instantiate(corneri, pos, rotation, pa);
            }
            if ((status & 2) > 0 && (status & 4) > 0)
            {
                rotation.eulerAngles = new Vector3(0, 90 + rotationOffset, 0);
                Instantiate(corneri, pos, rotation, pa);
            }
            if ((status & 4) > 0 && (status & 8) > 0)
            {
                rotation.eulerAngles = new Vector3(0, 0 + rotationOffset, 0);
                Instantiate(corneri, pos, rotation, pa);
            }
            if ((status & 8) > 0 && (status & 1) > 0)
            {
                rotation.eulerAngles = new Vector3(0, 270 + rotationOffset, 0);
                Instantiate(corneri, pos, rotation, pa);
            }

            if (((status & 1) > 0 && (status & 4) > 0))
            {
                rotation.eulerAngles = new Vector3(0, 180 + rotationOffset, 0);
                Instantiate(straight, pos, rotation, pa);
            }
            if (((status & 8) > 0 && (status & 2) > 0))
            {
                rotation.eulerAngles = new Vector3(0, 90 + rotationOffset, 0);
                Instantiate(straight, pos, rotation, pa);
            }
        }
        if (cnt1 == 3)
        {
            if ((status & 1) > 0 && (status & 2) > 0 && (status & 4) > 0)
            {
                rotation.eulerAngles = new Vector3(0, 90 + rotationOffset, 0);
                Instantiate(side, pos, rotation, pa);
            }
            if ((status & 2) > 0 && (status & 4) > 0 && (status & 8) > 0)
            {
                rotation.eulerAngles = new Vector3(0, 0 + rotationOffset, 0);
                Instantiate(side, pos, rotation, pa);
            }
            if ((status & 4) > 0 && (status & 8) > 0 && (status & 1) > 0)
            {
                rotation.eulerAngles = new Vector3(0, 270 + rotationOffset, 0);
                Instantiate(side, pos, rotation, pa);
            }
            if ((status & 8) > 0 && (status & 1) > 0 && (status & 2) > 0)
            {
                rotation.eulerAngles = new Vector3(0, 180 + rotationOffset, 0);
                Instantiate(side, pos, rotation, pa);
            }
        }
        if (cnt1 == 4)
        {
            rotation.eulerAngles = new Vector3(0, md[x, y].placeDirection * 90 + rotationOffset, 0);
            Instantiate(top, pos, rotation, pa);
        }

        if ((status & 16) == 0 && (status & 1) > 0 && (status & 2) > 0)
        {
            rotation.eulerAngles = new Vector3(0, 90 + rotationOffset, 0);
            Instantiate(cornero, pos, rotation, pa);
        }
        if ((status & 128) == 0 && (status & 1) > 0 && (status & 8) > 0)
        {
            rotation.eulerAngles = new Vector3(0, 180 + rotationOffset, 0);
            Instantiate(cornero, pos, rotation, pa);
        }
        if ((status & 32) == 0 && (status & 4) > 0 && (status & 8) > 0)
        {
            rotation.eulerAngles = new Vector3(0, 270 + rotationOffset, 0);
            Instantiate(cornero, pos, rotation, pa);
        }
        if ((status & 64) == 0 && (status & 2) > 0 && (status & 4) > 0)
        {
            rotation.eulerAngles = new Vector3(0, 0 + rotationOffset, 0);
            Instantiate(cornero, pos, rotation, pa);
        }

    }

    private void RenderMapObject(MapObjectData mobj, Vector3 pos, Transform root, bool onhill)
    {
        GameObject pa = new GameObject("ROOT");
        pa.transform.parent = root;
        Vector3 position = new Vector3();
        Quaternion rotation = new Quaternion();
        pa.transform.position = pos;

        bool topCover = false;

        if (mobj.staircase != null)
        {
            for (int z = 0; z < mobj.voxel_types.GetLength(2) - 1; z++)
            {
                position = new Vector3(mobj.staircase[0], pos.y + z * room_height, mobj.staircase[1]);
                Transform lparent = areaLeader[(int)((mobj.staircase[0]) / viewBlockSize),
                    (int)((mobj.staircase[1]) / viewBlockSize)].visuableKey.transform;
                Instantiate(stair, position, rotation, lparent);
            }
        }
        
        for (int x = 0; x < mobj.voxel_types.GetLength(0); x++)
        {
            for (int y = 0; y < mobj.voxel_types.GetLength(1); y++)
            {
                for (int z = 0; z < mobj.voxel_types.GetLength(2); z++)
                {
                    Transform lparent = areaLeader[(int)((pos.x + x) / viewBlockSize), (int)((pos.z + y) / viewBlockSize)].visuableKey.transform;
                    position = pos + new Vector3(x, z * room_height, y);
                    VoxelType type = mobj.voxel_types[x, y, z];
                    int dir = mobj.voxel_direction[x, y, z];
                    if (z == mobj.voxel_types.GetLength(2) - 1) 
                        topCover = true;
                    else 
                        topCover = false;
                    if ((type == VoxelType.WALL || type == VoxelType.DOOR || type == VoxelType.WINDOW) &&
                        dir != 0)
                    {
                        GameObject obj = wall;
                        if (type == VoxelType.DOOR)
                        {
                            obj = door;
                        }
                        if (type == VoxelType.WINDOW)
                        {
                            obj = windows[Random.Range(0, windows.Length)];
                        }

                        if (dir > 0 && type != VoxelType.GRASS && type != VoxelType.TREE && type != VoxelType.ROCK)
                        {
                            rotation.eulerAngles = new Vector3();
                            Instantiate(floor, position, rotation, lparent);
                            if (topCover)
                                Instantiate(floor, position + new Vector3(0, room_height, 0), rotation, lparent);
                        }

                        GameObject walllike = null;

                        if ((Mathf.Abs(dir) & 1) > 0)
                        {
                            rotation.eulerAngles = new Vector3(0, 270, 0);
                            walllike = Instantiate(obj, position, rotation, lparent);
                            walllike.transform.localScale = new Vector3(walllike.transform.localScale.x,
                                walllike.transform.localScale.y * room_height, walllike.transform.localScale.z);
                        }
                        if ((Mathf.Abs(dir) & 2) > 0)
                        {
                            rotation.eulerAngles = new Vector3(0, 0, 0);
                            walllike = Instantiate(obj, position, rotation, lparent);
                            walllike.transform.localScale = new Vector3(walllike.transform.localScale.x,
                                walllike.transform.localScale.y * room_height, walllike.transform.localScale.z);
                        }
                        if (Mathf.Abs(dir) > 128 && z == 0)
                        {
                            rotation.eulerAngles = new Vector3(0, 0, 0);
                            walllike = Instantiate(door, position, rotation, lparent);
                            walllike.transform.localScale = new Vector3(walllike.transform.localScale.x,
                                walllike.transform.localScale.y * room_height, walllike.transform.localScale.z);

                            rotation.eulerAngles = new Vector3(0, 270, 0);
                            walllike = Instantiate(wall, position, rotation, lparent);
                            walllike.transform.localScale = new Vector3(walllike.transform.localScale.x,
                                walllike.transform.localScale.y * room_height, walllike.transform.localScale.z);
                        }
                        if (Mathf.Abs(dir) > 128 && z > 0)
                        {
                            rotation.eulerAngles = new Vector3(0, 0, 0);
                            walllike = Instantiate(windows[Random.Range(0, windows.Length)], position, rotation, lparent);
                            walllike.transform.localScale = new Vector3(walllike.transform.localScale.x,
                                walllike.transform.localScale.y * room_height, walllike.transform.localScale.z);

                            rotation.eulerAngles = new Vector3(0, 270, 0);
                            walllike = Instantiate(wall, position, rotation, lparent);
                            walllike.transform.localScale = new Vector3(walllike.transform.localScale.x,
                                walllike.transform.localScale.y * room_height, walllike.transform.localScale.z);
                        }
                    }
                    if (type == VoxelType.FLOOR)
                    {
                        rotation.eulerAngles = new Vector3();
                        if (((x - 1 != Mathf.RoundToInt(mobj.staircase[0] - pos.x) ||
                            y - 1 != Mathf.RoundToInt(mobj.staircase[1] - pos.z)) &&
                            (x - 1 != Mathf.RoundToInt(mobj.staircase[0] - pos.x) ||
                            y != Mathf.RoundToInt(mobj.staircase[1] - pos.z)) &&
                            (x - 1 != Mathf.RoundToInt(mobj.staircase[0] - pos.x) ||
                            y - 2 != Mathf.RoundToInt(mobj.staircase[1] - pos.z))) ||
                            z == 0)
                            Instantiate(floor, position, rotation, lparent);
                        if (topCover)
                            Instantiate(floor, position + new Vector3(0, room_height, 0), rotation, lparent);

                    }

                    Vector3 hillFix = new Vector3(0, onhill ? 0.33f : 0, 0);

                    if (type == VoxelType.GRASS)
                    {
                        rotation.eulerAngles = new Vector3();
                        GameObject plt = Instantiate(grass[Random.Range(0, grass.Length)], position + hillFix, rotation, lparent);
                        plt.transform.localScale = plt.transform.localScale * planetScale;
                    }

                    if (type == VoxelType.TREE)
                    {
                        rotation.eulerAngles = new Vector3();
                        GameObject plt = Instantiate(tree[Random.Range(0, tree.Length)], position + hillFix, rotation, lparent);
                        plt.transform.localScale = plt.transform.localScale * planetScale;

                    }

                    if (type == VoxelType.ROCK)
                    {
                        rotation.eulerAngles = new Vector3();
                        GameObject plt = Instantiate(rock[Random.Range(0, rock.Length)], position + hillFix, rotation, lparent);
                        plt.transform.localScale = plt.transform.localScale * rockScale;
                    }

                }
            }
        }

        for (int x = 0; mobj.fur_types != null && x < mobj.fur_types.GetLength(0); x++)
        {
            for (int y = 0; y < mobj.fur_types.GetLength(1); y++)
            {
                for (int z = 0; z < mobj.fur_types.GetLength(2); z++)
                {

                    Transform lparent = areaLeader[(int)((pos.x + x) / viewBlockSize), (int)((pos.z + y) / viewBlockSize)].visuableKey.transform;
                    position = pos + new Vector3(x, z * room_height, y);

                    FurType type = mobj.fur_types[x, y, z];
                    int dir = mobj.fur_direction[x, y, z];
                    int offset = 1;

                    if (mobj.staircase != null &&
                        Vector2.Distance(new Vector2(position.x, position.z),
                        new Vector2(mobj.staircase[0], mobj.staircase[1])) < 5)
                    {
                        continue;
                    }

                    if (type == FurType.Sofa)
                    {
                        rotation.eulerAngles = new Vector3(0, (dir + offset) * 90, 0);
                        Instantiate(sofa, position, rotation, lparent);
                    }
                    if (type == FurType.Cooker)
                    {
                        rotation.eulerAngles = new Vector3(0, (dir + offset) * 90, 0);
                        Instantiate(cooker, position, rotation, lparent);
                    }
                    if (type == FurType.DoubleBed)
                    {
                        rotation.eulerAngles = new Vector3(0, (dir + offset) * 90, 0);
                        Instantiate(bed, position, rotation, lparent);
                    }
                    if (type == FurType.Frozer)
                    {
                        rotation.eulerAngles = new Vector3(0, (dir + offset) * 90, 0);
                        Instantiate(frozer, position, rotation, lparent);
                    }
                    if (type == FurType.DiningTable)
                    {
                        rotation.eulerAngles = new Vector3(0, (dir + offset) * 90, 0);
                        Instantiate(diningTable, position, rotation, lparent);
                    }
                    if (type == FurType.SmallContainer)
                    {
                        rotation.eulerAngles = new Vector3(0, (dir + offset) * 90, 0);
                        Instantiate(smalCter, position, rotation, lparent);
                    }
                    if (type == FurType.Table)
                    {
                        rotation.eulerAngles = new Vector3(0, (dir + offset) * 90, 0);
                        Instantiate(table, position, rotation, lparent);
                    }
                    if (type == FurType.Toilet)
                    {
                        rotation.eulerAngles = new Vector3(0, (dir + offset) * 90, 0);
                        Instantiate(toliet, position, rotation, lparent);
                    }
                    if (type == FurType.TV)
                    {
                        rotation.eulerAngles = new Vector3(0, (dir + offset) * 90, 0);
                        Instantiate(tv, position, rotation, lparent);
                    }
                    if (type == FurType.Washer)
                    {
                        rotation.eulerAngles = new Vector3(0, (dir + offset) * 90, 0);
                        Instantiate(washer, position, rotation, lparent);
                    }
                    
                }
            }
        }
        pa.transform.localScale = new Vector3(mobj.scale[0], mobj.scale[2], mobj.scale[1]);
        pa.transform.localScale *= room_size;
        pa.transform.Rotate(new Vector3(0, 360 * Random.Range(0f, room_rand_rotate), 0));
    }

    public void clear()
    {
        int childCount = gameObject.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(gameObject.transform.GetChild(i).gameObject);
        }
    }

    void GetNiceNeighbor(int x, int y,
        out GameObject cliffVoxel,
        out GameObject hillVoxel,
        out GameObject cornerIVoxel,
        out GameObject cornerOVoxel,
        out GameObject cliffTop)
    {
        if (x < 0 || y < 0 || x >= row || y >= col)
        {
            cliffVoxel = dust_block_cliff;
            hillVoxel = dust_block_hill;
            cornerIVoxel = dust_block_hill_corner_inner;
            cornerOVoxel = dust_block_hill_corner_out;
            cliffTop = dust_block_cliff_top;
            return;
        }
        switch (surface[x, y].type)
        {
            case SurfaceTypes.GRASS_BLOCK:
                cliffVoxel = grass_block_cliff;
                hillVoxel = grass_block_hill;
                cornerIVoxel = grass_block_hill_corner_inner;
                cornerOVoxel = grass_block_hill_corner_out;
                cliffTop = grass_block_cliff_top;
                break;
            case SurfaceTypes.ROAD:
                cliffVoxel = road_block_cliff;
                hillVoxel = road_block_hill;
                cornerIVoxel = road_block_hill_corner_inner;
                cornerOVoxel = road_block_hill_corner_out;
                cliffTop = road_block_cliff_top;
                break;
            case SurfaceTypes.BRIDGE:
                cliffVoxel = road_block_cliff;
                hillVoxel = road_block_hill;
                cornerIVoxel = road_block_hill_corner_inner;
                cornerOVoxel = road_block_hill_corner_out;
                cliffTop = road_block_cliff_top;
                break;
            case SurfaceTypes.WATER:
                cliffVoxel = water_block_cliff;
                hillVoxel = water_block_hill;
                cornerIVoxel = water_block_hill_corner_inner;
                cornerOVoxel = water_block_hill_corner_out;
                cliffTop = water_block_cliff_top;
                break;
            default:
                cliffVoxel = dust_block_cliff;
                hillVoxel = dust_block_hill;
                cornerIVoxel = dust_block_hill_corner_inner;
                cornerOVoxel = dust_block_hill_corner_out;
                cliffTop = dust_block_cliff_top;
                break;
        }
    }

    public void gennerator()
    {
        int childCount = gameObject.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(gameObject.transform.GetChild(i).gameObject);
        }
        // surface = MapFromImage.fromImage(debugImage, row, col);
        // surface = WorldGennerator.Util.GennerateMap(true, row, col, 0, 0.7,
        // 5, 0.2, 3, 1.0, 3, 0.4, 0.8, 10, 1.0, 1, 3, 0.3, 1.0, 4, 0.05, 0.4, 0.05, false);

        surface = WorldGennerator.Util.GennerateMap(true, row, col, 0, landScale,
            landWave, landHeight, islandNum, islandBalance, hillNum, hillScale, hillBalance, hillWave,
            hillHeight, riverWidth, townNum, townScale, townBalance, roomNum, treeCover, grassCover,
            rockCover, hillGrowTree);

        HeightMapDisplay.colors = colors;
        HeightMapDisplay.color_powers = color_powers;
        HeightMapDisplay.display = display;
        float[,] hmap = new float[row, col];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                hmap[i, j] = surface[i, j].height;
            }
        }
        HeightMapDisplay.ShowHeightMap(hmap);

        areaLeader = new AreaLeader[(int)(row / viewBlockSize) + 1, (int)(col / viewBlockSize) + 1];

        for (int i = 0; i < areaLeader.GetLength(0); i++)
        {
            for (int j = 0; j < areaLeader.GetLength(1); j++)
            {
                Vector3 lpos = new Vector3(i * viewBlockSize + viewBlockSize / 2, 
                    0, j * viewBlockSize + viewBlockSize / 2);
                areaLeader[i, j] = Instantiate(areaLeaderTemplate, lpos, new Quaternion(), gameObject.transform);
                areaLeader[i, j].vcamera = virtual_camera;
            }
        }

        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < col; y++)
            {
                surface[x, y].height *= maxHeight;
                surface[x, y].height = Mathf.Floor(surface[x, y].height);
                if (surface[x, y].type == SurfaceTypes.BRIDGE)
                {
                    surface[x, y].height++;
                }
            }
        }

        // surface[Random.Range(0, row), Random.Range(0, col)].mapObjectData = demo();

        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < col; y++)
            {
                Vector3 position = new Vector3(x, surface[x, y].height, y);
                Quaternion rotation = new Quaternion();
                rotation.eulerAngles = new Vector3(0, rotationOffset, 0);
                Transform parent = areaLeader[(int)(x / viewBlockSize), (int)(y / viewBlockSize)].visuableKey.transform;
                
                GameObject faceVoxel;
                GameObject cliffVoxel;
                GameObject hillVoxel;
                GameObject cornerIVoxel;
                GameObject cornerOVoxel;
                GameObject cliffTop;

                switch (surface[x, y].type)
                {
                    case SurfaceTypes.GRASS_BLOCK:
                        faceVoxel = grass_block_top;
                        break;
                    case SurfaceTypes.ROAD:
                        faceVoxel = road_block_top;
                        break;
                    case SurfaceTypes.BRIDGE:
                        faceVoxel = road_block_top;
                        break;
                    case SurfaceTypes.WATER:
                        faceVoxel = water_block_top;
                        break;
                    default:
                        faceVoxel = dust_block_top;
                        break;
                }
                
                SurfaceTypes type = surface[x, y].type;
                int neighborStatus = 0; // �ھӵر�״̬

                int[] dx = { -1, 0, 1, 0, -1, 1, 1, -1 };
                int[] dy = { 0, -1, 0, 1, -1, 1, -1, 1 };
                bool onhill = false;

                for (int i = 7; i >= 0; i--)
                {
                    SurfaceTypes ntype = SurfaceTypes.WATER;
                    int nheight = 0;
                    if (x + dx[i] >= 0 && x + dx[i] < row && y + dy[i] >= 0 && y + dy[i] < col)
                    {
                        ntype = surface[x + dx[i], y + dy[i]].type;
                        nheight = (int)(surface[x + dx[i], y + dy[i]].height);
                    }
                    neighborStatus <<= 1;
                    if (nheight > surface[x, y].height)
                        neighborStatus |= 1;
                }

                /* ����λ�ö�Ӧdx��dy�Ķ����ƿ���
                 * 16  1   128
                 * 2   X   8
                 * 64  4   32
                 */

                if ((neighborStatus & 1) > 0 && (neighborStatus & 2) == 0 && (neighborStatus & 8) == 0)
                {
                    // ����ɽ��
                    rotation.eulerAngles = new Vector3(0, 180 + rotationOffset, 0);
                    GetNiceNeighbor(x + dx[0], y + dy[0], out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                    Instantiate(hillVoxel, position, rotation, parent);
                    onhill = true;
                }
                if ((neighborStatus & 4) > 0 && (neighborStatus & 2) == 0 && (neighborStatus & 8) == 0)
                {
                    // ����ɽ��
                    rotation.eulerAngles = new Vector3(0, 0 + rotationOffset, 0);
                    GetNiceNeighbor(x + dx[2], y + dy[2], out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                    Instantiate(hillVoxel, position, rotation, parent);
                    onhill = true;
                }
                if ((neighborStatus & 8) > 0 && (neighborStatus & 1) == 0 && (neighborStatus & 4) == 0)
                {
                    // ����ɽ��
                    rotation.eulerAngles = new Vector3(0, 270 + rotationOffset, 0);
                    GetNiceNeighbor(x + dx[3], y + dy[3], out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                    Instantiate(hillVoxel, position, rotation, parent);
                    onhill = true;
                }
                if ((neighborStatus & 2) > 0 && (neighborStatus & 1) == 0 && (neighborStatus & 4) == 0)
                {
                    // ����ɽ��
                    rotation.eulerAngles = new Vector3(0, 90 + rotationOffset, 0);
                    GetNiceNeighbor(x + dx[1], y + dy[1], out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                    Instantiate(hillVoxel, position, rotation, parent);
                    onhill = true;
                }
                if ((neighborStatus & 1) == 1 && (neighborStatus & 8) == 8 &&
                    (neighborStatus & 2) == 0 && (neighborStatus & 4) == 0)
                {
                    // �����ڹս�
                    rotation.eulerAngles = new Vector3(0, 180 + rotationOffset, 0);
                    GetNiceNeighbor(x + dx[7], y + dy[7], out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                    Instantiate(cornerIVoxel, position, rotation, parent);
                    onhill = true;
                }
                if ((neighborStatus & 1) == 0 && (neighborStatus & 8) == 8 &&
                    (neighborStatus & 2) == 0 && (neighborStatus & 4) == 4)
                {
                    // �����ڹս�
                    rotation.eulerAngles = new Vector3(0, 270 + rotationOffset, 0);
                    GetNiceNeighbor(x + dx[5], y + dy[5], out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                    Instantiate(cornerIVoxel, position, rotation, parent);
                    onhill = true;
                }
                if ((neighborStatus & 1) == 0 && (neighborStatus & 8) == 0 &&
                    (neighborStatus & 2) == 2 && (neighborStatus & 4) == 4)
                {
                    // �����ڹս�
                    rotation.eulerAngles = new Vector3(0, 0 + rotationOffset, 0);
                    GetNiceNeighbor(x + dx[6], y + dy[6], out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                    Instantiate(cornerIVoxel, position, rotation, parent);
                    onhill = true;
                }
                if ((neighborStatus & 1) == 1 && (neighborStatus & 8) == 0 &&
                    (neighborStatus & 2) == 2 && (neighborStatus & 4) == 0)
                {
                    // �����ڹս�
                    rotation.eulerAngles = new Vector3(0, 90 + rotationOffset, 0);
                    GetNiceNeighbor(x + dx[4], y + dy[4], out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                    Instantiate(cornerIVoxel, position, rotation, parent);
                    onhill = true;
                }
                if (((neighborStatus & 1) == 0 || (neighborStatus & 8) == 0 ||
                    (neighborStatus & 2) == 0 || (neighborStatus & 4) == 0) &&
                    ((neighborStatus & 16) == 16 || (neighborStatus & 128) == 128 ||
                    (neighborStatus & 32) == 32 || (neighborStatus & 64) == 64))
                {
                    // ��ǽ��
                    if ((neighborStatus & 128) == 128 && (neighborStatus & 1) == 0 && (neighborStatus & 8) == 0)
                    {
                        // ������ǽ��
                        rotation.eulerAngles = new Vector3(0, 180 + rotationOffset, 0);
                        GetNiceNeighbor(x + dx[7], y + dy[7], out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                        Instantiate(cornerOVoxel, position, rotation, parent);
                        onhill = true;
                    }
                    if ((neighborStatus & 32) == 32 && (neighborStatus & 4) == 0 && (neighborStatus & 8) == 0)
                    {
                        // ������ǽ��
                        rotation.eulerAngles = new Vector3(0, 270 + rotationOffset, 0);
                        GetNiceNeighbor(x + dx[5], y + dy[5], out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                        Instantiate(cornerOVoxel, position, rotation, parent);
                        onhill = true;
                    }
                    if ((neighborStatus & 64) == 64 && (neighborStatus & 2) == 0 && (neighborStatus & 4) == 0)
                    {
                        // ������ǽ��
                        rotation.eulerAngles = new Vector3(0, 0 + rotationOffset, 0);
                        GetNiceNeighbor(x + dx[6], y + dy[6], out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                        Instantiate(cornerOVoxel, position, rotation, parent);
                        onhill = true;
                    }
                    if ((neighborStatus & 16) == 16 && (neighborStatus & 1) == 0 && (neighborStatus & 2) == 0)
                    {
                        // ������ǽ��
                        rotation.eulerAngles = new Vector3(0, 90 + rotationOffset, 0);
                        GetNiceNeighbor(x + dx[4], y + dy[4], out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                        Instantiate(cornerOVoxel, position, rotation, parent);
                        onhill = true;
                    }
                    
                }
                int badCornerCount = 0; // ���ڼ���޷�������ģ�͹��ɵ�λ��
                if ((neighborStatus & 1) == 1) badCornerCount++;
                if ((neighborStatus & 8) == 8) badCornerCount++;
                if ((neighborStatus & 2) == 2) badCornerCount++;
                if ((neighborStatus & 4) == 4) badCornerCount++;
                if (badCornerCount >= 3)
                {
                    if ((neighborStatus & 4) > 0)
                    {
                        // ������
                        rotation.eulerAngles = new Vector3(0, 0 + rotationOffset, 0);
                        GetNiceNeighbor(x + dx[2], y + dy[2], out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                        Instantiate(hillVoxel, position, rotation, parent);
                        onhill = true;
                    }
                    if ((neighborStatus & 2) > 0)
                    {
                        // ������
                        rotation.eulerAngles = new Vector3(0, 90 + rotationOffset, 0);
                        GetNiceNeighbor(x + dx[1], y + dy[1], out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                        Instantiate(hillVoxel, position, rotation, parent);
                        onhill = true;
                    }
                    if ((neighborStatus & 1) > 0)
                    {
                        // ������
                        rotation.eulerAngles = new Vector3(0, 180 + rotationOffset, 0);
                        GetNiceNeighbor(x + dx[0], y + dy[0], out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                        Instantiate(hillVoxel, position, rotation, parent);
                        onhill = true;
                    }
                    if ((neighborStatus & 8) > 0)
                    {
                        // ������
                        rotation.eulerAngles = new Vector3(0, 270 + rotationOffset, 0);
                        GetNiceNeighbor(x + dx[3], y + dy[3], out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                        Instantiate(hillVoxel, position, rotation, parent);
                        onhill = true;
                    }
                }


                // ��ͨƽ��
                rotation.eulerAngles = new Vector3(0, surface[x, y].placeDirection * 90 + rotationOffset, 0);
                if (surface[x, y].type == SurfaceTypes.ROAD || surface[x, y].type == SurfaceTypes.WATER)
                {
                    // ��ϸ��Ⱦƽ��
                    GetBuiltiFace(surface, x, y, position, parent);
                }
                else
                {
                    Instantiate(faceVoxel, position, rotation, parent);
                }

                int cliff_start = x + 1 < row && surface[x + 1, y].type == SurfaceTypes.WATER ? 0 : 1;
                if (x + 1 < row && surface[x + 1, y].height > surface[x, y].height + cliff_start)
                {
                    GetNiceNeighbor(x + 1, y, out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                    // ��������
                    for (int i = cliff_start; i < surface[x + 1, y].height - surface[x, y].height - 1; i++)
                    {
                        rotation.eulerAngles = new Vector3(0, 0 + rotationOffset, 0);
                        Instantiate(cliffVoxel, position + new Vector3(0, i, 0), rotation, parent);
                    }
                    rotation.eulerAngles = new Vector3(0, 0 + rotationOffset, 0);
                    Instantiate(cliffTop, position + 
                        new Vector3(0, surface[x + 1, y].height - surface[x, y].height - 1, 0), rotation, parent);
                }

                cliff_start = x - 1 >= 0 && surface[x - 1, y].type == SurfaceTypes.WATER ? 0 : 1;
                if (x - 1 >= 0 && surface[x - 1, y].height > surface[x, y].height + cliff_start)
                {
                    GetNiceNeighbor(x - 1, y, out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                    // ���±���
                    for (int i = cliff_start; i < surface[x - 1, y].height - surface[x, y].height - 1; i++)
                    {
                        rotation.eulerAngles = new Vector3(0, 180 + rotationOffset, 0);
                        Instantiate(cliffVoxel, position + new Vector3(0, i, 0), rotation, parent);
                    }
                    rotation.eulerAngles = new Vector3(0, 180 + rotationOffset, 0);
                    Instantiate(cliffTop, position + 
                        new Vector3(0, surface[x - 1, y].height - surface[x, y].height - 1, 0), rotation, parent);
                }

                cliff_start = y + 1 < col && surface[x, y + 1].type == SurfaceTypes.WATER ? 0 : 1;
                if (y + 1 < col && surface[x, y + 1].height > surface[x, y].height + cliff_start)
                {
                    GetNiceNeighbor(x, y + 1, out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                    // ���¶���
                    for (int i = cliff_start; i < surface[x, y + 1].height - surface[x, y].height - 1; i++)
                    {
                        rotation.eulerAngles = new Vector3(0, 270 + rotationOffset, 0);
                        Instantiate(cliffVoxel, position + new Vector3(0, i, 0), rotation, parent);
                    }
                    rotation.eulerAngles = new Vector3(0, 270 + rotationOffset, 0);
                    Instantiate(cliffTop, position + 
                        new Vector3(0, surface[x, y + 1].height - surface[x, y].height - 1, 0), rotation, parent);
                }

                cliff_start = y - 1 >= 0 && surface[x, y - 1].type == SurfaceTypes.WATER ? 0 : 1;
                if (y - 1 >= 0 && surface[x, y - 1].height > surface[x, y].height + cliff_start)
                {
                    GetNiceNeighbor(x, y - 1, out cliffVoxel, out hillVoxel, out cornerIVoxel, out cornerOVoxel, out cliffTop);
                    // ��������
                    for (int i = cliff_start; i < surface[x, y - 1].height - surface[x, y].height - 1; i++)
                    {
                        rotation.eulerAngles = new Vector3(0, 90 + rotationOffset, 0);
                        Instantiate(cliffVoxel, position + new Vector3(0, i, 0), rotation, parent);
                    }
                    rotation.eulerAngles = new Vector3(0, 90 + rotationOffset, 0);
                    Instantiate(cliffTop, position + 
                        new Vector3(0, surface[x, y - 1].height - surface[x, y].height - 1, 0), rotation, parent);
                }

                if (surface[x, y].mapObjectData != null)
                {
                    RenderMapObject(surface[x, y].mapObjectData, position, gameObject.transform, onhill);
                }
            }
        }

        
        for (int i = 0; i < areaLeader.GetLength(0); i++)
        {
            for (int j = 0; j < areaLeader.GetLength(1); j++)
            {
                areaLeader[i, j].merge();
            }
        }
    }
}
