#include "obstacle_avoidance.h"

#define ROUND(x) (x < 0.0f ? (int)floor((x) + 0.999) : (int)floor((x) + 0.5))  // 四捨五入
#define PI 3.14159265358979323846

static const int MAP_OBSTACLE_DATAE = -1;      // 地図上障害物データ
static const int MAX_ROAD_DISTANCE = 1000000;  // 初期化最大距離
static const int MID_POINTS_DISTANCE = 5;      // 途中点生成範囲有効距離
static const int MID_POINT_COUNT = 5;          // 途中点数
static const int OBSTACLE_DISTANCE = 3;        // 有効距離
static const int OBSTACLE_ANGLE = 3;           // 有効角度
static const int AVOIDANCE_DISTANCE = 5;       // 回避距離

// 始点を軸に指定距離、指定角度で回転させた座標を求める
void Ccalculation::CalcLineRotatePos(float sx, float sy, float ex, float ey, float r, float theta, float& x, float& y)
{
  double rad = PI / 180.0f * theta;
  CalcLineRotatePosRad(sx, sy, ex, ey, r, (float)rad, x, y);
}

// 始点を軸に指定距離、指定角度で回転させた座標を求める
void Ccalculation::CalcLineRotatePosRad(float sx, float sy, float ex, float ey, float r, float rad, float& x, float& y)
{
  float a = 0.0f, b = 0.0f;
  float tmp_x, tmp_y;
  float p_x, p_y;
  float d1, d2;
  
  // 直線の方程式を求める
  if(CalcLineEquation(sx, sy, ex, ey, a, b) == false)
  {
    // 垂直の時
    tmp_x = sx;
    tmp_y = sy + r;
    d1 = sqrt(pow((float)(ex - tmp_x), 2) + pow((float)(ey - tmp_y), 2));
    
    p_x = sx;
    p_y = sy - r;
    d2 = sqrt(pow((float)(ex - p_x), 2) + pow((float)(ey - p_y), 2));
  }
  else
  {
    tmp_x = sx + (r / sqrt(a * a + 1));
    tmp_y = a * tmp_x + b;
    d1 = sqrt(pow((float)(ex - tmp_x), 2) + pow((float)(ey - tmp_y), 2));
    
    p_x = sx - (r / sqrt(a * a + 1));
    p_y = a * p_x + b;
    d2 = sqrt(pow((float)(ex - p_x), 2) + pow((float)(ey - p_y), 2));
  }
  
  // 線分上の座標を選択
  if(d1 > d2)
  {
    tmp_x = p_x;
    tmp_y = p_y;
  }
  
  // 始点(sx,sy)を軸に任意の点(nx,ny)をθ(度)回転
  x = (cos(rad) * tmp_x) - (sin(rad) * tmp_y) + (sx - sx * cos(rad) + sy * sin(rad));
  y = (sin(rad) * tmp_x) + (cos(rad) * tmp_y) + (sy - sx * sin(rad) - sy * cos(rad));
}

// 直線の方程式を求める
bool Ccalculation::CalcLineEquation(float sx, float sy, float ex, float ey, float& a, float& b)
{
  a = b = 0.0f;
  // 垂直の場合は直線式は求まらない
  if(abs(ex - sx) < 1.0f) return false;
  
  // 直線の方程式
  a = (float)(ey - sy) / (float)(ex - sx);
  b = (float)(ex * sy - sx * ey) / (float)(ex - sx);
  return true;
}

// 2線分の交点を求める。
bool Ccalculation::GetLineClossPoint(int sx1, int sy1, int ex1, int ey1, int sx2, int sy2, int ex2, int ey2, int& rx, int& ry)
{
  long long v1, v2;
  long long m1, m2;
  long long is1, is2;
  double ds1, ds2;
  
  v1 = (sx2 - sx1)*(ey1 - sy1) - (sy2 - sy1)*(ex1 - sx1);
  v2 = (ex2 - sx1)*(ey1 - sy1) - (ey2 - sy1)*(ex1 - sx1);
  
  m1 = (sx1 - sx2)*(ey2 - sy2) - (sy1 - sy2)*(ex2 - sx2);
  m2 = (ex1 - sx2)*(ey2 - sy2) - (ey1 - sy2)*(ex2 - sx2);
  
  is1 = (ex2 - sx2)*(sy1 - sy2) - (ey2 - sy2)*(sx1 - sx2);
  is2 = (ex2 - sx2)*(sy2 - ey1) - (ey2 - sy2)*(sx2 - ex1);
  
  if((v1*v2 <= 0) && (m1*m2 <= 0) && (is1 + is2 != 0))
  {
    ds1 = is1 / 2.0f;
    ds2 = is2 / 2.0f;
    
    // ベクトル上に交点が存在するとき
    rx = sx1 + ROUND((ex1 - sx1)*ds1 / (ds1 + ds2));
    ry = sy1 + ROUND((ey1 - sy1)*ds1 / (ds1 + ds2));
    return true;
  }
  return false;
}

