//by hdp 2013.10.09
//////////////////////////////////////////////////////////////////////////
#include "stdafx.h"
#include <boost/thread.hpp>  

#include <CGAL/Cartesian.h>
#include <CGAL/CORE_algebraic_number_traits.h>
#include <CGAL/Arr_Bezier_curve_traits_2.h>
#include <CGAL/Arrangement_2.h>
//////////////////////////////////////////////////////////////////////////
typedef CGAL::CORE_algebraic_number_traits              Nt_traits;
typedef Nt_traits::Rational                             NT;
typedef Nt_traits::Rational                             Rational;
typedef Nt_traits::Algebraic                            Algebraic;
typedef CGAL::Cartesian<Rational>                       Rat_kernel;
typedef CGAL::Cartesian<Algebraic>                      Alg_kernel;
typedef Rat_kernel::Point_2                             Rat_point_2;
typedef CGAL::Arr_Bezier_curve_traits_2<Rat_kernel, Alg_kernel, Nt_traits>
	Traits_2;
typedef Traits_2::Curve_2                               Bezier_curve_2;
typedef CGAL::Arrangement_2<Traits_2>                   Arrangement_2;

typedef struct PointF
{
	PointF(float fx, float fy)
	{
		X = fx;
		Y = fy;
	}

	float X;
	float Y;
};
//////////////////////////////////////////////////////////////////////////
std::list<Rat_point_2>* pIntersctions = NULL;
//////////////////////////////////////////////////////////////////////////
bool EqualPointF(PointF p1, PointF p2)
{
	const float epsilon = 0.0001f;
	return abs(p1.X - p2.X) < epsilon && abs(p1.Y - p2.Y) < epsilon;
}
//////////////////////////////////////////////////////////////////////////
//计算相交点，返回交点数量
extern "C" __declspec(dllexport) int WINAPI CalculateIntersection(const int lineCount, const PointF lines[], const unsigned char types[])
{
	if (lineCount < 2)
		return 0;

	Arrangement_2 arr;
	CGAL::_Bezier_cache<Nt_traits> cache;
	std::list<Bezier_curve_2> curves;
	std::list<Rat_point_2> pts;
	if (pIntersctions == NULL)
		pIntersctions = new std::list<Rat_point_2>();
	pIntersctions->clear();

	PointF begin(0 ,0);
	int beginIndex = -1;

	for (int i = 0; i < lineCount - 1; i++)		//lineCount-1
	{
		int typeSign = types[i] & 0xF0;
		int pointSign = types[i+1] & 0x0F;

		//record begin point
		if (types[i] == 0)
		{
			begin = lines[i];
			beginIndex = i;
		}
		else if (typeSign == 0x80)				//end point
		{
			if (i != (beginIndex + 1))			//1 line,not close
			{
				if (!EqualPointF(lines[i], begin)) //same point,cancel
				{
					pts.push_back(Rat_point_2(lines[i].X, lines[i].Y));
					pts.push_back(Rat_point_2(begin.X, begin.Y));
					curves.push_back(Bezier_curve_2(pts.begin(), pts.end()));
					pts.clear();
				}
			}
		}

		if (pointSign == 1)					//line
		{
			if (!EqualPointF(lines[i], lines[i+1]))
			{
				pts.push_back(Rat_point_2(lines[i].X, lines[i].Y));
				pts.push_back(Rat_point_2(lines[i+1].X, lines[i+1].Y));
				curves.push_back(Bezier_curve_2(pts.begin(), pts.end()));
				pts.clear();
			}
		}
		else if (pointSign == 3)			//bezier
		{
			pts.push_back(Rat_point_2(lines[i].X, lines[i].Y));
			if (pts.size() == 3)			//4 point create a bezier
			{
				pts.push_back(Rat_point_2(lines[i+1].X, lines[i+1].Y));
				curves.push_back(Bezier_curve_2(pts.begin(), pts.end()));
				pts.clear();
			}
		}
	}
	//last data
	if ((types[lineCount-1]  & 0xF0) == 0x80)	
	{
		if ((lineCount - 1) != (beginIndex + 1))		//1 line,not close
		{
			if (!EqualPointF(lines[lineCount-1], begin))
			{
				pts.push_back(Rat_point_2(lines[lineCount-1].X, lines[lineCount-1].Y));
				pts.push_back(Rat_point_2(begin.X, begin.Y));
				curves.push_back(Bezier_curve_2(pts.begin(), pts.end()));
				pts.clear();
			}
		}
	}

	insert(arr, curves.begin(), curves.end());
	Arrangement_2::Vertex_iterator vit = arr.vertices_begin();
	for (;vit != arr.vertices_end(); vit++)
	{
		if ((*vit).degree() > 2)		//multi line
		{
			vit->point().make_exact(cache);
			double x = CGAL::to_double(vit->point().x());
			double y = CGAL::to_double(vit->point().y());
			pIntersctions->push_back(Rat_point_2(x, y));
		}
	}

	return pIntersctions->size();
}
//获取相交点数据
extern "C" __declspec(dllexport) void WINAPI GetIntersection(float intersX[],  float intersY[])
{
	if (pIntersctions == NULL)
		return;
	
	std::list<Rat_point_2>::iterator it = pIntersctions->begin();
	for (int i = 0; it != pIntersctions->end(); i++, it++)
	{
		intersX[i] = (*it).x().doubleValue();
		intersY[i] = (*it).y().doubleValue();
	}

	delete pIntersctions;
	pIntersctions = NULL;
}


