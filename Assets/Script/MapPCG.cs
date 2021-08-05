using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

namespace WorldGennerator
{
    public class Util
    {
        // ���������
        static Point GennerateRandomPoint(int row, int col)
        {
            Point p = new Point();
            p.x = rd.Next(0, row);
            p.y = rd.Next(0, col);
            return p;
        }

        // �������
        static int CalcuDistance(Point p1, Point p2)
        {
            int xdiff = p1.x - p2.x;
            int ydiff = p1.y - p2.y;
            return (int)Math.Sqrt(Math.Abs(xdiff * xdiff + ydiff * ydiff));
        }

        // ������
        static Point[] SamplePoints(int pointCnt, int row, int col, double disRatio)
        {
            int minDistance = (int)(disRatio * row);
            Point[] genneratedPoints = new Point[pointCnt];
            for (int i = 0; i < pointCnt; ++i)
            {
                while (true)
                {
                    Point p = GennerateRandomPoint(row, col);
                    bool farEnough = true;
                    for (int j = 0; j < i; ++j)
                    {
                        if (CalcuDistance(p, genneratedPoints[j]) < minDistance)
                        {
                            farEnough = false;
                            break;
                        }
                    }
                    if (farEnough)
                    {
                        genneratedPoints[i] = p;
                        break;
                    }
                }
            }
            return genneratedPoints;
        }

        static bool PointSatisfy(BaseModule[,] map, Point p, int condition)
        {
            bool flag = true;
            switch (condition)
            {
                case 0:  // ½�����������ж�
                    flag = (map[p.x, p.y].type == -1);
                    break;
                case 1:  // �������������ж�
                    flag = map[p.x, p.y].islandCnt < 0;
                    break;
                case 2:  // �������������ж�
                    flag = map[p.x, p.y].type != 0;
                    break;
                default:
                    break;
            }
            return flag;
        }

        static List<List<Point>> GennerateSeeds(BaseModule[,] coastLine, BaseModule[,] map, int seedNum, int row, int col, int type, int condition)
        {
            Point[] genneratedPoints;
            while (true)
            {
                genneratedPoints = SamplePoints(seedNum, row, col, 0.25);
                bool flag = true;
                foreach (Point p in genneratedPoints)
                {
                    if (condition == 0 && PointSatisfy(coastLine, p, condition))
                    {
                        flag = false;
                    }
                    if (condition != 0 && PointSatisfy(map, p, condition))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    break;
                }
            }
            List<List<Point>> seedPoints = new List<List<Point>>();
            for (int i = 0; i < seedNum; ++i)
            {
                seedPoints.Add(new List<Point>());
                Point p = genneratedPoints[i];
                seedPoints[i].Add(p);
                map[p.x, p.y].type = type;
                map[p.x, p.y].typeNo = i;
                if (type == 0)
                {
                    map[p.x, p.y].islandCnt = i;
                }
            }
            return seedPoints;
        }

        // ��������
        static bool SatisfyCondition(BaseModule[,] coastLine, BaseModule[,] map, int x, int y, int condition)
        {
            bool flag = true;
            switch (condition)
            {
                case -1:  // �ɰ������ж�
                    flag = map[x, y].islandCnt == -1;
                    break;
                case 0:  // ½�������ж�
                    flag = (map[x, y].type == -1 && coastLine[x, y].type != -1);
                    break;
                case 1:  // ���������ж�
                    flag = map[x, y].type == 0;
                    break;
                case 2:  // ���������ж�
                    flag = map[x, y].type == 0;
                    break;
                default:
                    break;
            }
            return flag;
        }

