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

// ポイント
struct CPoint
{
  int x;
  int y;

  // コンストラクタ
  CPoint(void)
  : x(0)
  , y(0)
  {
  }
  
  // コンストラクタ (始点、終点)
  CPoint(int x_, int y_)
  : x(x_)
  , y(y_)
  {
  }
};

// 矩形
struct CRect
{
  int left;
  int top;
  int right;
  int bottom;

  // コンストラクタ
  CRect(void)
  : left(0)
  , top(0)
  , right(0)
  , bottom(0)
  {
  }
  
  // コンストラクタ (始点、終点)
  CRect(int left_, int top_, int right_, int bottom_)
  : left(left_)
  , top(top_)
  , right(right_)
  , bottom(bottom_)
  {
  }
};

// 基準ポイント定義
struct stBasicPoints
{
  CPoint start_point;
  CPoint end_point;

  // コンストラクタ
  stBasicPoints(void)
  : start_point(0, 0)
  , end_point(0, 0)
  {
  }
  
  // コンストラクタ (始点、終点)
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

// 障害物ポイント距離定義
struct stObstaclePointsDis
{
  int distance;
  int angle;
  CPoint point;
  
  // コンストラクタ
  stObstaclePointsDis(void)
  : distance(0)
  , angle(0)
  , point(0, 0)
  {
  }
  
  // コンストラクタ (距離、角度、位置)
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

// 障害物ポイント角度定義
struct stObstaclePointsAng
{
  int angle;
  int distance;
  CPoint point;
  
  // コンストラクタ
  stObstaclePointsAng(void)
  : angle(0)
  , distance(0)
  , point(0, 0)
  {
  }
  
  // コンストラクタ (角度、距離、位置)
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

// 障害物構成定義
struct stObstacleStruct
{
  stBasicPoints width_struct;
  stBasicPoints height_struct;
  
  // コンストラクタ
  stObstacleStruct(void)
  : width_struct()
  , height_struct()
  {
  }
  
  // コンストラクタ (横構成、縦構成)
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

// Ccalculation ダイアログ
class Ccalculation
{
  public:
    // 始点を軸に指定距離、指定角度で回転させた座標を求める
    void CalcLineRotatePos(float sx, float sy, float ex, float ey, float r, float theta, float& x, float& y);
    
    // 始点を軸に指定距離、指定角度で回転させた座標を求める。
    void CalcLineRotatePosRad(float sx, float sy, float ex, float ey, float r, float rad, float& x, float& y);
    
    // 直線の方程式を求める
    bool CalcLineEquation(float sx, float sy, float ex, float ey, float& a, float& b);
    
    // 2線分の交点を求める。
    bool GetLineClossPoint(int sx1, int sy1, int ex1, int ey1, int sx2, int sy2, int ex2, int ey2, int& rx, int& ry);
    
    // 2点間の距離を求める
    int GetDistance(int sx, int sy, int ex, int ey);

    // 点と直線の距離を取得する
    double GetPointToLineDistace(CPoint& point, CPoint& sp, CPoint& ep);
};

// CobstacleAvoidance ダイアログ
class CobstacleAvoidance
{
  public:
    Ccalculation m_calculation;            // 計算用
    vector< std::vector<int> > m_map_data; // 地図データ
    stBasicPoints m_road_destination;      // 経路始終点の基準ポイント
    vector<CPoint> m_mid_point_list;       // 途中ポイントリスト
    vector<stObstaclePointsDis> m_obstacle_points_list;  // 障害物の点群リスト
    vector<stObstacleStruct> m_obstacle_struct_list;     // 障害物の構成リスト
    vector<CPoint> m_best_road_point;                    // 最適経路

    // コンストラクション
    CobstacleAvoidance();

  protected:
    // 地図データを取得
    void mapCallback(const nav_msgs::OccupancyGrid::ConstPtr& map);

    // 経路始終点を取得
    //void roadDestinationCallback(const road_destination);

    // 障害物情報を取得
    //void obstaclePointsCallback(const obstacle_points);

    // 探索指示を取得
    //void scanCallback(const scan);

  private:
    bool m_start_line_closs;
    int m_map_height;         // 地図の高さ
    int m_map_width;          // 地図の幅
    int m_best_road_distance; // 最短距離
    
    // 初期化
    void init(void);
    
    // 各障害物の構成を算出
    void GetObstacleStruct(void);
    
    // 途中点リストを算出
    void GetMidPointList(void);
    
    // 障害物回避最適ルート探索
    void FindBestRoad(void);

   // 中途点により経路を探索
    void FindRoadByPoints(int num_points);
    
    // 中途点により経路の全探索
    void FindAllRoad(int num_points, std::vector< std::vector<int> >& all_road_arrangement);

    // 中途点により経路の種類を生成
    void Portfolio(std::vector<int>& array, std::vector< std::vector<int> >& all_road_portfolio, int start, int end, int num_points, std::vector<int>& sel_array, int nsel_temp);

    // 経路の種類により経路の順番を生成
    void Arrangement(std::vector<int>& portfolio_list, std::vector< std::vector<int> >& all_road_arrangement, int start, int end);
};

#endif // OBSTACLE_AVOIDANCE_H
