#ifndef OBSTACLE_AVOIDANCE_H
#define OBSTACLE_AVOIDANCE_H

#include "ros/ros.h"
#include "nav_msgs/OccupancyGrid.h"
#include <vector>
#include <cstdlib>
#include <ctime>
#include <cmath>
#include <algorithm>

using namespace std;

// �|�C���g
struct CPoint
{
  int x;
  int y;

  // �R���X�g���N�^
  CPoint(void)
  : x(0)
  , y(0)
  {
  }
  
  // �R���X�g���N�^ (�n�_�A�I�_)
  CPoint(int x_, int y_)
  : x(x_)
  , y(y_)
  {
  }
};

// ��`
struct CRect
{
  int left;
  int top;
  int right;
  int bottom;

  // �R���X�g���N�^
  CRect(void)
  : left(0)
  , top(0)
  , right(0)
  , bottom(0)
  {
  }
  
  // �R���X�g���N�^ (�n�_�A�I�_)
  CRect(int left_, int top_, int right_, int bottom_)
  : left(left_)
  , top(top_)
  , right(right_)
  , bottom(bottom_)
  {
  }
};

// ��|�C���g��`
struct stBasicPoints
{
  CPoint start_point;
  CPoint end_point;

  // �R���X�g���N�^
  stBasicPoints(void)
  : start_point(0, 0)
  , end_point(0, 0)
  {
  }
  
  // �R���X�g���N�^ (�n�_�A�I�_)
  stBasicPoints(CPoint start_point, CPoint end_point)
  : start_point(start_point)
  , end_point(end_point)
  {
  }
  
  bool IsEmpty(void)
  {
    if (start_point.x == 0 && start_point.y == 0 && end_point.x == 0 && end_point.y == 0) return true;
    return false;
  }

  void Clear(void)
  {
    start_point.x = 0;
    start_point.y = 0;
    end_point.x = 0;
    end_point.y = 0;
  }
};

// ��Q���|�C���g������`
struct stObstaclePointsDis
{
  int distance;
  int angle;
  CPoint point;
  
  // �R���X�g���N�^
  stObstaclePointsDis(void)
  : distance(0)
  , angle(0)
  , point(0, 0)
  {
  }
  
  // �R���X�g���N�^ (�����A�p�x�A�ʒu)
  stObstaclePointsDis(int distance, int angle, CPoint point)
  : distance(distance)
  , angle(angle)
  , point(point)
  {
  }
  
  bool operator < (const stObstaclePointsDis& right) const
  {
    return distance == right.distance ? angle < right.angle : distance < right.distance;
  }
};

// ��Q���|�C���g�p�x��`
struct stObstaclePointsAng
{
  int angle;
  int distance;
  CPoint point;
  
  // �R���X�g���N�^
  stObstaclePointsAng(void)
  : angle(0)
  , distance(0)
  , point(0, 0)
  {
  }
  
  // �R���X�g���N�^ (�p�x�A�����A�ʒu)
  stObstaclePointsAng(int angle, int distance, CPoint point)
  : angle(angle)
  , distance(distance)
  , point(point)
  {
  }
  
  bool IsEmpty(void)
  {
    if (angle == 0 && distance == 0 && point.x == 0 && point.y == 0) return true;
    return false;
  }
  
  void Clear(void)
  {
    angle = 0;
    distance = 0;
    point.x = 0;
    point.y = 0;
  }
  
  bool operator < (const stObstaclePointsAng& right) const
  {
    return angle == right.angle ? distance < right.distance : angle < right.angle;
  }
};

// ��Q���\����`
struct stObstacleStruct
{
  stBasicPoints width_struct;
  stBasicPoints height_struct;
  
  // �R���X�g���N�^
  stObstacleStruct(void)
  : width_struct()
  , height_struct()
  {
  }
  