        static int[,] GrowSeed(double landScale, int seedNum, double growLandScale,
                                List<List<Point>> seedPoints, BaseModule[,] coastLine,
                                BaseModule[,] map, int row, int col, int type, int condition)
        {
            int[] dx = new int[] { -1, 1, 0, 0 };
            int[] dy = new int[] { 0, 0, -1, 1 };
            double growScale = landScale / seedNum * growLandScale;
            int growCnt = (int)(growScale * row * col);
            int[,] growsXY = new int[4, seedNum];
            for (int i = 0; i < seedNum; ++i)
            {
                growsXY[0, i] = row - 1;
                growsXY[1, i] = col - 1;
                growsXY[2, i] = 0;
                growsXY[3, i] = 0;

            }
            while (growCnt > 0)
            {
                for (int i = 0; i < seedNum; ++i)
                {
                    while (seedPoints[i].Count != 0)
                    {
                        int pickPointIndex = rd.Next(0, seedPoints[i].Count);
                        Point p = seedPoints[i][pickPointIndex];
                        List<int> possibleGrowDir = new List<int>();
                        for (int t = 0; t < 4; ++t)
                        {
                            int x = p.x + dx[t];
                            int y = p.y + dy[t];
                            if (InMap(x, y, row, col) && SatisfyCondition(coastLine, map, x, y, condition))
                            {
                                possibleGrowDir.Add(t);
                            }
                        }
                        if (possibleGrowDir.Count == 0)
                        {
                            seedPoints[i].RemoveAt(pickPointIndex);
                            continue;
                        }
                        if (possibleGrowDir.Count == 1)
                        {
                            seedPoints[i].RemoveAt(pickPointIndex);
                        }
                        int nextPointIndex = rd.Next(0, possibleGrowDir.Count);
                        Point nextPoint = new Point(p.x + dx[possibleGrowDir[nextPointIndex]], p.y + dy[possibleGrowDir[nextPointIndex]]);
                        if (nextPoint.x < growsXY[0, i])
                        {
                            growsXY[0, i] = nextPoint.x;
                        }
                        if (nextPoint.y < growsXY[1, i])
                        {
                            growsXY[1, i] = nextPoint.y;
                        }
                        if (nextPoint.x > growsXY[2, i])
                        {
                            growsXY[2, i] = nextPoint.x;
                        }
                        if (nextPoint.y > growsXY[3, i])
                        {
                            growsXY[3, i] = nextPoint.y;
                        }
                        seedPoints[i].Add(nextPoint);
                        map[nextPoint.x, nextPoint.y].type = type;
                        map[nextPoint.x, nextPoint.y].typeNo = i;
                        if (type == 0)
                        {
                            map[nextPoint.x, nextPoint.y].islandCnt = i;
                        }
                        break;
                    }
                }
                growCnt--;
                bool needEnd = true;
                for (int i = 0; i < seedNum; ++i)
                {
                    if (seedPoints[i].Count != 0)
                    {
                        needEnd = false;
                    }
                }
                if (needEnd)
                {
                    break;
                }
            }
            return growsXY;
        }

        // ���ӳ��� pos: 0: �������� 1: ��������
        static void LinkRow(BaseModule[,] map, int xMin, int xMax, int y)
        {
            for (int i = xMin; i <= xMax; ++i)
            {
                map[i, y].surface = 0;
            }
        }

        static void LinkCol(BaseModule[,] map, int yMin, int yMax, int x)
        {
            for (int i = yMin; i <= yMax; ++i)
            {
                map[x, i].surface = 0;
            }
        }

