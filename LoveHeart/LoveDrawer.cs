using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveHeart
{
    public class LoveDrawer
    {
        public LoveDrawer() { Creat_Data(); }
        public struct Point
        {
            public double x, y;
            public RawColor4 color;
        }
        public struct ParticleAxis
        {
            public List<Point> InsedePoint;
            public List<Point> OutsidePoint;
        }
        RawColor4[] colors = new RawColor4[] { new RawColor4(255, 32, 83, 255), new RawColor4(252, 222, 250, 255), new RawColor4(255, 0, 0, 255), new RawColor4(255, 0, 0, 255), new RawColor4(255, 2, 2, 255), new RawColor4(255, 0, 8, 255), new RawColor4(255, 5, 5, 255) };
        const int xScreen = 1200;
        const int yScreen = 800;
        const double PI = Math.PI;
        const double e = Math.E;
        const double averag_distance = 0.162;
        const int quentity = 506;
        const int circles = 210;
        const int frames = 20;
        Point[] origin_points = new Point[quentity];
        Point[] points = new Point[circles * quentity];
        public ParticleAxis[] image = new ParticleAxis[frames];
        double Screex_x(double x)
        {
            return x += xScreen / 2;
        }
        double Screex_y(double y)
        {
            return y = -y + yScreen / 2;
        }
        int Creat_Random(int x1,int x2)
        {
            if (x2 > x1)
                //return new Random().Next(0, (x2 - x1 + 1) + x1);
                return new Random().Next(x1, x2 + 1);
            else return 0;
        }
        void Creat_Data()
        {
            int index = 0; 
            
            double x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            for (double radian = 0.1; radian <= 2 * PI; radian += 0.005) 
            {
                x2 = 16 * Math.Pow(Math.Sin(radian), 3);
                y2 = 13 * Math.Cos(radian) - 5 * Math.Cos(2 * radian) - 2 * Math.Cos(3 * radian) - Math.Cos(4 * radian);
                //计算两点之间距离 (x1-x2)平方 + (y1-y2)平方
                double distance = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
                if (distance > averag_distance) 
                {
                    x1 = x2; y1 = y2;
                    origin_points[index].x = x2;
                    origin_points[index++].y = y2;
                }
            }
            index = 0;

            for (double size = 0.1; size <= 20; size += 0.1)
            {
                double success_p = 1 / (1 + Math.Pow(e, 8 - size / 2));
                for (int i = 0; i < quentity; ++i)
                {
                    if (success_p > Creat_Random(0, 100) / 100.0)
                    {
                        points[index].color = colors[Creat_Random(0, 6)];
                        points[index].x = size * origin_points[i].x + Creat_Random(-4, 4);  
                        points[index++].y = size * origin_points[i].y + Creat_Random(-4, 4);
                    }
                }
            }
            int points_size = index;
            for (int frame = 0; frame < frames; ++frame) 
            {
                image[frame].InsedePoint = new List<Point>();
                image[frame].OutsidePoint = new List<Point>();
                //内围粒子
                for (index = 0; index < points_size; ++index)
                {
                    double x = points[index].x, y = points[index].y;
                    //计算距离 根号下X平方+Y平方
                    double distance = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                    double distance_increase = -0.0009 * distance * distance + 0.35714 * distance + 5;

                    double x_increase = distance_increase * x / distance / frames;
                    double y_increase = distance_increase * y / distance / frames;

                    points[index].x += x_increase;
                    points[index].y += y_increase;

                    image[frame].InsedePoint.Add(new Point() { x = Screex_x(points[index].x), y = Screex_y(points[index].y), color = points[index].color });
                }
                //外围粒子
                for (double size = 17; size < 23; size += 0.3) 
                {
                    for (index = 0; index < quentity; ++index)
                    {
                        if ((Creat_Random(0, 100) / 100.0 > 0.6 && size >= 20) || (size < 20 && Creat_Random(0, 100) / 100.0 > 0.95))
                        {
                            double x, y;
                            if (size >= 20)
                            {
                                x = origin_points[index].x * size + Creat_Random(-frame * frame / 5 - 15, frame * frame / 5 + 15);
                                y = origin_points[index].y * size + Creat_Random(-frame * frame / 5 - 15, frame * frame / 5 + 15);
                            }
                            else
                            {
                                x = origin_points[index].x * size + Creat_Random(-5, 5);
                                y = origin_points[index].y * size + Creat_Random(-5, 5);
                            }
                            image[frame].OutsidePoint.Add(new Point() { x = Screex_x(x), y = Screex_y(y), color = colors[Creat_Random(0, 6)] });
                        }
                    }
                }
            }
        }
    }
}