  // �R���X�g���N�^ (���\���A�c�\��)
  stObstacleStruct(stBasicPoints width_struct, stBasicPoints height_struct)
  : width_struct(width_struct)
  , height_struct(height_struct)
  {
  }
  
  bool IsEmpty(void)
  {
    if (width_struct.IsEmpty() && height_struct.IsEmpty()) return true;
    return false;
  }
  
  void Clear(void)
  {
    width_struct.Clear();
    height_struct.Clear();
  }
};

// Ccalculation �_�C�A���O
class Ccalculation
{
  public:
    // �n�_�����Ɏw�苗���A�w��p�x�ŉ�]���������W�����߂�
    void CalcLineRotatePos(float sx, float sy, float ex, float ey, float r, float theta, float& x, float& y);
    
    // �n�_�����Ɏw�苗���A�w��p�x�ŉ�]���������W�����߂�B
    void CalcLineRotatePosRad(float sx, float sy, float ex, float ey, float r, float rad, float& x, float& y);
    
    // �����̕����������߂�
    bool CalcLineEquation(float sx, float sy, float ex, float ey, float& a, float& b);
    
    // 2�����̌�_�����߂�B
    bool GetLineClossPoint(int sx1, int sy1, int ex1, int ey1, int sx2, int sy2, int ex2, int ey2, int& rx, int& ry);
    
    // 2�_�Ԃ̋��������߂�
    int GetDistance(int sx, int sy, int ex, int ey);

    // �_�ƒ����̋������擾����
    double GetPointToLineDistace(CPoint& point, CPoint& sp, CPoint& ep);
};

// CobstacleAvoidance �_�C�A���O
class CobstacleAvoidance
{
  public:
    Ccalculation m_calculation;            // �v�Z�p
    vector< std::vector<int> > m_map_data; // �n�}�f�[�^
    stBasicPoints m_road_destination;      // �o�H�n�I�_�̊�|�C���g
    vector<CPoint> m_mid_point_list;       // �r���|�C���g���X�g
    vector<stObstaclePointsDis> m_obstacle_points_list;  // ��Q���̓_�Q���X�g
    vector<stObstacleStruct> m_obstacle_struct_list;     // ��Q���̍\�����X�g
    vector<CPoint> m_best_road_point;                    // �œK�o�H

    // �R���X�g���N�V����
    CobstacleAvoidance();

  protected:
    // �n�}�f�[�^���擾
    void mapCallback(const nav_msgs::OccupancyGrid::ConstPtr& map);

    // �o�H�n�I�_���擾
    //void roadDestinationCallback(const road_destination);

    // ��Q�������擾
    //void obstaclePointsCallback(const obstacle_points);

    // �T���w�����擾
    //void scanCallback(const scan);

  private:
    bool m_start_line_closs;
    int m_map_height;         // �n�}�̍���
    int m_map_width;          // �n�}�̕�
    int m_best_road_distance; // �ŒZ����
    
    // ������
    void init(void);
    
    // �e��Q���̍\�����Z�o
    void GetObstacleStruct(void);
    
    // �r���_���X�g���Z�o
    void GetMidPointList(void);
    
    // ��Q������œK���[�g�T��
    void FindBestRoad(void);

   // ���r�_�ɂ��o�H��T��
    void FindRoadByPoints(int num_points);
    
    // ���r�_�ɂ��o�H�̑S�T��
    void FindAllRoad(int num_points, std::vector< std::vector<int> >& all_road_arrangement);

    // ���r�_�ɂ��o�H�̎�ނ𐶐�
    void Portfolio(std::vector<int>& array, std::vector< std::vector<int> >& all_road_portfolio, int start, int end, int num_points, std::vector<int>& sel_array, int nsel_temp);

    // �o�H�̎�ނɂ��o�H�̏��Ԃ𐶐�
    void Arrangement(std::vector<int>& portfolio_list, std::vector< std::vector<int> >& all_road_arrangement, int start, int end);
};

#endif // OBSTACLE_AVOIDANCE_H