        static void LinkPoint(BaseModule[,] map, int row, int col, BaseModule a, BaseModule b)
        {
            int xMin = Math.Min(a.x, b.x);
            int yMin = Math.Min(a.y, b.y);
            int xMax = Math.Max(a.x, b.x);
            int yMax = Math.Max(a.y, b.y);
            int pos;
            if ((xMin == a.x && yMin == a.y) || (xMin == b.x && yMin == b.y))
            {
                pos = 0; // ��������
            }
            else
            {
                pos = 1; // ��������
            }
            if (pos == 0) // ��������
            {
                if (rd.NextDouble() < 0.5) // ���²������ӵ�
                {
                    LinkRow(map, xMin, xMax, yMin);
                    LinkCol(map, yMin, yMax, xMax);
                }
                else // ���ϲ������ӵ�
                {
                    LinkCol(map, yMin, yMax, xMin);
                    LinkRow(map, xMin, xMax, yMax);
                }
            }
            else // ��������
            {
                if (rd.NextDouble() < 0.5) // ���ϲ������ӵ�
                {
                    LinkRow(map, xMin, xMax, yMin);
                    LinkCol(map, yMin, yMax, xMin);
                }
                else // ���ϲ������ӵ�
                {
                    LinkCol(map, yMin, yMax, xMin);
                    LinkRow(map, xMin, xMax, yMin);
                }
            }
        }

