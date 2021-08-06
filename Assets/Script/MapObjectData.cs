
public enum VoxelType { WALL, DOOR, WINDOW, FLOOR, ROOF, TREE, GRASS, ROCK }

public enum FurType { Empty, DoubleBed, SmallContainer, Sofa, Table, TV, DiningTable, Toilet, Washer, Frozer, Cooker }
public class MapObjectData
{
    public VoxelType[,,] voxel_types; // ���ɸ����壨��������ľ������������
    public int[,,] voxel_direction; // ���ط���
    public FurType[,,] fur_types;
    public int[,,] fur_direction;
    public float direction = 0; // ���巽������Ƕȣ���λ����
    public float[] scale = { 1, 1, 1 }; // �������ţ����ؿռ䣨x��y��z����������
    public int[] staircase;
}

public class BaseModule
{
    public int x;
    public int y;
    public double height; // �߶�
    public int islandCnt; //������
    public int type; // -1:�� 0:½�� 1:���� 2:���� 
    public int typeNo; // �������ݱ��
    public int surface; // -1:�� 0:��· 1:���� 2:ֲ�� 3:ɭ��
    public MapObjectData obj = null;

    public BaseModule()
    {
        this.x = -1;
        this.y = -1;
        this.height = -1.0;
        this.islandCnt = -1;
        this.type = -1;
        this.typeNo = -1;
        this.surface = -1;
        this.obj = null;
    }
}

public class Point
{
    public int x;
    public int y;
    public Point() { }
    public Point(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
    public static Point operator +(Point a, Point b)
    {
        return new Point(a.x + b.x, a.y + b.y);
    }
    public static Point operator -(Point a, Point b)
    {
        return new Point(a.x - b.x, a.y - b.y);
    }
    public static int operator *(Point a, Point b)
    {
        return a.x * b.x + a.y * b.y;
    }
    public static int operator /(Point a, Point b)
    {
        return a.x * b.y - a.y * b.x;
    }
    public double abs()
    {
        return System.Math.Sqrt(x * x + y * y);
    }
}