// 2点間の距離を求める。
int Ccalculation::GetDistance(int sx, int sy, int ex, int ey)
{
  double d = sqrt(pow((double)(ex - sx), 2) + pow((double)(ey - sy), 2));
  return ROUND(d);
}

// 点と直線の距離を取得する
double Ccalculation::GetPointToLineDistace(CPoint& point, CPoint& sp, CPoint& ep)
{
  CPoint AB,AP;
  double D, L, H;
  
  AB.x = ep.x - sp.x;
  AB.y = ep.y - sp.y;
  AP.x = point.x - sp.x;
  AP.y = point.y - sp.y;
  
  D = abs(AB.x * AP.y - AB.y * AP.x);
  L = pow((ep.x - sp.x) * (ep.x - sp.x) + (ep.y - sp.y) * (ep.y - sp.y), 0.5);
  H = D / L;
  return H;
}

// 地図データを取得
void CobstacleAvoidance::mapCallback(const nav_msgs::OccupancyGrid::ConstPtr& map)
{
  m_map_height = map->info.height;
  m_map_width = map->info.width;
  vector<int> map_width_data;
  m_map_data.clear();
  for(int i = 0; i < m_map_height; i++)
  {
    for(int j = 0; j < m_map_width; j++)
    {
       map_width_data.push_back(map->data[i * m_map_width + j]);
    }
    m_map_data.push_back(map_width_data);
    map_width_data.clear();
  }
}

/*
// 経路始終点を取得
void CobstacleAvoidance::roadDestinationCallback(const road_destination)
{
  m_road_destination.Clear();
  m_road_destination.start_point.x = road_destination->info.start_point.x;
  m_road_destination.start_point.y = road_destination->info.start_point.y;
  m_road_destination.end_point.x = road_destination->info.end_point.x;
  m_road_destination.end_point.y = road_destination->info.end_point.y;
}

// 障害物情報を取得
void CobstacleAvoidance::obstaclePointsCallback(const obstacle_points)
{
  m_obstacle_points_list.clear();
  
  for (int i = 0; i < obstacle_points->info.count; i++)
  {
    int distance = obstacle_points->info.list[i].distance;
    int angle = obstacle_points->info.list[i].angle;
    CPoint point(obstacle_points->info.list[i].point.x, obstacle_points->info.list[i].point.y);
    stObstaclePointsDis obstacle_point_dis(distance, angle, point);
    m_obstacle_points_list.push_back(obstacle_point_dis);
  }
  this->GetObstacleStruct();
}

// 探索指示を取得
void CobstacleAvoidance::scanCallback(const scan)
{
  if(!scan) return;
  if(m_road_destination.empty()) return;
  if(m_obstacle_points_list.empty()) return;
  if(m_obstacle_struct_list.empty()) return;
  
  this->FindBestRoad();
}
*/

// コンストラクション
CobstacleAvoidance::CobstacleAvoidance()
{
  init();
  ros::NodeHandle handle_;
  ros::Subscriber subMap = handle_.subscribe("map", 1, &CobstacleAvoidance::mapCallback, this);
  //ros::Subscriber subRoadDestination = handle_.subscribe("road_destination", 1, &CobstacleAvoidance::roadDestinationCallback, this);
  //ros::Subscriber subObstaclePoints = handle_.subscribe("obstacle_points", 1, &CobstacleAvoidance::obstaclePointsCallback, this);
  //ros::Subscriber subURG = handle_.subscribe("scan", 1, &CobstacleAvoidance::scanCallback, this);
}