        static void LinkTown(BaseModule[,] map, int row, int col, int townNum)
        {
            Queue<BaseModule> Q = new Queue<BaseModule>();
            Hashtable ht = new Hashtable();
            BaseModule root = new BaseModule();
            bool[,] visited = new bool[row, col];
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    visited[i, j] = false;
                    if (map[i, j].surface == 0)
                    {
                        Q.Enqueue(map[i, j]);
                        ht.Add(map[i, j], root);
                        visited[i, j] = true;
                    }
                }
            }
            int[,] touched = new int[townNum, townNum];
            BaseModule[,] nearestRoadPoint = new BaseModule[townNum, townNum];
            for (int i = 0; i < townNum; ++i)
            {
                for (int j = 0; j < townNum; ++j)
                {
                    touched[i, j] = 0;
                    nearestRoadPoint[i, j] = new BaseModule();
                }
            }
            int[] dx = new int[4] { -1, 1, 0, 0 };
            int[] dy = new int[4] { 0, 0, -1, 1 };
            while (Q.Count != 0)
            {
                BaseModule module = Q.Dequeue();
                for (int i = 0; i < 4; ++i)
                {
                    int x = module.x + dx[i];
                    int y = module.y + dy[i];
                    if (InMap(x, y, row, col))
                    {
                        BaseModule tmp = map[x, y];
                        if (visited[x, y])
                        {
                            if (module.typeNo != tmp.typeNo && touched[module.typeNo, tmp.typeNo] == 0)
                            {
                                touched[module.typeNo, tmp.typeNo] = 1;
                                touched[tmp.typeNo, module.typeNo] = 1;
                                while (ht[module] != root)
                                {
                                    module = (BaseModule)ht[module];
                                }
                                while (ht[tmp] != root)
                                {
                                    tmp = (BaseModule)ht[tmp];
                                }
                                nearestRoadPoint[module.typeNo, tmp.typeNo] = module;
                                nearestRoadPoint[tmp.typeNo, module.typeNo] = tmp;
                            }
                        }
                        else
                        {
                            tmp.typeNo = module.typeNo;
                            Q.Enqueue(tmp);
                            visited[x, y] = true;
                            ht.Add(tmp, module);
                        }
                    }
                }
            }
            for (int i = 0; i < townNum; ++i)
            {
                for (int j = i + 1; j < townNum; ++j)
                {
                    if (touched[i, j] == 1)
                    {
                        LinkPoint(map, row, col, nearestRoadPoint[i, j], nearestRoadPoint[j, i]);
                    }
                }
            }
        }

        // ��ӡ��ͼ
        static void PrintMap(BaseModule[,] map, int row, int col, bool showIslandCnt)
        {
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    int type = map[i, j].type;
                    int surface = map[i, j].surface;
                    if (surface != -1)
                    {
                        switch (surface)
                        {
                            case 0:  // ��·
                                Console.Write('#');
                                break;
                            case 1:  // ����
                                Console.Write('R');
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (type)
                        {
                            case -1:
                                // ����
                                Console.Write('~');
                                break;
                            case 0:
                                // ����
                                if (showIslandCnt)
                                {
                                    Console.Write(map[i, j].islandCnt);
                                }
                                else
                                {
                                    Console.Write('��');
                                }
                                break;
                            case 1:
                                // ����
                                Console.Write('^');
                                break;
                            case 2:
                                // ����
                                Console.Write('T');
                                break;
                            default:
                                break;
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        // ��ӡ��ͼ����
        static void PrintMapType(BaseModule[,] map, int row, int col)
        {
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    Console.Write(map[i, j].type);
                }
                Console.WriteLine();
            }
        }

        // ��ӡ��ͼ����
        static void PrintMapSurface(BaseModule[,] map, int row, int col)
        {
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    Console.Write(map[i, j].surface);
                }
                Console.WriteLine();
            }
        }

        // ��ӡ�߶�ͼ
        static void PrintHeightMap(BaseModule[,] map, int row, int col, double scale = 1)
        {
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    int height = (int)(map[i, j].height * 10 * scale);
                    if (height <= 0)
                    {
                        if (map[i, j].type == -1)
                        {
                            Console.Write('~');
                        }
                        else
                        {
                            Console.Write('��');
                        }
                    }
                    else
                    {
                        Console.Write(height);
                    }
                }
                Console.WriteLine();
            }
        }

        // ���߶�
        static void FulfillHeight(BaseModule[,] map, int row, int col)
        {
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    if (map[i, j].height < 0)
                    {
                        map[i, j].height = 0;
                    }
                }
            }
        }

        // �ϲ��߶�
        static void MergeHeight(BaseModule[,] coastline, BaseModule[,] map, int row, int col)
        {
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    map[i, j].height += coastline[i, j].height;
                }
            }
        }

        // ������Ⱦ����
        static MapSurfaceData[,] RenderMap(BaseModule[,] map, int row, int col)
        {
            MapSurfaceData[,] renderData = new MapSurfaceData[row, col];
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    renderData[i, j] = new MapSurfaceData();
                    renderData[i, j].height = (float)map[i, j].height;
                    renderData[i, j].mapObjectData = map[i, j].obj;
                    int surface = map[i, j].surface;
                    int type = map[i, j].type;
                    if (surface != -1)
                    {
                        MapObjectData mj = new MapObjectData();
                        switch (surface)
                        {
                            case 0:  // ��·
                                if (type == -1)  // ˮ�ϵĵ�·����
                                {
                                    renderData[i, j].type = SurfaceTypes.BRIDGE;
                                }
                                else  // �����ĵ�·�ǵ�·
                                {
                                    renderData[i, j].type = SurfaceTypes.ROAD;
                                }
                                break;
                            case 1:  // ����
                                renderData[i, j].type = SurfaceTypes.GRASS_BLOCK;
                                break;
                            case 2:  // ��ľ
                                mj.voxel_types = new VoxelType[1, 1, 1];
                                mj.voxel_types[0, 0, 0] = VoxelType.TREE;
                                mj.voxel_direction = new int[1, 1, 1];
                                mj.voxel_direction[0, 0, 0] = 0;
                                mj.direction = (float)(rd.NextDouble() * 360);
                                mj.scale = new float[] { 1, 1, 1 };
                                renderData[i, j].mapObjectData = mj;
                                break;
                            case 3:  // ��
                                mj.voxel_types = new VoxelType[1, 1, 1];
                                mj.voxel_types[0, 0, 0] = VoxelType.GRASS;
                                mj.voxel_direction = new int[1, 1, 1];
                                mj.voxel_direction[0, 0, 0] = 0;
                                mj.direction = (float)(rd.NextDouble() * 360);
                                mj.scale = new float[] { 1, 1, 1 };
                                renderData[i, j].mapObjectData = mj;
                                break;
                            case 4:  // ʯͷ
                                mj.voxel_types = new VoxelType[1, 1, 1];
                                mj.voxel_types[0, 0, 0] = VoxelType.ROCK;
                                mj.voxel_direction = new int[1, 1, 1];
                                mj.voxel_direction[0, 0, 0] = 0;
                                mj.direction = (float)(rd.NextDouble() * 360);
                                mj.scale = new float[] { 1, 1, 1 };
                                renderData[i, j].mapObjectData = mj;
                                break;
                            default:
                                renderData[i, j].type = SurfaceTypes.ROAD;
                                break;
                        }
                    }
                    else
                    {
                        switch (type)
                        {
                            case -1: // ˮ��
                                renderData[i, j].type = SurfaceTypes.WATER;
                                break;
                            case 0: // ½��
                                renderData[i, j].type = SurfaceTypes.GRASS_BLOCK;
                                break;
                            case 1: // ����
                                renderData[i, j].type = SurfaceTypes.DUST;
                                break;
                            case 2: // ����
                                renderData[i, j].type = SurfaceTypes.GRASS_BLOCK;
                                break;
                            default:
                                renderData[i, j].type = SurfaceTypes.ROAD;
                                break;
                        }
                    }
                }
            }
            return renderData;
        }

        // �Ƿ���map��
        static bool InMap(int i, int j, int row, int col)
        {
            return i >= 0 && i < row && j >= 0 && j < col;
        }

        static void ReWriteType(BaseModule[,] map, int row, int col)
        {
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    if (map[i, j].islandCnt >= 0 && map[i, j].type < 0)
                    {
                        map[i, j].type = 0;
                    }
                }
            }

        }

        // ���ɺ���
        static BaseModule[,] GenerateRiver(BaseModule[,] map, int row, int col, int hillNum, int islandnNum)
        {

            //ReWriteType(map, row, col);

            int[,] dir = { { -1, -1 }, { -1, 1 }, { 1, 1 }, { 1, -1 }, { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
            //����͵����Ƿ��ڽ�
            bool[,] hill2Island = new bool[hillNum, islandnNum];

            // ��һ�飬����֮�����ɺ���;˳���ж�����͵����Ƿ��ڽ�
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    BaseModule mapOrigin = map[i, j];
                    if (mapOrigin.type != 0)
                    {
                        // �ǵ���
                        continue;
                    }
                    for (int k = 0; k < 8; ++k)
                    {
                        int ti = i + dir[k, 0];
                        int tj = j + dir[k, 1];
                        if (InMap(ti, tj, row, col))
                        {
                            BaseModule mapTarge = map[ti, tj];
                            if (mapTarge.type != 0)
                            {
                                if (mapTarge.type == 1)
                                {//����
                                    hill2Island[mapTarge.typeNo, mapOrigin.islandCnt] = true;
                                }
                                //�ǵ���
                                continue;
                            }
                            if (mapOrigin.islandCnt != mapTarge.islandCnt)
                            {
                                // ���߶��ǵ��죬�ұ�Ų�ͬ�������߱�Ϊˮ��
                                mapOrigin.type = -1;
                                mapOrigin.islandCnt = -1;
                                mapTarge.type = -1;
                                mapTarge.islandCnt = -1;
                                break;
                            }
                        }
                    }
                }
            }

            // ��ʼȫΪ0
            int[] selectedIsland = new int[hillNum];

            // Ϊÿ������ѡ��һ�����죬�������ĵ���
            for (int i = 0; i < hillNum; ++i)
            {
                for (int j = 0; j < islandnNum; ++j)
                {
                    if (hill2Island[i, j])
                    {
                        if (selectedIsland[i] == 0)
                        {
                            //��һ�θ�ֵΪ-1��Ϊ�˱�������ֻ��һ���������
                            selectedIsland[i] = -1;
                        }
                        else
                        {
                            //�ڶ���������ֵ
                            selectedIsland[i] = j;
                        }
                    }
                }
            }

            // �ڶ��飬������ڽӵĵ���֮����ѡһ�����ɺ���
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    BaseModule mapOrigin = map[i, j];
                    if (mapOrigin.type != 1)
                    {
                        //������
                        continue;
                    }
                    for (int k = 0; k < 8; ++k)
                    {
                        int ti = i + dir[k, 0];
                        int tj = j + dir[k, 1];
                        if (InMap(ti, tj, row, col))
                        {
                            BaseModule mapTarge = map[ti, tj];
                            if (mapTarge.type != 0)
                            {
                                //�ǵ���
                                continue;
                            }
                            if (selectedIsland[mapOrigin.typeNo] == mapTarge.islandCnt)
                            {
                                // mapOrigin�����꣬mapTarge�Ǳ�ѡ�еĵ��죬�����߱�Ϊ����
                                mapOrigin.type = -1;
                                mapOrigin.islandCnt = -1;
                                mapTarge.type = -1;
                                mapTarge.islandCnt = -1;
                                break;
                            }
                        }
                    }
                }
            }

            return map;
        }

        static Random rd = new Random();

        // ���ɵ�ͼ
        static public MapSurfaceData[,] GennerateMap(int row, int col, double mapSize, int islandnNum, bool inUnity)
        {
            int rowAdd = 0;
            int colAdd = 0;
            row += (int)(rowAdd * mapSize);
            col += (int)(colAdd * mapSize);
            BaseModule[,] map = new BaseModule[row, col];
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    map[i, j] = new BaseModule();
                    map[i, j].x = i;
                    map[i, j].y = j;
                }
            }

            // �����ɰ��ͼ��ȷ�������ߺ͵��θ߶�
            Console.WriteLine(">>>>>>>>>>Gennerate coastline<<<<<<<<<<");
            double landScale = 0.7;
            Point root = new Point(row / 2, col / 2);
            List<List<Point>> terrianPoints = new List<List<Point>>();
            terrianPoints.Add(new List<Point>());
            terrianPoints[0].Add(root);
            BaseModule[,] coastLine = new BaseModule[row, col];
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    coastLine[i, j] = new BaseModule();
                }
            }
            coastLine[row / 2, col / 2].islandCnt = 0;
            coastLine[row / 2, col / 2].type = 0;

            int[,] coastXY = GrowSeed(landScale, 1, 1.0, terrianPoints, coastLine, coastLine, row, col, 0, -1);

            if (!inUnity)
            {
                PrintMap(coastLine, row, col, false);
            }

            // �ɰ��ͼ���ɸ߶�ͼ
            Console.WriteLine(">>>>>>>>>>Gennerate coastline heightmap<<<<<<<<<<");
            int mapHeightScale = (int)(row / 5.0);
            MapPCG.HillsPCG.GenerateHillsMap(coastLine, coastXY[0, 0], coastXY[1, 0], coastXY[2, 0], coastXY[3, 0], mapHeightScale, 0.2, 0);
            if (!inUnity)
            {
                PrintHeightMap(coastLine, row, col, 1);
            }

            // ���ɵ������ӣ����ӱ������ں������е�½������
            Console.WriteLine(">>>>>>>>>>Choose island seeds<<<<<<<<<<");
            List<List<Point>> islandPoints = GennerateSeeds(coastLine, map, islandnNum, row, col, 0, 0);
            if (!inUnity)
            {
                PrintMap(map, row, col, true);
            }

            // �������ӳɳ�Ϊ����
            Console.WriteLine(">>>>>>>>>>Grow island seeds<<<<<<<<<<");
            GrowSeed(landScale, islandnNum, 1.0, islandPoints, coastLine, map, row, col, 0, 0);
            if (!inUnity)
            {
                PrintMap(map, row, col, true);
            }

            // ������������
            Console.WriteLine(">>>>>>>>>>Choose hill seeds<<<<<<<<<<");
            int hillNum = islandnNum;
            List<List<Point>> hillPoints = GennerateSeeds(coastLine, map, hillNum, row, col, 1, 1);

            // �������ӳɳ�Ϊ����
            Console.WriteLine(">>>>>>>>>>Grow hill seeds<<<<<<<<<<");
            int[,] hillsXY = GrowSeed(landScale, hillNum, 0.4, hillPoints, coastLine, map, row, col, 1, 1);
            if (!inUnity)
            {
                PrintMap(map, row, col, false);
            }

            // �����������
            Console.WriteLine(">>>>>>>>>>Gennerate exact hills<<<<<<<<<<");
            for (int i = 0; i < hillNum; ++i)
            {
                MapPCG.HillsPCG.GenerateHillsMap(map, hillsXY[0, i], hillsXY[1, i], hillsXY[2, i], hillsXY[3, i], row / 10, 1.0, 1);
            }
            if (!inUnity)
            {
                PrintHeightMap(map, row, col);
            }

            // ���ɺ���
            Console.WriteLine(">>>>>>>>>>Produce rivers<<<<<<<<<<");
            GenerateRiver(map, row, col, hillNum, islandnNum);
            if (!inUnity)
            {
                PrintMap(map, row, col, false);
            }

            // �ϲ��߶�
            Console.WriteLine(">>>>>>>>>>Merge height map<<<<<<<<<<");
            FulfillHeight(map, row, col);
            FulfillHeight(coastLine, row, col);
            MergeHeight(coastLine, map, row, col);
            if (!inUnity)
            {
                PrintHeightMap(map, row, col, 0.9);
            }

            // ���ɳ�������
            Console.WriteLine(">>>>>>>>>>Produce town seeds<<<<<<<<<<");
            int townNum = islandnNum;
            List<List<Point>> townPoints = GennerateSeeds(coastLine, map, townNum, row, col, 2, 2);

            // �������ӳɳ�Ϊ����
            Console.WriteLine(">>>>>>>>>>Grow town seeds<<<<<<<<<<");
            int[,] townsXY = GrowSeed(landScale, townNum, 0.3, townPoints, coastLine, map, row, col, 2, 2);
            if (!inUnity)
            {
                PrintMap(map, row, col, false);
            }

            // ���ɳ����Ų�
            Console.WriteLine(">>>>>>>>>>Gennerate exact towns<<<<<<<<<<");
            for (int i = 0; i < townNum; ++i)
            {
                List<MapPCG.Building> buildings = MapPCG.TownPCG.GenerateTownMap(map, townsXY[0, i], townsXY[1, i], townsXY[2, i], townsXY[3, i], 4, i);
            }
            if (!inUnity)
            {
                PrintMap(map, row, col, false);
            }

            // ��·����
            LinkTown(map, row, col, townNum);
            if (!inUnity)
            {
                PrintMap(map, row, col, false);
            }

            // ��ľ���ݡ�ʯͷ�������
            Console.WriteLine(">>>>>>>>>>Gennerate tree grass rock<<<<<<<<<<");
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    BaseModule module = map[i, j];
                    if (module.surface != -1)
                    {
                        continue;
                    }
                    if (module.type != -1)
                    {
                        double rand = rd.NextDouble();
                        if (rand <= 0.05)
                        {
                            if (module.type != 1)
                            {
                                module.surface = 2; // ��ľ
                            }
                        }
                        if (rand >= 0.2 && rand <= 0.25)
                        {
                            module.surface = 4; // ʯͷ
                        }
                        if (rand >= 0.4 && rand <= 0.8)
                        {
                            module.surface = 3; // ��
                        }
                    }
                }
            }

            // ת����Ⱦ����
            MapSurfaceData[,] renderData = RenderMap(map, row, col);

            return renderData;
        }

        static void Main()
        {
            MapSurfaceData[,] renderMap = GennerateMap(200, 200, 0.1, 3, false);
        }
    }
}
