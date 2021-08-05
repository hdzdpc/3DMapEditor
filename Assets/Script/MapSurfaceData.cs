using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SurfaceTypes { GRASS_BLOCK, WATER, DUST, ROAD, BRIDGE };
public class MapSurfaceData
{
    public float height; // �ر�߶�
    public SurfaceTypes type; // �ر�����
    public int placeDirection = 0; // �ر���0 - 0deg��1 - 90deg��2 - 180deg��3 - 270deg��ͨ��Ĭ��0����
    public MapObjectData mapObjectData = null; // �������壬�������ɵر�������ľ��ռ��������Դ���һ������һ����������
}