// 初期化
void CobstacleAvoidance::init(void)
{
  m_start_line_closs = false;
  m_map_height = 0;
  m_map_width = 0;
  m_best_road_distance = MAX_ROAD_DISTANCE;
  m_map_data.clear();
  m_mid_point_list.clear();
  m_obstacle_points_list.clear();
  m_obstacle_struct_list.clear();
  m_best_road_point.clear();
}

// 各障害物の構成を算出
void CobstacleAvoidance::GetObstacleStruct(void)
{
  m_obstacle_struct_list.clear();
  if(m_obstacle_points_list.empty()) return;
  sort(m_obstacle_points_list.begin(), m_obstacle_points_list.end());
  
  int average_distance = m_obstacle_points_list[0].distance;
  std::vector<stObstaclePointsAng> m_obstacle_points_ang;

  // 有効距離と有効角度を用いて、障害物の点群をグループ化
  for(int i = 0, ei = m_obstacle_points_list.size() - 1; i <= ei; ++i)
  {
    if(abs(m_obstacle_points_list[i].distance - average_distance) <= OBSTACLE_DISTANCE)
    {
      average_distance = (m_obstacle_points_list[i].distance + average_distance) / 2;
      stObstaclePointsAng points_ang(m_obstacle_points_list[i].angle, m_obstacle_points_list[i].distance, m_obstacle_points_list[i].point);
      m_obstacle_points_ang.push_back(points_ang);
    }
    if(abs(m_obstacle_points_list[i].distance - average_distance) > OBSTACLE_DISTANCE || i == ei)
    {
      sort(m_obstacle_points_ang.begin(), m_obstacle_points_ang.end());
      int base_ang = m_obstacle_points_ang[0].angle;
      std::vector<stObstaclePointsAng> m_obs_ang_list;
      for(int j = 0, ej = m_obstacle_points_ang.size() - 1; j <= ej; ++j)
      {
        if(abs(m_obstacle_points_ang[j].angle - base_ang) <= OBSTACLE_ANGLE)
        {
          m_obs_ang_list.push_back(m_obstacle_points_ang[j]);
        }
        if(abs(m_obstacle_points_ang[j].angle - base_ang) > OBSTACLE_ANGLE || j == ej)
        {
          int min_x = m_map_width, max_x = 0, min_y = m_map_height, max_y = 0;
          CPoint point_left, point_right, point_top, point_bottom;

          for(int m = 0, em = m_obs_ang_list.size(); m < em; ++m)
          {
            stObstaclePointsAng &obs_po = m_obs_ang_list[m];
            if(obs_po.point.x < min_x) {min_x = obs_po.point.x; point_left = obs_po.point;}
            if(obs_po.point.x > max_x) {max_x = obs_po.point.x; point_right = obs_po.point;}
            if(obs_po.point.y < min_y) {min_y = obs_po.point.y; point_top = obs_po.point;}
            if(obs_po.point.y > max_y) {max_y = obs_po.point.y; point_bottom = obs_po.point;}
          }
          stBasicPoints width_struct(point_left, point_right);
          stBasicPoints height_struct(point_top, point_bottom);
          stObstacleStruct obstacle_struct(width_struct, height_struct);
          m_obstacle_struct_list.push_back(obstacle_struct);
          
          m_obs_ang_list.clear();
          m_obs_ang_list.push_back(m_obstacle_points_ang[j]);
        }
        base_ang = m_obstacle_points_ang[j].angle;
      }

      m_obstacle_points_ang.clear();
      if(i != ei)
      {
        average_distance = m_obstacle_points_list[i].distance;
        stObstaclePointsAng points_ang(m_obstacle_points_list[i].angle, m_obstacle_points_list[i].distance, m_obstacle_points_list[i].point);
        m_obstacle_points_ang.push_back(points_ang);
      }
    }
  }
}

// 途中点リストを算出
void CobstacleAvoidance::GetMidPointList()
{
  m_mid_point_list.clear();
  if(m_road_destination.IsEmpty()) return;
  if(m_map_data.empty()) return;
  
  // 乱数生成器で途中点を作成
  CPoint dsp = m_road_destination.start_point;
  CPoint dep = m_road_destination.end_point;

  // 移動範囲を生成
  int range = 0;
  if(abs(dsp.x - dep.x) > abs(dsp.y - dep.y)) range = abs(dsp.x - dep.x) / 2;
  else range = abs(dsp.y - dep.y) / 2;
  if(range > MID_POINTS_DISTANCE) range = MID_POINTS_DISTANCE;
  CPoint mip(static_cast<int>((dsp.x + dep.x) / 2), static_cast<int>((dsp.y + dep.y) / 2));
  int left = mip.x - range;
  if (left < 0) left = 0;
  int right = mip.x + range;
  if (right > m_map_width) right = m_map_width;
  int top = mip.y - range;
  if (top < 0) top = 0;
  int bottom = mip.y + range;
  if (bottom > m_map_height) bottom = m_map_height;
  CRect moving_range = CRect(left, top, right, bottom);

  // 途中点を算出
  int count = 0;
  while (count < MID_POINT_COUNT && !m_road_destination.IsEmpty())
  {
    srand((unsigned)time(NULL));
    int x = (rand() % (moving_range.right - moving_range.left + 1)) + moving_range.left;
    int y = (rand() % (moving_range.bottom - moving_range.top + 1)) + moving_range.top;
    if(m_map_data[x][y] != MAP_OBSTACLE_DATAE)
    {
      CPoint mid_point(x, y);
      m_mid_point_list.push_back(mid_point);
      count++;
    }
  }
  
  m_best_road_distance = MAX_ROAD_DISTANCE;
  m_best_road_point.clear();
}


//障害物回避最適ルート探索
void CobstacleAvoidance::FindBestRoad()
{
  m_best_road_distance = MAX_ROAD_DISTANCE;
  m_best_road_point.clear();

  // 障害物を回避するかどうかを判断
  for(int i = 0, ei = m_obstacle_struct_list.size(); i < ei; ++i)
  {
    stObstacleStruct& obstacle_struct = m_obstacle_struct_list[i];
    int rx, ry;
    CPoint dsp = m_road_destination.start_point;
    CPoint dep = m_road_destination.end_point;
    CPoint ws_sp = obstacle_struct.width_struct.start_point;
    CPoint ws_ep = obstacle_struct.width_struct.end_point;
    CPoint hs_sp = obstacle_struct.height_struct.start_point;
    CPoint hs_ep = obstacle_struct.height_struct.end_point;

    m_start_line_closs = m_calculation.GetLineClossPoint(dsp.x, dsp.y, dep.x, dep.y, ws_sp.x, ws_sp.y, ws_ep.x, ws_ep.y, rx, ry);
    m_start_line_closs |= m_calculation.GetLineClossPoint(dsp.x, dsp.y, dep.x, dep.y, hs_sp.x, hs_sp.y, hs_ep.x, hs_ep.y, rx, ry);
    if(m_start_line_closs) break;
  }

  if(!m_start_line_closs) return;
  if(m_mid_point_list.empty()) return;
  int count = 1;
  while(count <= MID_POINT_COUNT)
  {
    this->FindRoadByPoints(count);
    count++;
  }

  // 最適ルートがない場合
  if(m_best_road_point.empty()) return;
}

// 中途点により経路を探索
void CobstacleAvoidance::FindRoadByPoints(int num_points)
{
  std::vector< std::vector<int> > all_road_arrangement;
  this->FindAllRoad(num_points, all_road_arrangement);

  // 全探索経路を用いて、一つずつ判断
  for(int n = 0, en = all_road_arrangement.size(); n < en; ++n)
  {
    std::vector<int>& road_points = all_road_arrangement[n];
    int mid_distance_road = 0;
    std::vector<CPoint> mid_road_points;

    mid_road_points.push_back(m_road_destination.start_point);
    for(int m = 0, em = road_points.size(); n < em; ++m)
    {
      int point_index = road_points[m];
      mid_road_points.push_back(m_mid_point_list[point_index]);
    }
    mid_road_points.push_back(m_road_destination.end_point);

    for(int i = 0, ei = static_cast<int>(mid_road_points.size() - 1); i < ei; ++i)
    {
      CPoint dsp, dep;
      dsp = mid_road_points[i];
      dep = mid_road_points[i+1];
      bool line_closs = false;

      for(int j = 0, ej = m_obstacle_struct_list.size(); j < ej; ++j)
      {
        stObstacleStruct& obstacle_struct = m_obstacle_struct_list[j];
        CPoint ws_sp, ws_ep, hs_sp, hs_ep;
        int rx, ry;

        ws_sp = obstacle_struct.width_struct.start_point;
        ws_ep = obstacle_struct.width_struct.end_point;
        hs_sp = obstacle_struct.height_struct.start_point;
        hs_ep = obstacle_struct.height_struct.end_point;

        // 障害物と交差するかを調査
        line_closs = m_calculation.GetLineClossPoint(dsp.x, dsp.y, dep.x, dep.y, ws_sp.x, ws_sp.y, ws_ep.x, ws_ep.y, rx, ry);
        line_closs |= m_calculation.GetLineClossPoint(dsp.x, dsp.y, dep.x, dep.y, hs_sp.x, hs_sp.y, hs_ep.x, hs_ep.y, rx, ry);
        if(line_closs) break;

        std::vector<CPoint> obstacle_points;
        obstacle_points.push_back(ws_sp);
        obstacle_points.push_back(ws_ep);
        obstacle_points.push_back(hs_sp);
        obstacle_points.push_back(hs_ep);

        // 障害物との有効回避距離があるかを調査
        for(int k = 0, ek = obstacle_points.size(); k < ek; ++k)
        {
          CPoint& point = obstacle_points[k];
          double distance = m_calculation.GetPointToLineDistace(point, dsp, dep);
          line_closs |= (distance <= AVOIDANCE_DISTANCE);
          if(line_closs) break;
        }
        if(line_closs) break;
      }
      if(line_closs)
      {
        mid_distance_road = MAX_ROAD_DISTANCE;
        break;
      }
      mid_distance_road += m_calculation.GetDistance(dsp.x, dsp.y, dep.x, dep.y);
    }

    // 最適ルートを更新
    if(mid_distance_road > 0 && mid_distance_road < m_best_road_distance)
    {
      m_best_road_distance = mid_distance_road;
      m_best_road_point.clear();
      m_best_road_point = mid_road_points;
    }
  }
}

// 中途点により経路の全探索
void CobstacleAvoidance::FindAllRoad(int num_points, std::vector< std::vector<int> >& all_road_arrangement)
{
  std::vector<int> array(MID_POINT_COUNT);
  for(int i = 0; i < MID_POINT_COUNT; i++)
    array[i] = i;

  std::vector< std::vector<int> > all_road_portfolio;
  int nsel_temp = num_points;
  std::vector<int> sel_array(num_points);
  this->Portfolio(array, all_road_portfolio, 0, MID_POINT_COUNT - 1, num_points, sel_array, nsel_temp);
  if(all_road_portfolio.empty()) return;

  for(int j = 0, ej = all_road_portfolio.size(); j < ej; ++j)
  {
    std::vector<int>& portfolio_list = all_road_portfolio[j];
    this->Arrangement(portfolio_list, all_road_arrangement, 0, num_points - 1);   
  }
}

// 中途点により経路の種類を生成
void CobstacleAvoidance::Portfolio(std::vector<int>& array, std::vector< std::vector<int> >& all_road_portfolio, int start, int end, int num_points, std::vector<int>& sel_array, int nsel_temp)
{
  if(num_points == 0)
  {
    std::vector<int> points_list;
    for(int i = 0; i < nsel_temp; ++i)
      points_list.push_back(array[sel_array[nsel_temp - i - 1]]);

    all_road_portfolio.push_back(points_list);
    return;
  }

  for(int i = start; i <= end - num_points + 1; ++i)
  {
    sel_array[num_points - 1] = i;
    Portfolio(array, all_road_portfolio, i + 1, end, num_points - 1, sel_array, nsel_temp);
  }
}

// 経路の種類により経路の順番を生成
void CobstacleAvoidance::Arrangement(std::vector<int>& portfolio_list, std::vector< std::vector<int> >& all_road_arrangement, int start, int end)
{
  if(start == end)
  {
    all_road_arrangement.push_back(portfolio_list);
    return;
  }

  for(int i = start; i <= end; ++i)
  {
    if(i != start) std::swap(portfolio_list[start], portfolio_list[i]);
    Arrangement(portfolio_list, all_road_arrangement, start + 1, end);
    if(i != start) std::swap(portfolio_list[start], portfolio_list[i]);
  }
}

int main(int argc, char **argv)
{
  ros::init(argc, argv, "listener");
  CobstacleAvoidance obstacle_avoidance;
  ros::spin();
  return 0;
}
